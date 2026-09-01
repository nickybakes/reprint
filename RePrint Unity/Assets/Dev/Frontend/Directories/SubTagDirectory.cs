using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SubTag Directory", menuName = "Scriptable Objects/SubTag Directory")]
public class SubTagDirectory : ScriptableObject
{
    [field: SerializeField] public List<SubTagColor> Colors { get; private set; }

    [field: SerializeField] public List<SubTagSprite> Sprites { get; private set; }

    [field: SerializeField] public List<SubTag> SubTags { get; private set; }

    public SubTagResult GetAllSubTagResults(string promptString)
    {
        SubTagResult results = new SubTagResult();

        List<string> descriptions = new List<string>();
        List<int> descriptionIndices = new List<int>();

        results.replaceString = ReplaceSubTagsRecursive(promptString, descriptions, descriptionIndices, new Dictionary<string, string>());
        results.subDescriptions = descriptions;

        return results;
    }


    private string ReplaceSubTagsRecursive(string promptString, List<string> descriptions, List<int> descriptionIndices, Dictionary<string, string> promptStringParameters)
    {
        string[] promptStringKeys = promptStringParameters.Keys.ToArray();
        for (int i = 0; i < promptStringKeys.Length; i++)
        {
            string paramaterCode = "%" + promptStringKeys[i];
            while (promptString.Contains(paramaterCode))
            {
                promptString = promptString.Replace(paramaterCode, promptStringParameters[promptStringKeys[i]]);
            }
        }

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
                    string replacement = SubTags[subtagIndex].Replacement;

                    string[] parameterKeys = SubTags[subtagIndex].Parameters.Split(',');
                    Dictionary<string, string> nextTagParameters = new Dictionary<string, string>();
                    for (int i = 0; i < Math.Min(parameterKeys.Length, nextTag.parameters.Length); i++)
                    {
                        nextTagParameters.Add(parameterKeys[i], nextTag.parameters[i]);
                    }

                    for (int i = 0; i < parameterKeys.Length; i++)
                    {
                        string paramaterCode = "%" + parameterKeys[i];
                        while (replacement.Contains(paramaterCode))
                        {
                            replacement = replacement.Replace(paramaterCode, nextTagParameters[parameterKeys[i]]);
                        }
                    }

                    if (SubTags[subtagIndex].SubDescription != "" && !descriptionIndices.Contains(subtagIndex))
                    {
                        descriptionIndices.Add(subtagIndex);
                        descriptions.Add(ReplaceSubTagsRecursive(SubTags[subtagIndex].SubDescription, descriptions, descriptionIndices, nextTagParameters));
                    }

                    string replacementResult = ReplaceSubTagsRecursive(replacement, descriptions, descriptionIndices, promptStringParameters);

                    promptString = promptString.Remove(nextTag.index, nextTag.length + 1);
                    promptString = promptString.Insert(nextTag.index, replacementResult);

                    currentIndex = nextTag.index + replacementResult.Length;
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

        return promptString;
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
                    int closingParamIndex = parameterString.IndexOf(')');
                    if (closingParamIndex != -1)
                    {
                        parameterString = parameterString.Substring(1, closingParamIndex - 1);
                        subTagString.parameters = parameterString.Split(',');
                        subTagString.tag = keyword.Substring(0, openingParamIndex);
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
    public string replaceString;
    public List<string> subDescriptions = new List<string>();
}

[Serializable]
public class SubTag
{
    [field: SerializeField] public string Tag { get; private set; }
    [field: SerializeField] public string Parameters { get; private set; }
    [field: SerializeField] public string Replacement { get; private set; }
    [field: SerializeField, TextArea] public string SubDescription { get; private set; }
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
