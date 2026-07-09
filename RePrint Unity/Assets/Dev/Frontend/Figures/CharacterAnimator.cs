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

    public void AnimEventCameraFocusEnemies()
    {
        figure.AnimEventCameraFocusEnemies();
    }

    public void AnimEventCameraFocusDefault()
    {
        figure.AnimEventCameraFocusDefault();
    }

    public void AnimEventVFX(int index)
    {
        figure.AnimEventVFX(index);
    }
}
