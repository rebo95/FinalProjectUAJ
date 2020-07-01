using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/* Adaptación del código del plugin "MusicMaker", creado para el TFG:
 * "Generación de música procedural y adaptativa para videojuegos" (2020)*/

#region Parte de la ventana
public class TracerWindow : EditorWindow
{
    public Tracer tracer;
    /*
     * Inicializa la ventana y crea una entrada en el menú
     */
    [MenuItem("Window/Tracer Window")]
    static void Init()
    {
        TracerWindow window = (TracerWindow)EditorWindow.GetWindow(typeof(TracerWindow));
        window.Show();
    }

    /*
     * Se llama cada vez que se abre la ventana
     */
    private void Awake()
    {
        tracer = GameObject.Find("Tracer").GetComponent<Tracer>();
    }

    /*
     * "Tick" de la ventana (para mostrar cosas al usuario)
     */
    void OnGUI()
    {
        GUILayout.Label("Variables rastreadas:", EditorStyles.boldLabel);
        for(int i = 0; i<tracer.vars.Count; i++)
        {
            GUILayout.Label(tracer.vars[i].nombre + "(" + tracer.vars[i].objeto.name + "'s " +
                tracer.vars[i].componente.GetType().Name.ToLower() + ")     " + tracer.vars[i].GetValue());
        }
    }

    void Update()
    {
        Repaint();
    }
}
#endregion

#region Parte del inspector
[CustomPropertyDrawer(typeof(TracedVar))]
public class DemoInspector : PropertyDrawer
{
    private int height = 58;
    private int compIndex = 0;

    public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return height;
    }

    public List<string> GetProperties(object component, bool showInherited = false)
    {
        //Propiedades del componente (mirar C# reflection)
        System.Type t = component.GetType();
        List<PropertyInfo> props = t.GetProperties().ToList();

        //Las pasamos a string 
        List<string> properties = new List<string>();
        foreach (var prop in props)
        {
            if (showInherited || prop.DeclaringType.Name == t.Name)
                properties.Add(prop.Name);
        }

        //Caso especial
        if (properties.Count == 0)
            return null;
        return properties;
    }


    // Draw the property inside the given rects
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Propiedades 
        SerializedProperty objProp = property.FindPropertyRelative("objeto");
        SerializedProperty compProp = property.FindPropertyRelative("componente");
        SerializedProperty varProp = property.FindPropertyRelative("nombre");

        // Rectángulos donde se pintan
        var objRect = new Rect(position.x, position.y, 100, 16);
        var compRect = new Rect(position.x, position.y + 18, 150, 16);
        var varRect = new Rect(position.x, position.y + 36, 120, 20);

        // Objetos (es un field normal y corriente) NOTA: todos los objetos tienen al menos un componente (el Transform)
        EditorGUI.PropertyField(objRect, objProp, GUIContent.none);
        if (objProp.objectReferenceValue == null)
            return;

        // Componente
        DisplayComponents(property, compRect);

        // Variables
        DisplayVariables(property, varRect);

        //Se coge la cariable
        TracedVar demo = new TracedVar((Component)compProp.objectReferenceValue, varProp.stringValue);
        object variable = demo.GetValue();

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    //Muestra un Popup con todos los componentes que tiene "objProp", devuelve el seleccionado
    private void DisplayComponents(SerializedProperty property, Rect rect)
    {
        //Cogemos las propiedades
        SerializedProperty objProp = property.FindPropertyRelative("objeto");
        SerializedProperty compProp = property.FindPropertyRelative("componente");

        //GameObject escogido y lista con sus componentes
        object go = objProp.objectReferenceValue;
        List<Component> comps = ((GameObject)go).GetComponents<Component>().ToList<Component>();

        //Quitamos los componentes que no tienen atributos públicos
        comps.RemoveAll(x => GetProperties(x) == null);

        //Vemos cuál está seleccionado
        Component seleccionado = comps.Find((x) => x == compProp.objectReferenceValue);

        //Lista de componentes disponibles
        string[] compList = new string[comps.Count];
        for (int i = 0; i < comps.Count; i++)
            compList[i] = comps[i].GetType().ToString();

        //Hacemos el Popup
        int compIndex = Mathf.Max(EditorGUI.Popup(rect, comps.IndexOf(seleccionado), compList), 0);
        compProp.objectReferenceValue = comps[compIndex];
    }

    //Muestra todas las variables que tiene un componente
    private void DisplayVariables(SerializedProperty property, Rect rect)
    {
        //Cogemos las propiedades
        SerializedProperty compProp = property.FindPropertyRelative("componente");
        SerializedProperty varProp = property.FindPropertyRelative("nombre");

        //Componente escogido
        object comp = compProp.objectReferenceValue;

        //Lista de variables disponibles
        List<string> varNames = GetProperties(comp);

        string seleccionada = varNames.Find((x) => x == varProp.stringValue);
        string[] varList = varNames.ToArray<string>();

        //Hacemos el Popup
        int varIndex = Mathf.Max(EditorGUI.Popup(rect, varNames.IndexOf(seleccionada), varList), 0);
        varProp.stringValue = varNames[varIndex];
    }
}
#endregion