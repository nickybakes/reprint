using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(ActionEffect))]
public class ActionEffectDrawer : BetterPropertyDrawer
{


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        AddQuarterBlankLine();

        SerializedProperty typeProperty = AddProperty("type", "Action Effect Type");
        ActionEffectType actionEffectType = (ActionEffectType)typeProperty.enumValueIndex;

        AddQuarterBlankLine();
        AddProperty("valueInput");
        AddQuarterBlankLine();

        AddProperty("chainModifier");
        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }


}