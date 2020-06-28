using UnityEngine;
using UnityEditor;


//Ventana del editor para elegir parámetros sobre la música generativa
public class DemoWindow : EditorWindow
{
    /*
     * Inicializa la ventana y crea una entrada en el menú
     */
    [MenuItem("Window/Demo Window")]
    static void Init()
    {
        DemoWindow window = (DemoWindow)EditorWindow.GetWindow(typeof(DemoWindow));
        window.Show();
    }

    /*
     * Se llama cada vez que se abre la ventana
     */
    private void Awake()
    {
    }

    /*
     * "Tick" de la ventana (para mostrar cosas al usuario)
     */
    void OnGUI()
    {
    }
}