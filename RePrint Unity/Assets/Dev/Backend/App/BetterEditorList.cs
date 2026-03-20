using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BetterEditorList<T>
{
    [SerializeField] private List<T> list;

    public List<T> List { get => list; }
}