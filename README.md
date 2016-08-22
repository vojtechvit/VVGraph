# VV Graph

VV Graph is a solution that contains tools, web services and web UI that together form an application which allows users to store and visualize graphs and to find shortest paths between selected nodes.

The application demonstrates the use of the following concepts and technologies in **C# 6** and **.NET Framework 4.6.1**:

1. **Domain Driven Design**
2. The new **.NET Core project** system targeting full .NET Framework 4.6.1.
3. **neo4j** graph database to store graphs and to run graph-native queries like finding the shortest path.
4. **ASP.NET Core** RESTful API to get/store graphs and to find shortest path.
  - and, for comparison, the same API implemented in **WCF HTTP Services**.
5. **.NET Core console application** with smart command interface to load graph data stored in files into the API.
7. **REST API C# proxy** that can be distributed as a NuGet package.
8. **ASP.NET Core single-page application** build with **AngularJS** and **vis.js** to visualize graphs and to look for shortest paths using the REST API.

## How to run

### Requirements

1. [Visual Studio 2015 Update 3](https://go.microsoft.com/fwlink/?LinkId=691129)
2. [.NET Core SDK and .NET Core 1.0.0 VS 2015 Tooling Preview 2](https://www.microsoft.com/net/core#windows)
  - Make sure you uninstall all .NET Core, .NET CLI or DNX-related previews first!
3. [neo4j](https://neo4j.com/) graph database.


1. Clone the repository to a local folder: `https://github.com/vojtechvit/VVGraph.git`
2. Open the solution in Visual Studio 2015 Update 3
3. Build the solution.
  - Project `WebServices.Wcf` will fail initially because it doesn't reference the other projects by a project reference; instead, it references the dlls. Project reference doesn't seem to work in this tooling preview. On the second run it should succeed.
4. Run your neo4j Community Edition. Enter the web-based administration. Note your username and password.
5. To run ASP.NET Core version of web services:
  1. Make sure that the neo4j URI (including credentials) in `WebServices.AspNetCore` project's `appsettings.json` is correct.
  2. Right-click the project named `WebServices.AspNetCore`, set it as a startup project.
  3. Hit `[ctrl]+[f5]`.
  4. Note your service base URL, e.g. `http://localhost:60832/api/v1/`.
  - Alternatively, you may just run in the project folder in the command line a command `dotnet run`.
6. To run WCF version of web services:
  1. Make sure that the neo4j URI (including credentials) in `WebServices.Wcf` project's `Web.config` is correct.
  2. Right-click the project named `WebServices.Wcf`, set it as a startup project.
  3. Hit `[ctrl]+[f5]`. Ignore the window with a WCF client that opens.
  4. Note your service base URL, e.g. `http://localhost:62017/GraphService.svc/`.
7. To run data loader:
  1. Right-click the project named `DataLoader` and Publish it.
  2. Go to the folder to which the data loader was publisher.
  3. Open Command Line and run `dataloader load-graph {graphName} -dir {input-files-directory} -url {web-services-base-url}`.
  - You can check in neo4j web administration that a new graph was added.
8. To run the UI:
  1. Right-click the project named `Ui`, set it as a startup project.
  2. Hit `[ctrl]+[f5]`.
  3. Use the Graph URL input field to display your graph. The URL should follow this template: `{base-URL}/graphs/{graph-name}`. The web service on the `base-URL` must be running.
  - To select multiple nodes, use [ctrl] or a long-click.
