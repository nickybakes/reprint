using UnityEditor;
using UnityEngine;

public class BetterPropertyDrawer : PropertyDrawer
{


    protected Rect position;

    protected SerializedProperty property;

    protected float childrenHeight;

    protected int sameLineCurrentIndex = 0;

    protected int sameLineAmount = 1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        childrenHeight = 0;
        this.position = position;
        this.property = property;
    }

    protected SerializedProperty AddProperty(string propertyName, string displayName = null, SerializedProperty serializedPropertyOverride = null)
    {
        SerializedProperty thisProperty = serializedPropertyOverride;

        if (thisProperty == null)
        {
            thisProperty = property.FindPropertyRelative(propertyName);
        }

        if (displayName != null)
        {
            GUIContent label = new GUIContent(displayName);
            EditorGUI.PropertyField(Position(), thisProperty, label);
        }
        else
        {
            EditorGUI.PropertyField(Position(), thisProperty);
        }

        childrenHeight += EditorGUI.GetPropertyHeight(thisProperty, true);

        return thisProperty;
    }

    protected bool AddHeaderFoldout(string text, bool foldout)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.Foldout(position, foldout, text, EditorStyles.foldoutHeader);
    }

    protected bool AddFoldout(string text, bool foldout)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.Foldout(position, foldout, text);
    }

    protected void AddLabel(string text)
    {
        Rect position = Position();
        NextLine();
        EditorGUI.LabelField(position, text);
    }

    protected int AddIntSlider(int value, int leftValue, int rightValue)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.IntSlider(position, value, leftValue, rightValue);
    }

    protected bool Button(string text)
    {
        Rect position = Position();
        NextLine();
        return GUI.Button(position, text);
    }

    private void NextLine()
    {
        if (sameLineCurrentIndex == sameLineAmount - 1)
        {
            AddBlankLine();
            sameLineCurrentIndex = 0;
            sameLineAmount = 1;
        }
        else
        {
            sameLineCurrentIndex++;
        }
    }

    protected void AddBlankLine()
    {
        childrenHeight += EditorGUIUtility.singleLineHeight;
    }

    protected void AddHalfBlankLine()
    {
        childrenHeight += EditorGUIUtility.singleLineHeight / 2f;
    }

    protected void AddQuarterBlankLine()
    {
        childrenHeight += EditorGUIUtility.singleLineHeight / 4f;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return childrenHeight;
    }

    protected void StartSameLine(int amount)
    {
        sameLineCurrentIndex = 0;
        sameLineAmount = amount;
    }

    protected Rect Position()
    {
        float width = position.width / sameLineAmount;
        return new Rect(position.x + (width * sameLineCurrentIndex), position.y + childrenHeight, width, EditorGUIUtility.singleLineHeight);
    }
}
