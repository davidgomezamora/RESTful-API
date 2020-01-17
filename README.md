# RESTful-API
Base guide for building and settings of the RESTful API with ASP.NET Core 3.0.

## Evernote notes
For more information and to read the documentation learned in the Pluralsight course, see the [link](https://www.evernote.com/client/web?login=true#?anb=true&b=61a4f299-ded9-4889-b75d-f7c27dc65fe8&n=52ff78c3-854f-4027-88e8-1579f78d7c00&s=s726&search=v4&).

## Test database
To test the RESTful API with connection to a database, [download](https://docs.microsoft.com/es-es/sql/samples/adventureworks-install-configure) the Microsoft Adventureworks test database.

## Implement template
To implement the template, it is necessary to rename the Visual Studio 2019 solution like this:

  1. Open the solution
  2. From Visual Studio 2019, rename the solution.
  3. Close the solution
  4. Rename the folder
  5. Open the WebAPI project file (.csproj) with a text editor
  6. Rename the <RootNamespace> tag to the name of the solution indicated in the second step.
