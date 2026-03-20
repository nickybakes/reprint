using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BetterEditorList<>))]
public class BetterEditorListDrawer : BetterListDrawer
{

    private string objectTypeName;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        objectTypeName = property.displayName;

        if (objectTypeName.EndsWith("s"))
            objectTypeName = objectTypeName.TrimEnd('s');

        float width = Math.Min(2f / property.depth, 1f);

        AddList("list", "Add " + objectTypeName, width);
        EditorGUI.EndProperty();
    }

    public override string GetNameOfElement(SerializedProperty element, int index)
    {
        return objectTypeName + " " + (index + 1).ToString();
    }
}