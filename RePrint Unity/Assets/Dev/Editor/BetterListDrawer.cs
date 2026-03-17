using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer extension that keeps track of its position in the Inspector.
/// </summary>
public class BetterListDrawer : BetterPropertyDrawer
{

    int indexToRemoveAt;

    List<bool> foldouts;

    bool listFoldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }

    public void AddList(string listPropertyName, string addNewString)
    {
        listFoldout = AddHeaderFoldout(property.displayName, listFoldout);
        EditorGUI.indentLevel++;

        if (listFoldout)
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

                EditorGUI.DrawRect(foldoutPosition, new Color(0, 0, 0, .15f));

                foldoutPosition.x = foldoutPosition.width;
                foldoutPosition.width = foldoutPosition.height;

                if (GUI.Button(foldoutPosition, "X"))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
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

            if (Button(addNewString))
            {
                listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
            }

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
    }


    public virtual string GetNameOfElement(SerializedProperty element, int index)
    {
        return (index + 1).ToString();
    }

}