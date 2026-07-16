using System.Collections.Generic;
using UnityEngine;

public class ModDisplayGroup : MonoBehaviour
{
    [SerializeField] private FloatingDraggableGroup group;

    [SerializeField] private Transform spawnTransform;

    [SerializeField] private FloatingDraggableDisplay prefab;
    [SerializeField] private int numToSpawn = 5;

    void Start()
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            FloatingDraggableDisplay spawned = Instantiate(prefab, spawnTransform);
            spawned.gameObject.SetActive(true);
            group.AddDraggableToGroup(spawned);
        }

        group.StartDragEvent.AddListener(StartDragMod);
        group.StopDragEvent.AddListener(StopDragMod);
    }

    public void StartDragMod(int index, FloatingDraggableDisplay display)
    {
        //If we wanted to drag multiple draggables (like all mods after the index or something)
        // do that here.

        List<FloatingDraggableDisplay> draggables = new List<FloatingDraggableDisplay>()
        {
            display
        };

        DragManager.instance.StartDragging(draggables);
    }

    public void StopDragMod(int index, FloatingDraggableDisplay display)
    {
        DragManager.instance.StopDragging();
    }
}