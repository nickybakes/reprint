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

        SerializedProperty typeProperty = AddProperty("type", "Value Type");
        ValueType valueType = (ValueType)typeProperty.enumValueIndex;

        if (valueType == ValueType.Single)
        {
            AddProperty("baseValue", "Value");
        }
        else if (valueType == ValueType.Range)
        {
            AddProperty("baseValue", "Min Value");
            AddProperty("maxValue", "Max Value");
        }

        EditorGUI.EndProperty();
    }


}