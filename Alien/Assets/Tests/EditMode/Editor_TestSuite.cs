using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


//Suite de tests para el Editor
public class Editor_TestSuite 
{
    //Script y ventana de prueba
    private TracerWindow window;
    private Tracer tracer;

    //Incializador
    [SetUp]
    public void Setup()
    {
        window = (TracerWindow)EditorWindow.GetWindow(typeof(TracerWindow));
        tracer = GameObject.Find("Tracer").GetComponent<Tracer>();

        //Añadimos 4 variables:
        tracer.vars = new List<TracedVar>();
        tracer.vars.Add(new TracedVar(GameObject.Find("Game").GetComponent<Game>(), "currentLifes")); //Correcta
        tracer.vars.Add(new TracedVar(GameObject.Find("Game").GetComponent<Game>(), "currentLifes")); //Correcta pero igual que la 1ª
        tracer.vars.Add(new TracedVar(GameObject.Find("ShipModel").GetComponent<Ship>(), "position")); //Correcta 
        tracer.vars.Add(new TracedVar(GameObject.Find("ShipModel").GetComponent<Ship>(), "isKinematic")); //Incorrecta
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

    //Test nº2 - Comprueba que la ventana ha cogido el tracer correctamente
    [UnityTest]
    public IEnumerator CorrectTracer()
    {
        yield return null;
        Assert.True(window.tracer == tracer);
    }

    //Test nº3 - Comprueba que funciona el método Equals() de TracedVar
    [UnityTest]
    public IEnumerator TracedVarEquals()
    {
        yield return null;
        Assert.True(tracer.vars[0].Equals(tracer.vars[1]));
        Assert.False(tracer.vars[0].Equals(tracer.vars[2]));
    }

    //Test nº4 - Comprueba que funciona el método IsCorrect() de TracedVar
    [UnityTest]
    public IEnumerator TracedVarIsCorrect()
    {
        yield return null;
        Assert.True(tracer.vars[0].IsCorrect());
        Assert.False(tracer.vars[3].IsCorrect());
    }
}