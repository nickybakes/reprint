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
                SerializedProperty eventProperty = AddProperty("gameEvent");
                GameEvent gameEvent = (GameEvent)eventProperty.enumValueIndex;

                // switch (gameEvent)
                // {
                //     case GameEvent.OnCharacterUsesAbility:

                //         break;
                // }
                break;
            case GameConditionType.CharacterStat:
                AddProperty("characterStat", "Stat");
                AddProperty("character");
                AddProperty("comparison1", "");
                AddProperty("valueInput1");
                break;
            case GameConditionType.AbilityType:
                AddProperty("abilityType");
                break;
            case GameConditionType.AbilitySequenceIndex:
                AddProperty("abilityType");
                SerializedProperty indexTypeProp = AddProperty("indexType");
                IndexType indexType = (IndexType)indexTypeProp.enumValueIndex;
                switch (indexType)
                {
                    case IndexType.Specific:
                        AddProperty("valueInput1");
                        break;
                }
                break;

        }

        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }


}