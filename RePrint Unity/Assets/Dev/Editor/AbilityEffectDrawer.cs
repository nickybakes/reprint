using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityEffect))]
public class AbilityEffectDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        AddQuarterBlankLine();

        AddProperty("occurrences");
        AddProperty("occurrencesArithmetics");
        AddProperty("newInstancePerOccurrence");
        // AddProperty("affectTotal");
        // AddProperty("affectCurrentInstance");
        AddProperty("applicationModes");

        SerializedProperty typeProperty = AddProperty("type", "Ability Effect Type");
        AbilityEffectType type = (AbilityEffectType)typeProperty.enumValueIndex;
        AddQuarterBlankLine();

        switch (type)
        {
            case AbilityEffectType.DoDamage:
                AddProperty("valueInput");
                AddProperty("extraArithmetics");
                AddProperty("dontAutoCountHits");
                AddProperty("dontAddCharacterToUniqueHitList");
                break;
            case AbilityEffectType.CountHits:
                AddProperty("hitAmount");
                AddProperty("extraArithmetics");
                AddProperty("dontAddCharacterToUniqueHitList");
                break;
            default:
                AddProperty("valueInput");
                AddProperty("extraArithmetics");
                break;
        }

        AddProperty("extraEffects");

        EditorGUI.EndProperty();
    }


}