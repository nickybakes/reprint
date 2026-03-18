using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(ActionRules))]
public class ActionRulesDrawer : BetterPropertyDrawer
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