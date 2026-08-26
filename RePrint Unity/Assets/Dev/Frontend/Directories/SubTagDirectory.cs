using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        int currentIndex = 0;
        while (currentIndex < promptString.Length)
        {
            SubTagString nextTag = FindNextTag(promptString, currentIndex);
            if (nextTag.index == -1)
            {
                break;
            }

            if (nextTag.isASubTag)
            {
                int subtagIndex = GetSubTagDirectoryIndex(nextTag.tag);
                if (subtagIndex != -1)
                {
                    SubTagResult replacementResult = GetSubTagResults(SubTags[subtagIndex].Replacement);

                    // for (int i = 0; i < SubTags[subtagIndex].Parameters.Length; i++)
                    // {
                    //     string paramaterCode = "%" + SubTags[subtagIndex].Parameters[i];
                    //     while (replacementResult.promptReplaceString.Contains(paramaterCode))
                    //     {
                    //         replacementResult.promptReplaceString = replacementResult.promptReplaceString.Replace(paramaterCode, nextTag.parameters[i]);
                    //     }
                    // }

                    promptString = promptString.Remove(nextTag.index, nextTag.length + 1);
                    promptString = promptString.Insert(nextTag.index, replacementResult.promptReplaceString);

                    currentIndex = nextTag.index + replacementResult.promptReplaceString.Length;
                    continue;
                }
                else
                {
                    int colorIndex = GetColorDirectoryIndex(nextTag.tag);
                    if (colorIndex != -1)
                    {
                        string colorString = "<color=#" + Colors[colorIndex].Color.ToHexString() + ">";
                        promptString = promptString.Remove(nextTag.index, nextTag.length + 1);
                        promptString = promptString.Insert(nextTag.index, colorString);

                        currentIndex = nextTag.index + colorString.Length;
                        continue;
                    }
                    else
                    {
                        int spriteIndex = GetSpriteDirectoryIndex(nextTag.tag);
                        if (spriteIndex != -1)
                        {
                            string spriteString = "<sprite=" + Sprites[spriteIndex].SpriteIndex + ">";
                            promptString = promptString.Remove(nextTag.index, nextTag.length + 1);
                            promptString = promptString.Insert(nextTag.index, spriteString);

                            currentIndex = nextTag.index + spriteString.Length;
                            continue;
                        }
                    }
                }
            }

            currentIndex = nextTag.index + nextTag.length;
        }

        results.promptReplaceString = promptString;
        return results;
    }

    private SubTagString FindNextTag(string promptString, int startingIndex)
    {
        SubTagString subTagString = new SubTagString();

        int openingIndex = promptString.IndexOf('<', startingIndex);

        if (openingIndex != -1)
        {
            subTagString.index = openingIndex;
            subTagString.length = 1;
            string subString = promptString.Substring(openingIndex);
            int closingIndex = subString.IndexOf('>');
            if (closingIndex != -1)
            {
                subTagString.length = closingIndex;
                string keyword = subString.Substring(1, closingIndex - 1);

                subTagString.tag = keyword;

                int openingParamIndex = keyword.IndexOf('(');
                if (openingParamIndex != -1)
                {
                    string parameterString = keyword.Substring(openingParamIndex);
                    int closingParamIndex = keyword.IndexOf(')');
                    if (closingParamIndex != -1)
                    {
                        parameterString = parameterString.Substring(1, closingParamIndex - 1);
                        subTagString.parameters = parameterString.Split(',');
                        subTagString.tag = keyword.Substring(1, openingParamIndex);
                    }
                }

                subTagString.isASubTag = true;
                return subTagString;
            }
        }

        subTagString.isASubTag = false;
        return subTagString;
    }

    private int GetSubTagDirectoryIndex(string tag)
    {
        for (int i = 0; i < SubTags.Count; i++)
        {
            if (SubTags[i].Tag == tag)
                return i;
        }

        return -1;
    }

    private int GetColorDirectoryIndex(string tag)
    {
        for (int i = 0; i < Colors.Count; i++)
        {
            if (Colors[i].Tag == tag)
                return i;
        }

        return -1;
    }

    private int GetSpriteDirectoryIndex(string tag)
    {
        for (int i = 0; i < Sprites.Count; i++)
        {
            if (Sprites[i].Tag == tag)
                return i;
        }

        return -1;
    }

}

public class SubTagString
{
    public bool isASubTag;
    public string tag;
    public int index = -1;
    public int length;
    public string[] parameters = new string[0];
}

public class SubTagResult
{
    public string promptReplaceString;
    public List<SubTagResult> subDescriptions = new List<SubTagResult>();
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
