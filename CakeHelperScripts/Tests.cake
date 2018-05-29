// #tool nuget:?package=NUnit.ConsoleRunner
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
    NUnit3("../**/bin/" + configuration + "/*.Tests.Framework.dll", new NUnit3Settings {
        NoResults = true
    });
    	}
	catch(Exception exp) {

        Information(exp.Message);
    }
});