using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbilityRules))]
public class AbilityRulesDrawer : BetterPropertyDrawer
{

    bool foldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        foldout = AddFoldout(property.displayName, foldout);

        if (foldout)
        {
            AddProperty("targetAllEnemies");
            AddProperty("numberOfHits");
        }

        EditorGUI.EndProperty();
    }


}