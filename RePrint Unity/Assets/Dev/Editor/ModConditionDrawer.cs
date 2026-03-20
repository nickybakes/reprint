using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModCondition))]
public class ModConditionDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("minChainThreshold");

        EditorGUI.EndProperty();
    }


}