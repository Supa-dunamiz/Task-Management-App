using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using TaskApp.Interfaces;
using TaskApp.Models;
using TaskApp.Repository;
using TaskApp.ViewModels;

namespace TaskApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;

        public DashboardController(IDashboardRepository dashboardRepository, 
            IHttpContextAccessor httpContextAccessor,
            IPhotoService photoService,
            IUserRepository userRepository)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
            _userRepository = userRepository;
        }

        private void MapUserEdit(AppUser user, EditUserProfileViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.UserName = editVM.UserName;
            user.FirstName = editVM.FirstName;
            user.LastName = editVM.LastName;
            user.State = editVM.State;
            user.Country = editVM.Country;
            user.ProfileImageUrl = photoResult.Url.ToString();  
        }
        public async Task<IActionResult> Index()
        {
            var userTask = await _dashboardRepository.GetUserTasks();
            var dashboardViewModel = new DashboardViewModel
            {
                UserTasks = userTask
            };
            return View(dashboardViewModel);
        }
        public async Task<IActionResult> UserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userRepository.GetUserById(curUserId);
            var userDetail = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
                State = user.State,
                Country = user.Country,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(userDetail);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editVM = new EditUserProfileViewModel
            {
                Id = curUserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                State = user.State,
                Country = user.Country,
                
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(editVM);    
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }
            var user = await _dashboardRepository.GetUserByIdNoTracking(editVM.Id);
            if(user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to delete photo");
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
        }


    }
}

