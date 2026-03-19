using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(ValueInput))]
public class ValueInputDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddLabel(property.displayName);

        SerializedProperty typeProperty = property.FindPropertyRelative("type");
        ValueType valueType = (ValueType)typeProperty.enumValueIndex;

        if (valueType == ValueType.Single)
        {
            StartSameLine(2);
            typeProperty.enumValueIndex = (int)(ValueType)AddDropDownSelection((ValueType)typeProperty.enumValueIndex);
            AddIntProperty("baseValue");
        }
        else if (valueType == ValueType.Range)
        {
            StartSameLine(3);
            typeProperty.enumValueIndex = (int)(ValueType)AddDropDownSelection((ValueType)typeProperty.enumValueIndex);
            AddIntProperty("baseValue");
            AddIntProperty("maxValue");
        }

        EditorGUI.EndProperty();
    }


}