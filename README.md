# LLamasoft API Development Exercise

The goal of this exercise to have you dive in and get your hands dirty with some code, while also having you dig in to some concepts to spur converation.

## Requirements

For this exercise, you will need basic knowledge of C# and WebAPI development in [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2). You'll also need an environment to build and run the code. Any environment you like should suffice. A good free IDE is Visual Studio Community, available [here](https://docs.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes).

## Back Story

A local (unnamed) bakery has requested your help. They contracted a developer to create an API to manage their menu, but they left before their work was complete. It is incomplete, buggy, and insecure. Your job will be to fix some of the problems outlined below, as well as writup some short discussions on how the project can be improved.

## Technologies we're using

We are using numerous technologies to aid in our development.
1) [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2) for our API
1) [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for our data layer (You will likely not need to modify the schema, don't worry!)
1) [Swagger](https://swagger.io/tools/swagger-ui/) for API documentation and development
1) [XUnit](https://xunit.github.io/) for unit testing
1) [FluentAssertions](https://fluentassertions.com/) for making our unit tests prettier.

## Getting Started

1) Pull code:
    ```
    git clone git:address
    ```
1) Open the the solution, BakeryExercise.sln
1) Make sure BakerExercise.Server is set as the startup project
1) You can start debugging by clicking the green play button, or pressing F5
    [Insert image here]
1) The solution should build and open a console window, as well your default web browser. Note: If your browser does not open and the service is running, you can open your browser of choice and navigate to http://localhost:5000/swagger/index.html
1) You should now see Swagger in your browser window.

You're now up and running!

## Making your first requests

### Unauthenticated Requests

1) In the browser, using Swagger, open the section named "Food".
1) Open the section for GET /api/food
1) Click "Try it out", then "Execute"
1) You should see the Server response of 200 with a response body:
    ```
    [
        {
            "foodId": "21def0fd-9364-45df-b619-11ee8a1303f8",
            "name": "Doughnut",
            "isVegan": false
        },
        {
            "foodId": "f01d1324-27c6-4b3b-ba43-e757d0dcd1b2",
            "name": "Muffin",
            "isVegan": false
        },
        {
            "foodId": "525f356f-eb7f-4edb-9975-542d7f58f41a",
            "name": "Bagel",
            "isVegan": false
        }
    ]
    ```
    This is a list of the various food available in the system. Each food has its own unique foodId (a [Guid](http://guid.one/)), along with a name, and a flag indicating if the food is vegan or not.

    Similar GET requests are available for other entities of the Bakery API, including Menus, MenuItems, and Portions.

### Authenticated Requests

Some of the requests for the Bakery API require you to be authenticated as an administrator in the system. Locate the request for creating menus, POST /api/menu, and notice the padlock icon at the top of that section. This indicates that this request requires authentication. The administator user account is seeded in the database.  

1) Attempt to make a request with the POST /api/menu.  You should get a 401 indicating you're not authorized.
1) Click on the padlock
1) Login with username "admin@bakery.com" and password "password" (no quotes on those)
1) Make the same request again.  You should get a successful response.

## Tests

You can run the unit tests from the Test Explorer dialog in Visual Studio. You can also run them from the dotnet CLI if you choose.

![Test Explorer](ReadmeContent/TestExplorer.png?raw=true "Test Explorer")

## Entity Structure

The database has a few main entities, described by the diagram below.

1) Users: Not related to any entity, just for authentication
1) Menus: Represents a menu
1) MenuItem: Indicates an item a customer could order
1) Food: The actual food being ordered
1) Portion: Indicated a particular amount of food

### An Example

We've seeded the system with the "Everyday Breakfast" menu. One of the items you can order off this menu is the "Huge Spread". This is comprised of a dozen muffins, a dozen doughnuts, and a dozen bagels.

In our system, this is modeled as:
```
├── Menu: "Everyday Breakfast"
    ├── MenuItem: "Huge Spread"
        ├── Portion: 12 Doughnuts
        │   └── Food: Doughnuts 
        ├── Portion: 12 Muffins
        │   └── Food: Muffins             
        └── Portion: 12 Bagels
            └── Food: Bagels 
```

Here's a diagram that shows the relationship

![Entity Diagram](ReadmeContent/Diagram.png?raw=true "Entity Diagram")

## Assignment

### Work Items

The Baker does not currently have a way to add a full menu to the system. In it's current state, no additional MenuItems, Portions, or Foods can be added. There are also numerous bugs in the system with the current endpoints. The Baker is requesting the following:

1) Add endpoints for adding menu items, portions, and additional foods
1) Add checks in all the current (and any new) endpoints to ensure that the user is notified when trying to access resources that do not exist.
1) Add an endpoint for deleting a menu. This should also delete associated menu items, portions, and food **only if they are not used by another menu**
1) Add unit tests to cover all the changes for new endpoints and functionality. Some of these tests are already stubbed out for you, and are marked with a Trait saying "TODO".

Notes:
1) **In order to call any endpoint that adds or deletes data from the system, the caller should be an admin**
1) Decide the proper [HTTP Verb](https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods) and [HTTP Response Code](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status). Be prepared to explain your decisions. 

### Points of Discussion

1) What areas of the code would you refactor, given the time? What if we didn't want to use Entity Framework anymore, and maybe Dapper. How would we prepare for that?
1) What areas of the code are important for testing? What areas are lacking tests that should have them? What other sorts of tests should our application support?
1) You'll notice that our controller methods all return a Task. What are the benefits of Tasks in C#?
1) Our application uses the "Basic" authentication schema. Is this a good choice? How about how we store user credentials?

## EF Notes

These commands can be run from the solution root folder

### Removing Migration

You should NOT need to change the schema for this exercise. If you need to, though, you can use the `dotnet ef` CLI tool.  To remove, add and update migrations and schema, you can use the commands below

```
dotnet ef migrations remove -f --project "BakeryExercise.EntityFramework\BakeryExercise.EntityFramework.csproj" --startup-project "BakeryExercise\BakeryExercise.Server.csproj"
```

### Creating Migration
```
dotnet ef migrations add Initial --project "BakeryExercise.EntityFramework\BakeryExercise.EntityFramework.csproj" --startup-project "BakeryExercise\BakeryExercise.Server.csproj"
```

### Updating Database Schema and Seed Data
```
dotnet ef database update --project "BakeryExercise.EntityFramework\BakeryExercise.EntityFramework.csproj" --startup-project "BakeryExercise\BakeryExercise.Server.csproj"
```

### Changing Seed Data and Schema
* You can find the seed data here: `BakeryExercise/BakeryExercise.EntityFramework/ModelDataSeeder.cs`
* You can find the schema setup here: `BakeryExercise/BakeryExercise.EntityFramework/ModelSchemaSetup.cs`