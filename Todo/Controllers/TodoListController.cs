using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly TodoListDetailViewmodelFactory _todoListDetailViewmodelFactory;


        public TodoListController(ApplicationDbContext dbContext, IUserStore<IdentityUser> userStore, TodoListDetailViewmodelFactory todoListDetailViewmodelFactory)
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
            _todoListDetailViewmodelFactory = todoListDetailViewmodelFactory;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = dbContext.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists, userId);
            return View(viewmodel);
        }

        public async Task<IActionResult> Detail(int todoListId, bool hideCompleted = false, SortOrder sortOrder = SortOrder.Rank)
        {
            var todoList = await dbContext.SingleTodoListAsync(todoListId);
            var viewmodel = await _todoListDetailViewmodelFactory.CreateAsync(todoList);

            viewmodel.SortOrder = sortOrder;
            viewmodel.HideCompleted = hideCompleted;
            if (hideCompleted)
            {
                viewmodel.Items = viewmodel.Items.Where(x => !x.IsDone).ToList();
            }
            switch (sortOrder)
            {
                case SortOrder.Rank:
                    viewmodel.Items = viewmodel.Items.OrderBy(i => i.Rank).ToList();
                    break;
                case SortOrder.Importance:
                    viewmodel.Items = viewmodel.Items.OrderBy(i => i.Importance).ToList();
                    break;
            }
            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await dbContext.AddAsync(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new {todoList.TodoListId});
        }

    }
}