using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using TaskApp.Data;
using TaskApp.Interfaces;
using TaskApp.Models;
using TaskApp.ViewModels;

namespace TaskApp.Controllers
{
    public class UserTaskController : Controller
    {
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTaskController(IUserTaskRepository userTaskRepository, IHttpContextAccessor httpContextAccessor)
        {

            _userTaskRepository = userTaskRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UserTask> userTasks = await _userTaskRepository.GetAll();
            return View(userTasks);
        }

        public async Task<IActionResult> Detail(int id)
        {
            UserTask userTask = await _userTaskRepository.GetByIdAsync(id);
            return View(userTask);
        }

        public async Task<IActionResult> CurrentTask()
        {
             
            IEnumerable<UserTask> curTask = await _userTaskRepository.GetByPresentDateAsync();
            return View(curTask);
        }
        public IActionResult SearchByDate()
        {
            return View();
        }

        public async Task<IActionResult> TaskByDate(DateTime date)
        {
            IEnumerable<UserTask> userTasks = await _userTaskRepository.GetByDateAsync(date.Date);
            return View(userTasks);
        }
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createUserTaskVM = new CreateUserTaskViewModel { AppUserId = curUserId };
            return View(createUserTaskVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserTaskViewModel createTaskVM)
        {
            if (ModelState.IsValid)
            {
                var userTask = new UserTask
                {
                    Title = createTaskVM.Title,
                    Description = createTaskVM.Description,
                    DateOfTask = createTaskVM.DateOfTask,
                    AppUserId = createTaskVM.AppUserId,
                    TaskUpdate = createTaskVM.TaskUpdate,
                    TaskPriority = createTaskVM.TaskPriority
                };
                _userTaskRepository.Add(userTask);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View(createTaskVM);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var userTask = await _userTaskRepository.GetByIdAsync(id);
            if (userTask == null)
            {
                return View("Error");
            }
            var userTaskVM = new EditUserTaskViewModel
            {
                Title = userTask.Title,
                AppUserId = userTask.AppUserId,
                Description = userTask.Description,
                DateOfTask = userTask.DateOfTask,
                TaskUpdate = userTask.TaskUpdate,
                TaskPriority = userTask.TaskPriority
            };
            return View(userTaskVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditUserTaskViewModel editTaskVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View(editTaskVM);
            }

            var userTask = new UserTask
            {
                Id = id,
                Title = editTaskVM.Title,
                Description = editTaskVM.Description,
                AppUserId = editTaskVM.AppUserId,
                DateOfTask = editTaskVM.DateOfTask,
                TaskUpdate = editTaskVM.TaskUpdate,
                TaskPriority = editTaskVM.TaskPriority
            };
            _userTaskRepository.Update(userTask);
            return RedirectToAction("Index", "Dashboard");
        }


        public async Task<IActionResult> EditStatus(int id)
        {
            var userTask = await _userTaskRepository.GetByIdAsync(id);
            if (userTask == null)
            {
                return View("Error");
            }
            var userTaskVM = new EditUserTaskViewModel
            {
                Title = userTask.Title,
                AppUserId = userTask.AppUserId,
                Description = userTask.Description,
                DateOfTask = userTask.DateOfTask,
                TaskUpdate = userTask.TaskUpdate,
                TaskPriority = userTask.TaskPriority
            };
            return View(userTaskVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditStatus(int id, EditUserTaskViewModel editTaskVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View(editTaskVM);
            }

            var userTask = new UserTask
            {
                Id = id,
                Title = editTaskVM.Title,
                Description = editTaskVM.Description,
                AppUserId = editTaskVM.AppUserId,
                DateOfTask = editTaskVM.DateOfTask,
                TaskUpdate = editTaskVM.TaskUpdate,
                TaskPriority = editTaskVM.TaskPriority
            };
            _userTaskRepository.Update(userTask);
            return RedirectToAction("Index", "Dashboard");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var userTask = await _userTaskRepository.GetByIdAsync(id);
            if (userTask == null)
            {
                return View("Error");
            }
            return View(userTask);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userTask = await _userTaskRepository.GetByIdAsync(id);
            if (userTask == null)
            {
                return View("Error");
            }
            _userTaskRepository.Delete(userTask);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
