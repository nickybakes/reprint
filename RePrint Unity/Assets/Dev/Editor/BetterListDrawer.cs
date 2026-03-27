using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BetterListDrawer : BetterPropertyDrawer
{

    protected int indexToRemoveAt;

    protected List<bool> foldouts;

    protected bool listFoldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }

    public void AddList(string listPropertyName, string addNewString, float normalizedAddNewButtonWidth = 0, string listFoldoutPropString = "", string foldoutsPropString = "")
    {
        SerializedProperty listFoldoutProp = null;
        if (listFoldoutPropString != "")
        {
            listFoldoutProp = property.FindPropertyRelative(listFoldoutPropString);
        }

        bool currentListFoldout = listFoldout;

        if (listFoldoutProp != null)
        {
            currentListFoldout = listFoldoutProp.boolValue;
        }
        else if (property.depth > 1)
        {
            currentListFoldout = true;
        }

        if (property.depth > 1 && listFoldoutProp == null)
        {
            AddBoldLabel(property.displayName);
        }
        else
        {
            if (listFoldoutProp != null)
            {
                listFoldoutProp.boolValue = AddHeaderFoldout(property.displayName, listFoldoutProp.boolValue);
            }
            else
            {
                listFoldout = AddHeaderFoldout(property.displayName, listFoldout);
            }
        }
        EditorGUI.indentLevel++;

        if (currentListFoldout)
        {
            SerializedProperty listProperty = property.FindPropertyRelative(listPropertyName);
            SerializedProperty foldoutsProp = null;
            if (listFoldoutPropString != "")
            {
                foldoutsProp = property.FindPropertyRelative(foldoutsPropString);
            }

            if (foldoutsProp != null)
            {
                while (foldoutsProp.arraySize < listProperty.arraySize)
                {
                    foldoutsProp.InsertArrayElementAtIndex(foldoutsProp.arraySize);
                    foldoutsProp.GetArrayElementAtIndex(foldoutsProp.arraySize - 1).boolValue = true;
                }
                while (foldoutsProp.arraySize > listProperty.arraySize)
                {
                    foldoutsProp.DeleteArrayElementAtIndex(foldoutsProp.arraySize - 1);
                }
            }
            else
            {
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

                if (property.depth > 1 && foldoutsProp == null)
                {
                    AddLabel(name);
                }
                else
                {
                    if (foldoutsProp != null)
                    {
                        foldoutsProp.GetArrayElementAtIndex(i).boolValue = AddFoldout(name, foldoutsProp.GetArrayElementAtIndex(i).boolValue);
                    }
                    else
                    {
                        foldouts[i] = AddFoldout(name, foldouts[i]);
                    }
                }

                if (foldoutsProp == null)
                {
                    if (foldouts[i] || property.depth > 1)
                    {
                        AddProperty("", null, listProperty.GetArrayElementAtIndex(i));
                    }
                }
                else if (foldoutsProp.GetArrayElementAtIndex(i).boolValue)
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

    protected float GetWidth(SerializedProperty property)
    {
        return Math.Min(2f / property.depth, 1f);
    }

}