# VV Graph

VV Graph is a solution that contains tools, web services and web UI that together form an application which allows users to store and visualize graphs and to find shortest paths between selected nodes.

The application demonstrates the use of the following concepts and technologies in **C# 6** and **.NET Framework 4.6.1**:

1. **Domain Driven Design**
2. **.NET Core project system** targeting full .NET Framework 4.6.1.
3. **Neo4j** graph database to store graphs and to run graph-native queries like finding the shortest path.
4. **ASP.NET Core REST API** to get/store graphs and to find shortest path.
  - and, for comparison, the same API implemented in **WCF HTTP Services**.
5. **.NET Core console application** with smart command interface to load graph data stored in files into the API.
7. **REST API C# proxy** that can be distributed as a NuGet package.
8. **ASP.NET Core single-page application** build with **AngularJS** and **vis.js** to visualize graphs and to look for shortest paths using the REST API. All packages are imported using **bower**.

## How to run

### Requirements

1. [Visual Studio 2015 Update 3](https://www.visualstudio.com/en-us/news/releasenotes/vs2015-update3-vs)
2. [.NET Core SDK and .NET Core 1.0.0 VS 2015 Tooling Preview 2](https://www.microsoft.com/net/core#windows)
  - Make sure you uninstall all .NET Core, .NET CLI or DNX-related previews first!
3. [Neo4j Community Edition](https://neo4j.com/download/)

### Steps

1. **Clone the repository** to a local folder: https://github.com/vojtechvit/VVGraph.git
2. **Start Neo4j** Community Edition. 
  - You may open the  Neo4j web administration to see the changes there, but *do not change the password!*
2. Run the PowerShell script in `src\RunAll.ps1`
  - [x] The ASP.NET Core single-page application starts on URL: [http://localhost:5000/](http://localhost:5000/)
    - Use the Graph URL input field to display your graph. The URL should follow this template:<br/>
      `{web-service-base-url}/graphs/{graph-name}`.
    - To select multiple nodes for shortest path search, use `[ctrl]+(click)` or a longheld click (or touch).
  - [x] The ASP.NET Core REST API starts on URL: [http://localhost:5001/api/v1/](http://localhost:5001/api/v1/)
    - You may test the API using the Postman collection in `samples/postman` folder.
  - [x] The Data Loader loads data from folder `samples\input-data` as a graph called `graph1`.

If you want to run the **WCF HTTP Services**:

1. **Open the solution** in Visual Studio 2015 Update 3
2. **Build the solution**.
  - Project `WebServices.Wcf` will fail to build initially because it doesn't reference the other projects by a project reference; instead, it references the dlls. Project reference doesn't seem to work in this tooling preview. On the second run it should succeed.
3. Right-click the project named `WebServices.Wcf`, set it as a startup project.
4. Hit `[ctrl]+[f5]`.
5. The service should start on URL: [http://localhost:5002/GraphService.svc/](http://localhost:5002/GraphService.svc/)
  - Ignore the WCF client window that probably opens.
  - You may test the API using the Postman collection in `samples/postman` folder.