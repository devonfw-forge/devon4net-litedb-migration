:: runs commands to test and generate code coverage report 
:: place in /<RootProjectDirectory>/CodeCoverage
:: use terminal to run script
:: this script uses the tool dotnet-reportgenerator-globaltool
:: please install it using dotnet tool install -g dotnet-reportgenerator-globaltool
cd %~dp0 
cd..
set "projectPath=%CD%"
set excludeClasses=-Program;^
-Devon4Net.Application.WebAPI;^
-Devon4Net.Application.WebAPI.Implementation.Domain.*;^
-Devon4Net.Application.WebAPI.Implementation.Configuration.*;^
-Devon4Net.Application.WebAPI.Implementation.Data.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.AnsibleTowerManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.AntiForgeryTokenManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.AuthManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.CyberArkManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.EmployeeManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.MediatRManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.RabbitMqManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.SmaxHcmrManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.TodoManagement.*;^
-Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto.ExtraDto;^
-Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto.CategoryDto;^
-Devon4Net.Application.WebAPI.Implementation.Business.CategoryManagement.Dto.CategoryDto;^
-Devon4Net.Application.WebAPI.Implementation.Business.CategoryManagement.Converters.CategoryConverter;^
-Devon4Net.Application.WebAPI.Implementation.Business.CategoryManagement.Controllers.CategoryController;
cd %projectPath%\Templates\WebAPI\Devon4Net.Test 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Exclude="[xunit.*]*" /p:CoverletOutput="%projectPath%\CodeCoverage\test_result\coverage.cobertura.xml" 
reportgenerator "-reports:%projectPath%\CodeCoverage\test_result\coverage.cobertura.xml" "-targetdir:%projectPath%\CodeCoverage\report" "-classfilters:%excludeClasses%" 
:: start %projectPath%\CodeCoverage\report\index.html
:: pause
