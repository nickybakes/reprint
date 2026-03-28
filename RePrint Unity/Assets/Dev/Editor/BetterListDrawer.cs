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
        SerializedProperty listProperty = property.FindPropertyRelative(listPropertyName);
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

        string headerName = property.displayName;

        Rect headerArraySizeLabelPosition = Position();
        headerArraySizeLabelPosition.x = headerArraySizeLabelPosition.width - 40;
        EditorGUI.LabelField(headerArraySizeLabelPosition, "Size: " + listProperty.arraySize, EditorStyles.boldLabel);

        if (property.depth > 1 && listFoldoutProp == null)
        {
            AddBoldLabel(headerName);
        }
        else
        {
            if (listFoldoutProp != null)
            {
                listFoldoutProp.boolValue = AddHeaderFoldout(headerName, listFoldoutProp.boolValue);
            }
            else
            {
                listFoldout = AddHeaderFoldout(headerName, listFoldout);
            }
        }

        EditorGUI.indentLevel++;

        if (currentListFoldout)
        {
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

                foldoutPosition.x = position.x + 8 * (property.depth - 1) + 2;

                EditorGUI.DrawRect(foldoutPosition, new Color(0, 0, 0, .15f));

                foldoutPosition.x = foldoutPosition.width;
                foldoutPosition.width = foldoutPosition.height;

                if (GUI.Button(foldoutPosition, "X"))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                    if (foldoutsProp != null)
                    {
                        foldoutsProp.DeleteArrayElementAtIndex(i);
                    }
                    else
                    {
                        foldouts.RemoveAt(i);
                    }
                    childrenHeight = 10000;
                    return;
                }

                foldoutPosition.x -= 50;

                EditorGUI.BeginDisabledGroup(i == listProperty.arraySize - 1);

                if (GUI.Button(foldoutPosition, "↓"))
                {
                    listProperty.MoveArrayElement(i, i + 1);
                    if (foldoutsProp != null)
                    {
                        foldoutsProp.MoveArrayElement(i, i + 1);
                    }
                    else
                    {
                        bool tempFoldout = foldouts[i + 1];
                        foldouts[i + 1] = foldouts[i];
                        foldouts[i] = tempFoldout;
                    }
                    childrenHeight = 10000;
                    return;
                }

                EditorGUI.EndDisabledGroup();

                foldoutPosition.x += 17;

                EditorGUI.BeginDisabledGroup(i == 0);

                if (GUI.Button(foldoutPosition, "↑"))
                {
                    listProperty.MoveArrayElement(i, i - 1);
                    if (foldoutsProp != null)
                    {
                        foldoutsProp.MoveArrayElement(i, i - 1);
                    }
                    else
                    {
                        bool tempFoldout = foldouts[i - 1];
                        foldouts[i - 1] = foldouts[i];
                        foldouts[i] = tempFoldout;
                    }
                    childrenHeight = 10000;
                    return;
                }

                EditorGUI.EndDisabledGroup();

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

                // if (i < listProperty.arraySize - 1)
                //     AddQuarterBlankLine();
            }

            AddQuarterBlankLine();

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
        bottomLinePosition.x = position.x + 8 * (property.depth - 1);
        bottomLinePosition.width *= 1.25f;
        bottomLinePosition.height = 2;
        EditorGUI.DrawRect(bottomLinePosition, new Color(0, 0, 0, .25f));

        Rect sideLinePosition = new Rect(position.x + 8 * (property.depth - 1), position.y + EditorGUIUtility.singleLineHeight, 2, childrenHeight - EditorGUIUtility.singleLineHeight);
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