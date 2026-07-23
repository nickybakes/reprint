using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EffectApplication))]
public class AbilityEffectApplicationDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty modeProperty = AddProperty("applicationMode");
        EffectApplicationMode mode = (EffectApplicationMode)modeProperty.enumValueIndex;

        if (mode == EffectApplicationMode.NonTargetedEnemies)
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