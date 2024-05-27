using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Xunit;

namespace Todo.Tests.ModelMappers
{
    public class WhenTodoItemIsConvertedToEditFields
    {
        private readonly TodoItem srcTodoItem;
        private readonly TodoItemEditFields resultFields;

        public WhenTodoItemIsConvertedToEditFields()
        {
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem(new TodoItemEditFields() { Title = "bread", Importance = Importance.High, Rank = 1 })
                    .Build()
                ;

            srcTodoItem = todoList.Items.First();

            resultFields = TodoItemEditFieldsFactory.Create(srcTodoItem);
        }

        [Fact]
        public void EqualTodoListId()
        {
            Assert.Equal(srcTodoItem.TodoListId, resultFields.TodoListId);
        }

        [Fact]
        public void EqualTitle()
        {
            Assert.Equal(srcTodoItem.Title, resultFields.Title);
        }

        [Fact]
        public void EqualImportance()
        {
            Assert.Equal(srcTodoItem.Importance, resultFields.Importance);
        }
    }
}