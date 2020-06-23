using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEditor;


//Suite de tests para el Editor
public class Editor_Test_Suite 
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

    }

    //Test nº1
    [UnityTest]
    public IEnumerator WindowCreated()
    {
        yield return new WaitForSeconds(0.1f);
        Assert.True(window == null);
    }
}