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

                switch (gameEvent)
                {
                    case GameEvent.OnThisCharacterUsesAbility:
                        AddProperty("onlyOnOneInstance");
                        break;
                    case GameEvent.OnOtherCharacterUsesAbility:
                        AddProperty("onlyOnOneInstance");
                        break;
                }
                break;
            case GameConditionType.CharacterStat:
                AddProperty("characterStat", "Stat");
                AddProperty("characters");
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
            case GameConditionType.StoreAbilityInternally:
                SerializedProperty intValue1 = property.FindPropertyRelative("intValue1");
                intValue1.intValue = AddIntSlider(intValue1.intValue, 0, 4, "Slot");
                break;
            case GameConditionType.ComboAmount:
                AddProperty("comboCountType");
                AddProperty("comparison1", "");
                AddProperty("valueInput1");
                break;
            case GameConditionType.TurnHistory:
                SerializedProperty turnIndexTypeProp = AddProperty("turnIndexType");
                TurnIndexType turnIndexType = (TurnIndexType)turnIndexTypeProp.enumValueIndex;
                // switch (turnIndexType)
                // {
                //     case IndexType.Specific:
                //         AddProperty("valueInput1");
                //         break;
                // }
                AddProperty("turnStat");
                AddProperty("characters");
                AddProperty("comparison1", "");
                AddProperty("valueInput1");
                break;
            case GameConditionType.TurnOrWaveIndex:
                AddProperty("turnCountType");
                indexTypeProp = AddProperty("indexType");
                indexType = (IndexType)indexTypeProp.enumValueIndex;
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