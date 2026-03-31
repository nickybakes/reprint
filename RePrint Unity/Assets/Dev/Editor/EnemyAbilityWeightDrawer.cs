using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnemyAbilityWeight))]
public class EnemyAbilityWeightDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("abilityData");

        AddProperty("weight");

        EditorGUI.EndProperty();
    }


}