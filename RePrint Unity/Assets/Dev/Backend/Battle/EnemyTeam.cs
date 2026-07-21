

using System.Collections.Generic;

public class EnemyTeam : Team
{
    protected List<EnemyCharacter> enemyMembers;

    public List<EnemyCharacter> Enemies { get => enemyMembers; }

    /// <summary>
    /// The list of Characters in this Team in turn order.
    /// </summary>
    protected List<EnemyCharacter> enemiesInTurnOrder;

    /// <summary>
    /// Public getter for the list of Characters in this Team in turn order.
    /// </summary>
    public List<EnemyCharacter> EnemiesInTurnOrder { get => enemiesInTurnOrder; }

    public EnemyTeam() : base()
    {
        enemyMembers = new List<EnemyCharacter>();
    }

    public override void AddMember(Character character)
    {
        enemyMembers.Add((EnemyCharacter)character);
        base.AddMember(character);
    }

    public override void RemoveMemberAt(int index)
    {
        enemyMembers.RemoveAt(index);
        base.RemoveMemberAt(index);
    }

    public override bool RemoveMember(Character character)
    {
        enemyMembers.Remove((EnemyCharacter)character);
        return base.RemoveMember(character);
    }

    public override void CalculateTurnOrder()
    {
        base.CalculateTurnOrder();

        enemiesInTurnOrder = new List<EnemyCharacter>();

        foreach (Character character in membersInTurnOrder)
        {
            enemiesInTurnOrder.Add((EnemyCharacter)character);
        }
    }



}