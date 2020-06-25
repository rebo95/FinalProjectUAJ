using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEditor;


//Suite de tests para el Editor
public class DemoWindow_TestSuite 
{
    private DemoWindow window;

    //Incializador
    [SetUp]
    public void Setup()
    {
        window = (DemoWindow)EditorWindow.GetWindow(typeof(DemoWindow));
    }

    //Destructor
    [TearDown]
    public void Teardown()
    {
        window.Close();
    }

    //Test nº1
    [UnityTest]
    public IEnumerator WindowCreated()
    {
        yield return null;
        Assert.True(window != null);
    }
}