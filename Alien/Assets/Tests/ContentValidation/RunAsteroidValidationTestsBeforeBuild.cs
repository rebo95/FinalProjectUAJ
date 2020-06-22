using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

public class RunAsteroidValidationTestsBeforeBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        var results = new ResultCollector();

        var api = ScriptableObject.CreateInstance<TestRunnerApi>();
        api.RegisterCallbacks(results);

        api.Execute(new ExecutionSettings
        {
            runSynchronously = true,
            filters = new[]
            { 
                new Filter
                {
                    groupNames = new[] { $"AsteroidValidationTests." },
                    testMode = TestMode.EditMode
                }
            }
        });

        if (results.Failed)
        {
            results.ReportTestResults();
            throw new BuildFailedException("One or more of the asteroid validation tests did not pass");
        }

        Debug.Log($"{results.PassCount} asteroid validation tests passed ({results.SkipCount} skipped)");
    }
}