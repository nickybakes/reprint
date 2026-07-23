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
                AddProperty("applicationModes");
                AddProperty("valueInput1");
                break;
            case ModEffectType.StackStatChange:
                AddProperty("statChange");
                AddProperty("valueInput1");
                AddProperty("extraArithmetics1");
                AddProperty("applicationModes");
                break;

        }

        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }


}