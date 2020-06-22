using UnityEngine;
using UnityEditor.TestTools.TestRunner.Api;
using System.Collections.Generic;
using System.Linq;
using System;

public class ResultCollector : ICallbacks
{
    public ITestResultAdaptor Result { get; private set; }
    public bool Failed { get; private set; }
    public int PassCount { get; private set; }
    public int SkipCount { get; private set; }

    public void RunFinished(ITestResultAdaptor result)
    {
        Result = result;
        PassCount = Result.PassCount;
        SkipCount = Result.SkipCount;
        Failed = Result.FailCount > 0;
    }

    public void RunStarted(ITestAdaptor testsToRun) { }
    public void TestFinished(ITestResultAdaptor result) { }
    public void TestStarted(ITestAdaptor test) { }

    public void ReportTestResults()
    {
        if (Result.FailCount == 0)
        {
            Debug.Log($"All {Result.PassCount} validation tests passed.");
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

            string failedTestNames = string.Join("\n", GetFailedTestNames(Result));
            Debug.Log($"{Result.FailCount} tests failed:\n{failedTestNames}");
        }
    }
}