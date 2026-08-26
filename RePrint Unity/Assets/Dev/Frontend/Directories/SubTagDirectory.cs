using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubTag Directory", menuName = "Scriptable Objects/SubTag Directory")]
public class SubTagDirectory : ScriptableObject
{
    [field: SerializeField] public List<SubTagColor> Colors { get; private set; }

    [field: SerializeField] public List<SubTagSprite> Sprites { get; private set; }

    [field: SerializeField] public List<SubTag> SubTags { get; private set; }


    public SubTagResult GetSubTagResults(string promptString)
    {
        SubTagResult results = new SubTagResult();


        return results;
    }

    private SubTagString FindNextTag(string promptString)
    {

        int openingIndex = promptString.IndexOf('<');

        if (openingIndex != -1)
        {
            string subString = promptString.Substring(openingIndex);
            int closingIndex = subString.IndexOf('>');
            if (closingIndex != -1)
            {

            }
        }

        return null;
    }
}

public class SubTagString
{
    public string tag;
    public int index;
    public int length;
    public string[] parameters;
}

public class SubTagResult
{
    public string promptReplaceString;
    public List<string> subDescriptions = new List<string>();
}

[Serializable]
public class SubTag
{
    [field: SerializeField] public string Tag { get; private set; }
    [field: SerializeField] public string Parameters { get; private set; }
    [field: SerializeField] public string Replacement { get; private set; }
    [field: SerializeField, TextArea] public string SubDescription { get; private set; }
    [field: SerializeField, Range(0, 5)] public int MaxLines { get; private set; } = 5;
}

[Serializable]
public class SubTagColor
{
    [field: SerializeField] public string Tag { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
}

[Serializable]
public class SubTagSprite
{
    [field: SerializeField] public string Tag { get; private set; }
    [field: SerializeField] public int SpriteIndex { get; private set; }
}
