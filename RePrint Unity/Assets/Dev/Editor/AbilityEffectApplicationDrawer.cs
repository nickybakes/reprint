using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityEffectApplication))]
public class AbilityEffectApplicationDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty modeProperty = AddProperty("applicationMode");
        AbilityEffectApplicationMode mode = (AbilityEffectApplicationMode)modeProperty.enumValueIndex;

        if (mode == AbilityEffectApplicationMode.NonTargetedEnemies)
        {
            AddProperty("nonTargetedEnemyPriority");
            AddProperty("numberOfNonTargetedEnemies");
        }
        // else if (mode == AbilityEffectApplicationMode.Self)
        // {
        //     AddProperty("addValueToTargetPriority");
        // }

        EditorGUI.EndProperty();
    }


}