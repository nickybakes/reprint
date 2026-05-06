using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    [SerializeField] protected CharacterFigure figure;

    public void AnimEventFinishAbility()
    {
        figure.AnimEventFinishAbility();
    }

    public void AnimEventReturnToIdle()
    {
        figure.AnimEventReturnToIdle();
    }

    public void AnimEventUpdateStats()
    {
        figure.AnimEventUpdateStats();
    }
}
