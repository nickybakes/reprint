using System;
using UnityEngine;

public class VisualFX : MonoBehaviour
{

    [SerializeField] private float lifetime = 1;

    private float currentTime;

    private bool instantiated;

    void Awake()
    {
        if (!instantiated)
        {
            instantiated = true;
            gameObject.SetActive(false);
        }
    }

    public void Spawn()
    {
        instantiated = true;
        gameObject.SetActive(true);
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
    [SerializeField] private VisualFX visualEffect;
    [SerializeField] private Transform transform;
    [SerializeField] private bool stayParented;

    public VisualFX VisualEffect { get => visualEffect; }
    public Transform Transform { get => transform; }
    public bool StayParented { get => stayParented; }
}
