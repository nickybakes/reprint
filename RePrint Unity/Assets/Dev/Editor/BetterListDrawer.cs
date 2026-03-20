using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BetterListDrawer : BetterPropertyDrawer
{

    private int indexToRemoveAt;

    private List<bool> foldouts;

    private bool listFoldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }

    public void AddList(string listPropertyName, string addNewString, float normalizedAddNewButtonWidth = 0)
    {
        if (property.depth > 1)
        {
            AddBoldLabel(property.displayName);
        }
        else
        {
            listFoldout = AddHeaderFoldout(property.displayName, listFoldout);
        }
        EditorGUI.indentLevel++;

        if (listFoldout || (property.depth > 1))
        {
            SerializedProperty listProperty = property.FindPropertyRelative(listPropertyName);
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

                string name = GetNameOfElement(element, i);

                Rect foldoutPosition = Position();

                foldoutPosition.x = foldoutPosition.x + 8 * property.depth;

                EditorGUI.DrawRect(foldoutPosition, new Color(0, 0, 0, .15f));

                foldoutPosition.x = foldoutPosition.width;
                foldoutPosition.width = foldoutPosition.height;

                if (GUI.Button(foldoutPosition, "X"))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                    return;
                }

                if (property.depth > 1)
                {
                    AddLabel(name);
                }
                else
                {
                    foldouts[i] = AddFoldout(name, foldouts[i]);
                }

                if (foldouts[i] || (property.depth > 1))
                {
                    AddProperty("", null, listProperty.GetArrayElementAtIndex(i));
                }

                if (i < listProperty.arraySize - 1)
                    AddQuarterBlankLine();
            }

            AddQuarterBlankLine();

            // EditorGUI.DrawRect(new Rect(10, childrenHeight - 4, position.width * 1.25f, EditorGUIUtility.singleLineHeight * 1.5f + 2), new Color(0, 0, 0, .15f));

            if (normalizedAddNewButtonWidth != 0)
            {
                normalizedWidthOverride = normalizedAddNewButtonWidth;
                normalizedXPositionOverride = .5f - normalizedAddNewButtonWidth * .5f;
            }

            if (Button(addNewString))
            {
                listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
            }

            normalizedWidthOverride = -1;
            normalizedXPositionOverride = -1;

            AddQuarterBlankLine();
        }


        Rect bottomLinePosition = Position();
        bottomLinePosition.x = 10;
        bottomLinePosition.width *= 1.25f;
        bottomLinePosition.height = 2;
        EditorGUI.DrawRect(bottomLinePosition, new Color(0, 0, 0, .25f));

        Rect sideLinePosition = new Rect(position.x + 8 * (property.depth - 1), position.y + EditorGUIUtility.singleLineHeight + 2, 2, childrenHeight - EditorGUIUtility.singleLineHeight);
        EditorGUI.DrawRect(sideLinePosition, new Color(0, 0, 0, .25f));

        AddHalfBlankLine();

        EditorGUI.indentLevel--;
    }


    public virtual string GetNameOfElement(SerializedProperty element, int index)
    {
        return (index + 1).ToString();
    }

}