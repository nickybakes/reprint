using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModEffect))]
public class ModEffectDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("chainGainAmount");

        AddQuarterBlankLine();
        AddProperty("extraArithmetics");

        EditorGUI.EndProperty();
    }


}