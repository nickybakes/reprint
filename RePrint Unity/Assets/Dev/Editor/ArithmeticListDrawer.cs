using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BetterEditorList<Arithmetic>))]
public class ArithmeticListDrawer : BetterListDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        AddList("list", "Add Arithmetic", .5f, "listFoldout", "foldouts");
        EditorGUI.EndProperty();
    }

    public override string GetNameOfElement(SerializedProperty element, int index)
    {
        SerializedProperty mathTypeProperty = element.FindPropertyRelative("mathType");
        MathType mathType = (MathType)mathTypeProperty.enumValueIndex;

        SerializedProperty inGameValueProperty = element.FindPropertyRelative("gameValueType");
        string[] displayNames = inGameValueProperty.enumDisplayNames;

        string name = "";
        string inGameValueString = displayNames[inGameValueProperty.enumValueIndex];
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
            name += "Value" + mathString + inGameValueString;
        }
        else
        {
            name += inGameValueString + mathString + "Value";
        }

        return name;
    }
}