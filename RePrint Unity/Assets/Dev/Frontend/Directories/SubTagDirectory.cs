using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubTag Directory", menuName = "Scriptable Objects/SubTag Directory")]
public class SubTagDirectory : ScriptableObject
{
    [field: SerializeField] public List<SubTagColor> Colors { get; private set; }

    [field: SerializeField] public List<SubTagSprite> Sprites { get; private set; }

    [field: SerializeField] public List<SubTag> SubTags { get; private set; }
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
