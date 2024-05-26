using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoItems;
using Xunit;

namespace Todo.Tests.ModelMappers
{
    public class TodoListSummaryViewmodelFactoryTests
    {

        [Fact]
        public void Create_WhenEmptyItemsList_Correct()
        {
            // Arrange
            var owner = new IdentityUser { Id = "1", UserName = "John Doe" };
            var todoList = new TestTodoListBuilder(owner, "Empty").Build();
            string userId = "1";

            // Act
            var result = TodoListSummaryViewmodelFactory.Create(todoList, userId);

            // Assert
            Assert.Equal(0, result.NumberOfNotDoneItems);
        }
        [Fact]
        public void Create_WhenMapTodoListToViewModel_Correct()
        {
            // Arrange
            var owner = new IdentityUser { Id = "1", UserName = "John Doe" };
            var todoList = new TestTodoListBuilder(owner, "Todo List")
                .WithItem("Task 1", Importance.High)
                .WithItem("Task 2", Importance.Low)
                .WithItem("Task 3", Importance.High)
                .Build();

            // Mark one task as done
            todoList.Items.First().IsDone = true;

            string userId = "1";

            // Act
            var result = TodoListSummaryViewmodelFactory.Create(todoList, userId);

            // Assert
            Assert.Equal(todoList.TodoListId, result.TodoListId);
            Assert.Equal(todoList.Title, result.Title);
            Assert.Equal(2, result.NumberOfNotDoneItems); // 2 out of 3 items are not done
            Assert.Equal(todoList.Owner.UserName, result.Owner.UserName);
            Assert.True(result.IsOwner);
        }

        [Fact]
        public void Create_WhenUserIdIsNull_IsOwnerIsFalse_Correct()
        {
            // Arrange
            var owner = new IdentityUser { Id = "1", UserName = "John Doe" };
            var todoList = new TestTodoListBuilder(owner, "Todo List").Build();
            string userId = null;

            // Act
            var result = TodoListSummaryViewmodelFactory.Create(todoList, userId);

            // Assert
            Assert.False(result.IsOwner);
        }

        [Fact]
        public void Create_WhenUserIdDifferentToOwner_IsOwnerIsFalse_Correct()
        {
            // Arrange
            var owner = new IdentityUser { Id = "1", UserName = "John Doe" };
            var todoList = new TestTodoListBuilder(owner, "Todo List").Build();
            string userId = "2";

            // Act
            var result = TodoListSummaryViewmodelFactory.Create(todoList, userId);

            // Assert
            Assert.False(result.IsOwner);
        }
    }
}