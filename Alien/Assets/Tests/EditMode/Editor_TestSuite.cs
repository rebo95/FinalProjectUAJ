using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEditor;


//Suite de tests para el Editor
public class Editor_TestSuite 
{
    //Script y ventana de prueba
    private TracerWindow window;
    private Tracer script;

    //Incializador
    [SetUp]
    public void Setup()
    {
        window = (TracerWindow)EditorWindow.GetWindow(typeof(TracerWindow));
        script = GameObject.Find("Tracer").GetComponent<Tracer>();
    }

    //Destructor
    [TearDown]
    public void Teardown()
    {
        window.Close();
    }

    //Test nº1 - Comprueba que la ventana se ha creado
    [UnityTest]
    public IEnumerator WindowCreated()
    {
        yield return null;
        Assert.True(window != null);
    }

    //Test nº2 - Comprueba que no hay variables repetidas en la lista de trackeo
    [UnityTest]
    public IEnumerator NotRepeatingVariables()
    {
        yield return null;
        Debug.Log(script.vars.Count);
        for (int i = 0; i < script.vars.Count; i++)
            for (int j = 0; j < script.vars.Count; j++)
                if (i != j)
                    Assert.False(script.vars[i].Equals(script.vars[j]));
    }
}