﻿using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;


//Contiene una lista de variables
[ExecuteInEditMode]
public class Tracer : MonoBehaviour
{
    public List<TracedVar> vars;
}

/* Adaptación del código del plugin "MusicMaker", creado para el TFG:
 * "Generación de música procedural y adaptativa para videojuegos" (2020)*/

[System.Serializable]
public class TracedVar 
{
    public Object objeto;
    public Component componente;
    public string nombre;


    #region Constructoras
    public TracedVar()
    {
        this.objeto = null;
        this.componente = null;
        this.nombre = "";
    }

    public TracedVar(Component component, string value)
    {
        this.objeto = component.gameObject;
        this.componente = component;
        this.nombre = value;
    }


    public TracedVar(UnityEngine.Object objeto, Component component, string value)
    {
        this.objeto = objeto;
        this.componente = component;
        this.nombre = value;
    }
    #endregion

    #region Equals y Hash
    public override bool Equals(object obj)
    {
        var demo = obj as TracedVar;
        return demo != null &&
               nombre == demo.nombre &&
               EqualityComparer<object>.Default.Equals(componente, demo.componente) &&
               EqualityComparer<Object>.Default.Equals(objeto, demo.objeto);
    }

    public override int GetHashCode()
    {
        var hashCode = -1495079906;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nombre);
        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(componente);
        hashCode = hashCode * -1521134295 + EqualityComparer<Object>.Default.GetHashCode(objeto);
        return hashCode;
    }
    #endregion

    #region Métodos

    //Tipo de la variable
    public System.Type GetType()
    {
        //Obtenemos la propiedad del componente
        System.Type t = componente.GetType();
        PropertyInfo prop = t.GetProperty(nombre);
        if (prop == null)
            return null;

        //Devolvemos el valor de la propiedad
        return prop.PropertyType;
    }

    //Valor de la variable
    public object GetValue()
    {
        //Obtenemos la propiedad del componente
        object obj = componente; //casting necesario
        System.Type type = obj.GetType();
        PropertyInfo prop = type.GetProperty(nombre);

        //Si no existe, nada
        if (prop == null)
            return null;

        //Devolvemos el valor de la propiedad
        return prop.GetValue(obj);
    }


    //Es correcta
    public bool IsCorrect()
    {
        //1. Comprobamos que hay un gameobject y objeto seleccionados
        object obj = componente;
        if (obj == null || objeto == null)
            return false;

        //2. Comprobamos que el componente tiene esa propiedad
        System.Type t = obj.GetType(); //Tipo
        List<PropertyInfo> props = t.GetProperties().ToList();
        PropertyInfo p = props.Find((x) => x.Name == nombre);
        if (p == null)
            return false;

        return true;
    }
    #endregion
}
