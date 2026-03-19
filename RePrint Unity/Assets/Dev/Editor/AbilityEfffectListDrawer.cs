using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(AbilityEffectList))]
public class AbilityEffectListDrawer : BetterListDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        AddList("abilityEffects", "Add Ability Effect");
        EditorGUI.EndProperty();
    }

    public override string GetNameOfElement(SerializedProperty element, int index)
    {
        SerializedProperty typeProperty = element.FindPropertyRelative("type");
        string[] displayNames = typeProperty.enumDisplayNames;

        string name = (index + 1).ToString() + ": " + displayNames[typeProperty.enumValueIndex];

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

        return name;
    }
}