#tool nuget:?package=JetBrains.ReSharper.CommandLineTools&version=2018.3.4
#addin nuget:?package=Cake.Issues&version=0.6.0
#addin nuget:?package=Cake.Issues.InspectCode&version=0.6.0

var logPath = @"resharper-clt-output.xml";
var buildDirectory = Directory(".");

Task("Analyze-Project")
  .WithCriteria(IsRunningOnWindows())
  .IsDependentOn("Restore-Nuget")
  .Does(() =>
  {
    var settings = new InspectCodeSettings() {
      OutputFile = logPath
    };
    InspectCode(solutionFile.Path, settings);
  });

Task("Analyze-Project-Report")
  .WithCriteria(IsRunningOnWindows())
  .IsDependentOn("Analyze-Project")
  .Does(() =>
  {
    var issues = ReadIssues(
      InspectCodeIssuesFromFilePath(logPath),
      buildDirectory);

    if(issues.Count()>0) {
      Information("InspectCode : {0} issues found.", issues.Count());
      
      foreach (var item in issues)
      {
        var issueMessage = $"Priority: {item.PriorityName}, Details: {item.Message}, Line: {item.Line}, File: {item.AffectedFileRelativePath}";
        if(AppVeyor.IsRunningOnAppVeyor)
          AppVeyor.AddMessage(item.Message,  AppVeyorMessageCategoryType.Error, issueMessage);
        else
          Information(issueMessage);
      }
      
      throw new Exception("Build Failed : InspectCode issues found.");
    }
    else {
      if(AppVeyor.IsRunningOnAppVeyor)
        AppVeyor.AddMessage("No code issues.",  AppVeyorMessageCategoryType.Information);
      else
        Information("InspectCode : No code issues.");
    }
  });
