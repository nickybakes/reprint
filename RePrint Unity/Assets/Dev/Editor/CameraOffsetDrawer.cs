using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(CameraOffset))]
public class CameraOffsetDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddLabel(property.displayName);

        AddProperty("positionOffset");
        AddProperty("fovOffset");

        EditorGUI.EndProperty();
    }


}