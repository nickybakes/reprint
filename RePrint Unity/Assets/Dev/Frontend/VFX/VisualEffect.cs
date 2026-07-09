using System;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{

    [SerializeField] private float lifetime = 1;

    private float currentTime;

    public void Spawn()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class VisualEffectAndTransform
{
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] private Transform transform;

    public VisualEffect VisualEffect { get => visualEffect; }
    public Transform Transform { get => transform; }
}
