using System.Collections.Generic;
using UnityEngine;

public class ModDisplayGroup : MonoBehaviour
{
    [SerializeField] private FloatingDraggableGroup group;

    [SerializeField] private Transform spawnTransform;

    [SerializeField] private FloatingDraggableDisplay prefab;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            FloatingDraggableDisplay spawned = Instantiate(prefab, spawnTransform);
        }
    }
}