using System;
using static System.Net.WebRequestMethods;

namespace Todo.Models.TodoItems
{
    public class UserSummaryViewmodel
    {
        public string UserName { get; }
        public string Email { get; }
        public string ImageUrl { get; set; }
        public bool fromGravatar { get; set; }

        public UserSummaryViewmodel(string userName, string email, string displayName, string imageUrl)
        {
            UserName = String.IsNullOrEmpty( displayName)? userName:displayName;
            fromGravatar = !String.IsNullOrEmpty(displayName);
            Email = email;
            ImageUrl = imageUrl;
        }
    }
}