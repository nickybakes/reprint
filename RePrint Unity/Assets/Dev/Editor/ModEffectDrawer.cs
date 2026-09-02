using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModEffect))]
public class ModEffectDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProperty = AddProperty("type");
        ModEffectType type = (ModEffectType)typeProperty.enumValueIndex;

        AddQuarterBlankLine();

        switch (type)
        {
            case ModEffectType.DoDamage:
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                AddProperty("applicationModes");
                break;
            case ModEffectType.StackDamageMultiplier:
                AddProperty("starterActions");
                AddProperty("finisherActions");
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                break;
            case ModEffectType.RetriggerAbility:
                SerializedProperty intValue1 = property.FindPropertyRelative("intValue1");
                intValue1.intValue = AddIntSlider(intValue1.intValue, 0, 4, "Slot");
                break;
            case ModEffectType.GainChain:
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                AddProperty("applicationModes");
                break;
            case ModEffectType.GainDodge:
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                AddProperty("applicationModes");
                break;
            case ModEffectType.GainMaxAP:
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                AddProperty("applicationModes");
                break;
            case ModEffectType.StackCritChance:
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                break;
        }

        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }


}