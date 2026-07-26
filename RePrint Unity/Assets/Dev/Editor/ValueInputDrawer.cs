using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(ValueInput))]
public class ValueInputDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        StartSameLine(2);
        AddLabel(property.displayName);
        AddProperty("floatMode");

        SerializedProperty typeProperty = property.FindPropertyRelative("type");
        ValueType valueType = (ValueType)typeProperty.enumValueIndex;

        SerializedProperty floatModeProperty = property.FindPropertyRelative("floatMode");
        bool floatMode = floatModeProperty.boolValue;

        if (valueType == ValueType.Single)
        {
            StartSameLine(2);
            typeProperty.enumValueIndex = (int)(ValueType)AddDropDownSelection((ValueType)typeProperty.enumValueIndex);
            if (floatMode)
            {
                AddFloatProperty("baseValue");
            }
            else
            {
                AddTruncatedFloatProperty("baseValue");
            }
        }
        else if (valueType == ValueType.Range)
        {
            StartSameLine(3);
            typeProperty.enumValueIndex = (int)(ValueType)AddDropDownSelection((ValueType)typeProperty.enumValueIndex);
            if (floatMode)
            {
                AddFloatProperty("baseValue");
                AddFloatProperty("maxValue");
            }
            else
            {
                AddTruncatedFloatProperty("baseValue");
                AddTruncatedFloatProperty("maxValue");
            }
        }

        EditorGUI.EndProperty();
    }


}