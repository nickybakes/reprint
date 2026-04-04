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

        SerializedProperty typeProperty = AddProperty("type", "Ability Effect Type");
        AbilityEffectType type = (AbilityEffectType)typeProperty.enumValueIndex;

        AddProperty("applicationModes");

        switch (type)
        {
            default:
                AddQuarterBlankLine();
                AddProperty("valueInput");
                AddQuarterBlankLine();

                AddProperty("extraArithmetics");
                AddQuarterBlankLine();
                break;


        }



        EditorGUI.EndProperty();
    }


}