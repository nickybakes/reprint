using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameCondition))]
public class GameConditionDrawer : BetterPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProperty = AddProperty("type");
        GameConditionType type = (GameConditionType)typeProperty.enumValueIndex;

        AddQuarterBlankLine();

        switch (type)
        {
            case GameConditionType.OnGameEvent:
                AddProperty("gameEvent");
                break;
            case GameConditionType.PlayerStat:
                AddProperty("characterStat", "Stat");
                AddProperty("comparison1", "");
                AddProperty("valueInput1");
                break;

        }

        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }


}