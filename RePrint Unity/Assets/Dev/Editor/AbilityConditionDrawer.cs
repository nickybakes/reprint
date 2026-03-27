using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityCondition))]
public class AbilityConditionDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        // AddProperty("minChainThreshold");
        AddLabel("TODO");

        EditorGUI.EndProperty();
    }


}