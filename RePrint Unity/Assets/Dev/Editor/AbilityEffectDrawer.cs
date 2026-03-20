using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityEffect))]
public class AbilityEffectDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        AddQuarterBlankLine();

        AddProperty("type", "Ability Effect Type");

        AddQuarterBlankLine();
        AddProperty("valueInput");
        AddQuarterBlankLine();

        AddProperty("extraArithmetics");
        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }


}