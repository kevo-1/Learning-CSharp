# Learning-C#


## General Structure:

| File-based | Project-based |
| - | - |
| a single `*.cs` file | a `*.csproj` file with multiple `*.cs` files|  
| uses `dotnet *.cs` to run the script/file | uses `dotnet new`, `dotnet build`, and `dotnet run` workflow to run the project

> You can transform your file-based project into a project-based on using [Dotnet project convert](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-project-convert)

**Top Level Statements** are used to run executable code directly at the root of the file without using a main method or wrapping it in a class, **(Only one file in the project-based app can use them)**


