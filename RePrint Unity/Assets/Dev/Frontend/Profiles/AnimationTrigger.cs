using UnityEngine;

[CreateAssetMenu(fileName = "AnimationTrigger", menuName = "Scriptable Objects/AnimationTrigger")]
public class AnimationTrigger : ScriptableObject
{
    [field: SerializeField] public string TriggerName { get; private set; }

}
