using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.EntityModelMappers.TodoLists
{
    public class TodoListDetailViewmodelFactory
    {
        private readonly Gravatar _gravatarService;

        public TodoListDetailViewmodelFactory(Gravatar gravatarService)
        {
            _gravatarService = gravatarService;
        }

        public async Task<TodoListDetailViewmodel> CreateAsync(TodoList todoList)
        {
            var items = await Task.WhenAll(todoList.Items.Select(item => TodoItemSummaryViewmodelFactory.CreateAsync(item, _gravatarService)));
            var orderedItems = items.OrderBy(x => x.Importance).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, orderedItems);
        }
    }
}