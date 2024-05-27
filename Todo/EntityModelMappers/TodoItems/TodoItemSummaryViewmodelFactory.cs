using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.EntityModelMappers.TodoItems
{
    public static class TodoItemSummaryViewmodelFactory
    {
        public static async Task<TodoItemSummaryViewmodel> CreateAsync(TodoItem ti, Gravatar gravatarService)
        {
            string displayName = null;
            var profile = await gravatarService.GetGravatarProfileAsync(ti.ResponsibleParty.Email);

            if (profile != null && profile.Entry != null && profile.Entry.Count > 0)
            {
                displayName = profile.Entry[0].DisplayName;
            }
            string imageUrl = "https://www.gravatar.com/avatar/" + Gravatar.GetHash(ti.ResponsibleParty.Email) + "?s=30";

            return new TodoItemSummaryViewmodel(ti.TodoItemId, ti.Title, ti.IsDone, UserSummaryViewmodelFactory.Create(ti.ResponsibleParty, displayName, imageUrl), ti.Importance, ti.Rank);
        }
    }
}