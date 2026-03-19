using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(AbilityEffectModifier))]
public class AbilityEffectModifierDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty mathTypeProperty = property.FindPropertyRelative("mathType");
        SerializedProperty incomingValueProperty = property.FindPropertyRelative("incomingValue");

        StartSameLine(2);
        mathTypeProperty.enumValueIndex = (int)(MathType)AddDropDownSelection((MathType)mathTypeProperty.enumValueIndex);
        incomingValueProperty.enumValueIndex = (int)(ModifierIncomingValue)AddDropDownSelection((ModifierIncomingValue)incomingValueProperty.enumValueIndex);

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