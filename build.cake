#addin Cake.Curl
#addin "package:?Cake.AppVeyor"
#addin "package:?Refit&version=3.0.0"
#addin :package:?Newtonsoft.Json&version=9.0.1"
#load "CakeHelperScripts/NativeScriptDownloader.cake"
#load "CakeHelperScripts/Tests.cake"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


// --------------------------------------------------------------------------------
// FILES & DIRECTORIES
// --------------------------------------------------------------------------------

var solutionFile = File("SafeApp.sln");

var androidProject = File("SafeApp.AppBindings.Android/SafeApp.AppBindings.Android.csproj");
var androidBin = Directory("SafeApp.AppBindings.Android/bin") + Directory(configuration);

var DesktopProject = File("SafeApp.AppBindings.Desktop/SafeApp.AppBindings.Desktop.csproj");
var DesktopBin = Directory("SafeApp.AppBindings.Desktop/bin") + Directory(configuration);

var iOSProject = File("SafeApp.AppBindings.iOS/SafeApp.AppBindings.iOS.csproj");
var iOSBin = Directory("SafeApp.AppBindings.Android/bin/iPhone") + Directory(configuration);


// --------------------------------------------------------------------------------
// PREPARATION
// --------------------------------------------------------------------------------

Task("Clean")
    .Does(() =>
{
    CleanDirectory(DesktopBin);
    CleanDirectory(androidBin);
    CleanDirectory(iOSBin);
});

Task("Restore-NuGet")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

// --------------------------------------------------------------------------------
// Desktop
// --------------------------------------------------------------------------------

Task("Build Desktop")
    .IsDependentOn("Restore-NuGet")
    .Does(() =>
{
 	MSBuild(DesktopProject, settings =>
        settings.SetConfiguration(configuration));
});

// --------------------------------------------------------------------------------
// ANDROID
// --------------------------------------------------------------------------------

Task("Build Android")
    .IsDependentOn("Restore-NuGet")
    .IsDependentOn("Build Desktop")
    .Does(() =>
{
 	MSBuild(androidProject, settings =>
        settings.SetConfiguration(configuration));
        // .WithTarget("SignAndroidPackage"));
});

// --------------------------------------------------------------------------------
// IOS
// --------------------------------------------------------------------------------

Task("Build iOS")
    .WithCriteria(IsRunningOnUnix())
    .IsDependentOn("Build Android")
    .Does(() =>
{
    MSBuild (iOSProject, settings => 
	    settings.SetConfiguration(configuration)
    		.WithProperty("Platform", "iPhone")
    		.WithProperty("OutputPath", $"bin/iPhone/{configuration}/"));
});


Task("Nuget Package")
    .IsDependentOn("Build iOS")
    .Does(() =>
{
    var nuspecfile = File("SafeApp.nuspec"); 
    NuGetPack(nuspecfile, new NuGetPackSettings
        {
            BasePath = ".",
            OutputDirectory = "NugetPackage"
        });
});

Task("Default")
    .IsDependentOn("DownloadTask")
    .IsDependentOn("UnZipTask")
    .IsDependentOn("Run unit tests")
    // .IsDependentOn("Nuget Package")
    .Does(() =>
{
    
});



RunTarget(target);