#load "NativeScriptDownloader.cake"
#load "DesktopTest.cake"
#load "AndroidTest.cake"
#load "iOSTest.cake"
#load "Utility.cake"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// --------------------------------------------------------------------------------
// FILES & DIRECTORIES
// --------------------------------------------------------------------------------

var solutionFile = File("../SafeApp.sln");

// --------------------------------------------------------------------------------
// PREPARATION
// --------------------------------------------------------------------------------

Task("Restore-NuGet")
  .Does(() => {
    NuGetRestore(solutionFile);
  });

Task("Analyse-Test-Result-Files")
  .Does(() => {
    AnalyseResultFile(Desktop_TESTS_RESULT_PATH);
    AnalyseResultFile(ANDROID_TESTS_RESULT_PATH);
    AnalyseResultFile(IOS_TESTS_RESULT_PATH);
    Information("All Test Results Analysed successfully.");
});

Task("Run-AppVeyor-Build")
  .IsDependentOn("UnZip-Libs")
  .IsDependentOn("Run-Desktop-Tests-AppVeyor")
  .Does(() => {
  });

Task("Default")
  .IsDependentOn("UnZip-Libs")
  .IsDependentOn("Run-Desktop-Tests")
  .IsDependentOn("Run-Android-Tests")
  .IsDependentOn("Run-iOS-Tests")
  .IsDependentOn("Analyse-Test-Result-Files")
  .Does(() => {
  });

RunTarget(target);
