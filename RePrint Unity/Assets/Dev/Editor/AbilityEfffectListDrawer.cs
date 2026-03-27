using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BetterEditorList<AbilityEffect>))]
public class AbilityEffectListDrawer : BetterListDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        AddList("list", "Add Ability Effect", GetWidth(property), "listFoldout", "foldouts");
        EditorGUI.EndProperty();
    }

    public override string GetNameOfElement(SerializedProperty element, int index)
    {
        SerializedProperty typeProperty = element.FindPropertyRelative("type");
        AbilityEffectType type = (AbilityEffectType)typeProperty.enumValueIndex;
        string[] displayNames = typeProperty.enumDisplayNames;

        string name = (index + 1).ToString() + ": " + displayNames[(int)type];

        switch (type)
        {
            case AbilityEffectType.RemoveAllChain:
                break;

            default:
                SerializedProperty valueInputProp = element.FindPropertyRelative("valueInput");
                SerializedProperty valueTypeProp = valueInputProp.FindPropertyRelative("type");
                SerializedProperty valueBaseProp = valueInputProp.FindPropertyRelative("baseValue");
                SerializedProperty valueMaxProp = valueInputProp.FindPropertyRelative("maxValue");
                ValueType valueType = (ValueType)valueTypeProp.enumValueIndex;

                string valueString = " (" + valueBaseProp.intValue.ToString();

                if (valueType == ValueType.Range)
                {
                    valueString += " to " + valueMaxProp.intValue.ToString();
                }

                name += valueString + ")";
                break;

        }



        return name;
    }
}