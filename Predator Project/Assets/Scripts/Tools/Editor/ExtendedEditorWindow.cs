using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropertyPath = string.Empty;

        foreach (SerializedProperty _property in property)
        {
            if (_property.isArray && _property.propertyType == SerializedPropertyType.Generic)
            {

            }
        }
    }
}
