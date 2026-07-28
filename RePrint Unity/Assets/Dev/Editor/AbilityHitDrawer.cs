using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityHit))]
public class AbilityHitDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("amount");
        AddProperty("extraArithmetics");

        AddProperty("effects");

        EditorGUI.EndProperty();
    }


}