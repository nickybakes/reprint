using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VisualEffectAndTransform))]
public class VisualEffectAndTransformDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("visualEffect");

        AddProperty("transform");

        EditorGUI.EndProperty();
    }


}