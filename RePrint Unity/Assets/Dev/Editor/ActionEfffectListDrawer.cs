using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(ActionEffectList))]
public class ActionEffectListDrawer : BetterPropertyDrawer
{

    int indexToRemoveAt;

    List<bool> foldouts;

    bool listFoldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        listFoldout = AddHeaderFoldout(property.displayName, listFoldout);
        EditorGUI.indentLevel++;

        if (listFoldout)
        {
            SerializedProperty listProperty = property.FindPropertyRelative("actionEffects");
            if (foldouts == null)
            {
                foldouts = new List<bool>();
            }

            while (foldouts.Count < listProperty.arraySize)
            {
                foldouts.Add(true);
            }
            while (foldouts.Count > listProperty.arraySize)
            {
                foldouts.RemoveAt(foldouts.Count - 1);
            }

            indexToRemoveAt = Math.Clamp(indexToRemoveAt, 0, Math.Max(listProperty.arraySize - 1, 0));

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
                SerializedProperty typeProperty = element.FindPropertyRelative("type");
                string[] displayNames = typeProperty.enumDisplayNames;

                string name = displayNames[typeProperty.enumValueIndex] + ": ";

                SerializedProperty valueInputProp = element.FindPropertyRelative("valueInput");
                SerializedProperty valueTypeProp = valueInputProp.FindPropertyRelative("type");
                SerializedProperty valueBaseProp = valueInputProp.FindPropertyRelative("baseValue");
                SerializedProperty valueMaxProp = valueInputProp.FindPropertyRelative("maxValue");
                ValueType valueType = (ValueType)valueTypeProp.enumValueIndex;

                string valueString = valueBaseProp.intValue.ToString();

                if (valueType == ValueType.Range)
                {
                    valueString += " to " + valueMaxProp.intValue.ToString();
                }

                SerializedProperty chainModProp = element.FindPropertyRelative("chainModifier");
                SerializedProperty chainModTypeProp = chainModProp.FindPropertyRelative("type");
                ChainModifierType chainModType = (ChainModifierType)chainModTypeProp.enumValueIndex;

                string chainModString = "";

                if (chainModType == ChainModifierType.Add)
                {
                    chainModString = " + ";
                }
                else if (chainModType == ChainModifierType.Subtract)
                {
                    chainModString = " - ";
                }
                else if (chainModType == ChainModifierType.Multiply)
                {
                    chainModString = " * ";
                }
                else if (chainModType == ChainModifierType.Divide)
                {
                    chainModString = " / ";
                }

                bool chainInvert = chainModProp.FindPropertyRelative("invertEquation").boolValue;

                if (chainModType == ChainModifierType.None)
                {
                    name += valueString;
                }
                else
                {
                    if (!chainInvert)
                    {
                        name += valueString + chainModString + "Chain";
                    }
                    else
                    {
                        name += "Chain" + chainModString + valueString;
                    }
                }

                Rect foldoutPosition = Position();

                EditorGUI.DrawRect(foldoutPosition, new Color(0, 0, 0, .15f));

                foldoutPosition.x = foldoutPosition.width;
                foldoutPosition.width = foldoutPosition.height;

                if (GUI.Button(foldoutPosition, "X"))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                    EditorGUI.EndProperty();
                    return;
                }

                foldouts[i] = AddFoldout(name, foldouts[i]);

                if (foldouts[i])
                {
                    AddProperty("", null, listProperty.GetArrayElementAtIndex(i));
                }

                if (i < listProperty.arraySize - 1)
                    AddQuarterBlankLine();
            }

            AddHalfBlankLine();

            EditorGUI.DrawRect(new Rect(10, childrenHeight - 4, position.width * 1.25f, EditorGUIUtility.singleLineHeight * 1.5f + 2), new Color(0, 0, 0, .15f));

            // StartSameLine(3);
            // sameLineCurrentIndex = 2;

            if (Button("Add Action Effect"))
            {
                listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
            }

            // EditorGUI.BeginDisabledGroup(listProperty.arraySize == 0);

            // indexToRemoveAt = AddIntSlider(indexToRemoveAt, 0, Math.Max(listProperty.arraySize - 1, 0));

            // if (Button("Remove At Index"))
            // {
            //     listProperty.DeleteArrayElementAtIndex(indexToRemoveAt);
            // }

            // EditorGUI.EndDisabledGroup();

            AddQuarterBlankLine();
        }


        Rect bottomLinePosition = Position();
        bottomLinePosition.x = 10;
        bottomLinePosition.width *= 1.25f;
        bottomLinePosition.height = 2;
        EditorGUI.DrawRect(bottomLinePosition, new Color(0, 0, 0, .25f));

        Rect sideLinePosition = new Rect(10, EditorGUIUtility.singleLineHeight + 2, 2, childrenHeight - EditorGUIUtility.singleLineHeight);
        EditorGUI.DrawRect(sideLinePosition, new Color(0, 0, 0, .25f));

        AddHalfBlankLine();

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }


}