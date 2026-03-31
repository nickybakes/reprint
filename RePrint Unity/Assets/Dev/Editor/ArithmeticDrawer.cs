using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(Arithmetic))]
public class ArithmeticDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty mathTypeProperty = property.FindPropertyRelative("mathType");
        SerializedProperty inGameValueProperty = property.FindPropertyRelative("gameValueType");

        StartSameLine(2);
        mathTypeProperty.enumValueIndex = (int)(MathType)AddDropDownSelection((MathType)mathTypeProperty.enumValueIndex);
        inGameValueProperty.enumValueIndex = (int)(GameValueType)AddDropDownSelection((GameValueType)inGameValueProperty.enumValueIndex);

        StartSameLine(2);
        AddProperty("invertEquation");
        SerializedProperty clampProperty = AddProperty("clamp", "Clamp Solution");
        if (clampProperty.boolValue)
        {
            StartSameLine(3);
            AddLabel("Min, Max");
            AddIntProperty("minClamp");
            AddIntProperty("maxClamp");
        }

        EditorGUI.EndProperty();
    }


}