using UnityEngine;

public class CharacterFigure : FloatingFigure
{

    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform positionBone;

    private bool isMovingInAnimation;


    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    void Awake()
    {
        SetupTravelingTransformData();
    }

    void Update()
    {
        if (!isMovingInAnimation)
            UpdateTravel();
    }
}
