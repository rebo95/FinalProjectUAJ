using System.Linq;
using Tests;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunSceneValidationTestsDuringBuild : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        if (report == null) return;

        var scenePath = scene.path;

        if (!new GameplayScenesProvider().Contains(scenePath)) return;

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
                    groupNames = new[] { $"SceneValidationTests\\(\"{scenePath}\"\\)\\." },
                    testMode = TestMode.EditMode
                }
            }
        });

        if (results.Failed)
        {
            results.ReportTestResults();
            throw new BuildFailedException("One or more of the scene validation tests did not pass");
        }

        Debug.Log($"{results.PassCount} scene validation tests passed for {scenePath} ({results.SkipCount} skipped)");
    }
}
