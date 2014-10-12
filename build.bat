@ECHO OFF
cd /d %~dp0
set DOTNET_PATH="C:\Windows\Microsoft.NET\Framework64\v4.0.30319"
set SOLUTION_HOME="%CD%"
set PATH=%PATH%;%CD%\.nuget;%DOTNET_PATH%

:: build and pack API
msbuild App\SamplePlugin.App.sln /m /target:Clean,Build /property:Configuration=Release
nuget pack App\SamplePlugin.Api\SamplePlugin.Api.csproj -IncludeReferencedProjects -Prop Configuration=Release -OutputDirectory %SOLUTION_HOME%\NugetRelease

:: build and pack Catalog plugin
msbuild Plugins\Catalog\SamplePlugin.Catalog.sln /m /target:Clean,Build /property:Configuration=Release
nuget pack Plugins\Catalog\SamplePlugin.Catalog.Api\SamplePlugin.Catalog.Api.csproj -IncludeReferencedProjects -Prop Configuration=Release -OutputDirectory %SOLUTION_HOME%\NugetRelease
nuget pack Plugins\Catalog\SamplePlugin.Catalog\SamplePlugin.Catalog.csproj -IncludeReferencedProjects -Prop Configuration=Release -OutputDirectory %SOLUTION_HOME%\NugetRelease

:: build and pack Payment plugin
msbuild Plugins\Payment\SamplePlugin.Payment.sln /m /target:Clean,Build /property:Configuration=Release
nuget pack Plugins\Payment\SamplePlugin.Payment\SamplePlugin.Payment.csproj -IncludeReferencedProjects -Prop Configuration=Release -OutputDirectory %SOLUTION_HOME%\NugetRelease