using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class RunSceneValidationTestsFromContextMenu : ScriptableObject, ICallbacks
{
    static RunSceneValidationTestsFromContextMenu()
    {
        SceneHierarchyHooks.addItemsToSceneHeaderContextMenu += (menu, scene) =>
        {
            menu.AddItem(new GUIContent("Run validation tests"), false, DoRunTests, scene);
        };
    }

    static void DoRunTests(object userData)
    {
        CreateInstance<RunSceneValidationTestsFromContextMenu>().StartTestRun((Scene) userData);
    }

    private static TestRunnerApi _runnerApi;
    private static TestRunnerApi RunnerApi => _runnerApi ? _runnerApi : (_runnerApi = CreateInstance<TestRunnerApi>());

    private void StartTestRun(Scene scene)
    {
        hideFlags = HideFlags.HideAndDontSave;

        RunnerApi.Execute(new ExecutionSettings
        {
            filters = new[]
            {
                new Filter
                {
                    groupNames = new[] { $"SceneValidationTests\\(\"{scene.path}\"\\)\\." },
                    testMode = TestMode.EditMode
                }
            }
        });
    }

    public void OnEnable()
    {
        RunnerApi.RegisterCallbacks(this);
    }

    public void OnDisable()
    {
        RunnerApi.UnregisterCallbacks(this);
    }

    public void RunFinished(ITestResultAdaptor result)
    {
        if (!result.HasChildren)
        {
            EditorUtility.DisplayDialog("Validation Test Result", "No tests were found to run. Are you sure this is a scene that can be validated?", "OK");
        }
        else if (result.FailCount == 0)
        {
            EditorUtility.DisplayDialog("Validation Test Result", $"All {result.PassCount} validation tests passed.", "OK");
        }
        else
        {
            IEnumerable<string> GetFailedTestNames(ITestResultAdaptor test)
            {
                if (test.HasChildren)
                {
                    return test.Children.SelectMany(GetFailedTestNames);
                }

                return test.TestStatus == TestStatus.Failed ? new[] { test.Name } : Array.Empty<string>();
            }

            string failedTestNames = string.Join("\n", GetFailedTestNames(result));

            EditorUtility.DisplayDialog("Validation Test Result", $"{result.FailCount} tests failed:\n{failedTestNames}", "Go to Test Runner");
            EditorApplication.ExecuteMenuItem("Window/General/Test Runner");
        }

        DestroyImmediate(this);
    }

    public void RunStarted(ITestAdaptor testsToRun)
    {
    }

    public void TestFinished(ITestResultAdaptor result)
    {
    }

    public void TestStarted(ITestAdaptor test)
    {
    }
}
