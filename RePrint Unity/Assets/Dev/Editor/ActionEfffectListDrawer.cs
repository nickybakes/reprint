using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(ActionEffectList))]
public class ActionEffectListDrawer : BetterListDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        AddList("actionEffects", "Add Action Effect");
        EditorGUI.EndProperty();
    }

    public override string GetNameOfElement(SerializedProperty element, int index)
    {
        SerializedProperty typeProperty = element.FindPropertyRelative("type");
        string[] displayNames = typeProperty.enumDisplayNames;

        string name = displayNames[typeProperty.enumValueIndex] + ": ";

        SerializedProperty valueInputProp = element.FindPropertyRelative("valueInput");
        SerializedProperty valueTypeProp = valueInputProp.FindPropertyRelative("type");
        SerializedProperty valueBaseProp = valueInputProp.FindPropertyRelative("baseValue");
        SerializedProperty valueMaxProp = valueInputProp.FindPropertyRelative("maxValue");
        ValueType valueType = (ValueType)valueTypeProp.enumValueIndex;

        string valueString = valueBaseProp.intValue.ToString();

        if (valueType == ValueType.Range)
        {
            valueString += " to " + valueMaxProp.intValue.ToString();
        }

        SerializedProperty chainModProp = element.FindPropertyRelative("chainModifier");
        SerializedProperty chainModTypeProp = chainModProp.FindPropertyRelative("type");
        ChainModifierType chainModType = (ChainModifierType)chainModTypeProp.enumValueIndex;

        string chainModString = "";

        if (chainModType == ChainModifierType.Add)
        {
            chainModString = " + ";
        }
        else if (chainModType == ChainModifierType.Subtract)
        {
            chainModString = " - ";
        }
        else if (chainModType == ChainModifierType.Multiply)
        {
            chainModString = " * ";
        }
        else if (chainModType == ChainModifierType.Divide)
        {
            chainModString = " / ";
        }

        bool chainInvert = chainModProp.FindPropertyRelative("invertEquation").boolValue;

        if (chainModType == ChainModifierType.None)
        {
            name += valueString;
        }
        else
        {
            if (!chainInvert)
            {
                name += valueString + chainModString + "Chain";
            }
            else
            {
                name += "Chain" + chainModString + valueString;
            }
        }

        return name;
    }
}