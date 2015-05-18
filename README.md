# bigrivers2015
Database, Models, Helpers, Frontend and Backend

SET UP PROJECT:
- Open file {root}\src\Bigrivers.sln in Visual Studio 2013;
- Open the Package Manager Console;
- Package Manager Console will report missing packages. Click "Restore" to restore these packages;
- Close Visual Studio completely and reopen the solution;
- Set Bigrivers.Server.Data as the startup project;
- Set the default project in the Package Manager Console to Bigrivers.Server.Data;
- In the Package Manager Console, run "update-database". The console may return the error "Automatic migration was not applied because it would result in data loss.", in this case run "update-database -force";
- Set the startup projects as Bigrivers.Client.WebApplication and Bigrivers.Client.Backend;

IN ORDER TO CONTRIBUTE TO THE PROJECT:
- Be familiar of the Git-flow workflow which is used in the Bigrivers Repo;
- In your local repository, create the following branches: Develop, Feature, Release, Hotfix. If you are using a git application which directly supports the Git-flow workflow, simply initialize Git-flow via this application with the default settings;