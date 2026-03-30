# Create Project
dotnet --version
dotnet new webapi -n BankingLite.Api

# Add Packages
dotnet add package Microsoft.EntityFrameworkCore --version=8.0.3
dotnet add package Microsoft.EntityFrameworkCore.Design --version=8.0.3
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version=8.0.3
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version=8.0.13
dotnet add package Microsoft.EntityFrameworkCore.Relational --version=8.0.3
dotnet add package Pomelo.EntityFrameworkCore.MySql --version=8.0.2 

# Environment setup
export MYSQL_ROOT_PASSWORD=your_password"
echo 'export MYSQL_ROOT_PASSWORD="your_password_here"' >> ~/.zshrc && source ~/.zshrc
echo $MYSQL_ROOT_PASSWORD

# Build and Run Project
dotnet build
dotnet run

# Migrations
dotnet ef migrations add initmigration
dotnet ef database update 

# Tests
dotnet new xunit -n BankingLite.Tests  
cd BankingLite.Tests 
dotnet add reference ../BankingLite.Api
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version=8.0.3
dotnet add package xunit --version 2.9.3
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.runner.visualstudio

dotnet restore
dotnet test

# Publish
dotnet publish -c Release