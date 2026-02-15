using UnityEngine;

public class BattleState
{

    protected float timeInState;

    public virtual void StartState()
    {

    }

    public virtual void EndState()
    {

    }

    public virtual void Update(float deltaTime)
    {
        timeInState += deltaTime;
    }

    public static void print(object message)
    {
        Debug.Log(message);
    }
}
