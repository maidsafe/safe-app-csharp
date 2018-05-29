#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.4.0"
#tool "nuget:?package=ReportGenerator"
// --------------------------------------------------------------------------------
// TESTS
// --------------------------------------------------------------------------------

Task("Build tests")
    .IsDependentOn("Build Android")
    .Does(() =>
{
    var parsedSolution = ParseSolution(solutionFile);
          
	foreach(var project in parsedSolution.Projects)
	{
	    if(project.Name.EndsWith("Tests.Framework"))
		{
            Information("Start Building Test: " + project.Name);

            MSBuild(project.Path, 
            settings => settings.SetConfiguration(configuration)
            .SetPlatformTarget(PlatformTarget.x64));

		}
	} 
});

Task("Run unit tests")
    .IsDependentOn("Build tests")
    .Does(() =>
{
    try
	{
        var resultsFile ="./TestResult.xml";
        NUnit3("../**/bin/" + configuration + "/*.Tests.Framework.dll", new NUnit3Settings {
            Results = new[] { new NUnit3Result { FileName = resultsFile } },     
            X86 = false
        });

        if(AppVeyor.IsRunningOnAppVeyor)
        {
            AppVeyor.UploadTestResults(resultsFile, AppVeyorTestResultsType.NUnit3);
        }
    }
	catch(Exception exp) {

        Information(exp.Message);
    }
});