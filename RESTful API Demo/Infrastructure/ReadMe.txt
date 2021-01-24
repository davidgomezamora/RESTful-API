DATA LAYER - OVERVIEW

REQUIREMENTS
1. Database designed and deployed by Data Governance
	- Create database initial scripts
	- Deploy database
	- Create dev (and prod) credentials
	- Get Connection String --> <conn-string>

2. Project dependencies:
	Nuget Packages
	- Microsoft.EntityFrameworkCore.Tools
	- Microsoft.EntityFrameworkCore.SqlServer
	
3. Scaffold DB-Context Command
	- Open Package Manager Console in VStudio
	- Navigate to <Data Layer> Project

	EXECUTE:
	dotnet ef dbcontext scaffold "Data Source=localhost;Initial Catalog=Northwind;User ID=dev;Password=D3v;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer --context-dir Context --output-dir Entities --force --data-annotations

	Help: 
	dotnet ef dbcontext scaffold --help

4. Remove OnConfiguring method DBContext
	No connection string should be stored in this project!!!