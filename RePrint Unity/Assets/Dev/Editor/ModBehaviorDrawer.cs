using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModBehavior))]
public class ModBehaviorDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("breakOutIfConditionsAreTrue");

        AddProperty("conditions");

        AddProperty("effects");

        EditorGUI.EndProperty();
    }


}