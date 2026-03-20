using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BetterEditorList<Arithmetic>))]
public class ArithmeticListDrawer : BetterListDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        AddList("list", "Add Arithmetic", .5f);
        EditorGUI.EndProperty();
    }

    public override string GetNameOfElement(SerializedProperty element, int index)
    {
        SerializedProperty mathTypeProperty = element.FindPropertyRelative("mathType");
        MathType mathType = (MathType)mathTypeProperty.enumValueIndex;

        SerializedProperty incomingValueProperty = element.FindPropertyRelative("inGameValueType");
        string[] displayNames = incomingValueProperty.enumDisplayNames;

        string name = "";
        string incomingValueString = displayNames[incomingValueProperty.enumValueIndex];
        string mathString = "";

        if (mathType == MathType.Add)
        {
            mathString = " + ";
        }
        else if (mathType == MathType.Subtract)
        {
            mathString = " - ";
        }
        else if (mathType == MathType.Multiply)
        {
            mathString = " * ";
        }
        else if (mathType == MathType.Divide)
        {
            mathString = " / ";
        }

        bool invertEquation = element.FindPropertyRelative("invertEquation").boolValue;

        if (!invertEquation)
        {
            name += "Value" + mathString + incomingValueString;
        }
        else
        {
            name += incomingValueString + mathString + "Value";
        }

        return name;
    }
}