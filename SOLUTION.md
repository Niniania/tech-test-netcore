
forked, cloned 
built, ef update, run

1
Registered 2 users, added multiple lists and items, edited etc
noticed a Bug:
When editing a task the saved status does not populate correctly in drop down, it is always Medium 
This is also reflecting in one of the 3 tests not passing 
Fixed 

2
Notes on the arch
Why the use of name "Factory" for the model mapper? is it simple factory just to encapsulate the creation ?
ApplicationDbContextConvenience is a repository equivalent? and a service

3 
already done in step 1!

4 
Done

5
Two ways - jQuery and MVC (form submission to the be)

Requierements question - should this choice be remembered - should it be persisted ?
if so - per list basis or 1 setting per user ?

6
Also addedd a display if current logged in user is the owner or not, otherwise it might be confusing for the user

7
dotnet ef migrations add AddRankToTodoItem
dotnet ef database update
hidden the jquery sort, mvc makes more sense now with the sort combined

8 
to handle potential performance issue from gravatar Im calling it asynch, which then resulted in changing methods in the application to asynch
created model based on sample json from gravat api, only the values needed are serialised
added dependency injection for the services, as wanted to test and needed to mock 
