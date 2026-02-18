using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(ChainModifier))]
public class ChainEffectDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProperty = AddProperty("type", "Chain Modifier");
        ChainModifierType chainModifierType = (ChainModifierType)typeProperty.enumValueIndex;

        if (chainModifierType != ChainModifierType.None)
        {
            AddProperty("invertEquation");

            SerializedProperty clampProperty = AddProperty("clamp", "Clamp Solution");

            if (clampProperty.boolValue)
            {
                AddProperty("minClamp");
                AddProperty("maxClamp");
            }
        }



        EditorGUI.EndProperty();
    }


}