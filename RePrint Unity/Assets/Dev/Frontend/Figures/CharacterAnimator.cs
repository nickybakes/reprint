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

    public void AnimEventMoveCharacter(string stringCode)
    {
        figure.AnimEventMoveCharacter(parseStringToArray(stringCode));
    }

    public void AnimEventGlitchEffect(string stringCode)
    {
        figure.AnimEventGlitchEffect(parseStringToArray(stringCode));
    }

    private float[] parseStringToArray(string stringCode)
    {
        string[] stringArray = stringCode.Split(',');
        float[] floatArray = new float[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            floatArray[i] = float.Parse(stringArray[i]);
        }
        return floatArray;
    }
}
