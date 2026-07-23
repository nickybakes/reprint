using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A team is a collection of characters in a battle. A battle is between two or more teams.
/// </summary>
public class Team
{
    /// <summary>
    /// The list of Characters in this Team.
    /// </summary>
    protected List<Character> members;

    /// <summary>
    /// Public getter for the list of Characters in this Team.
    /// </summary>
    public List<Character> Members { get => members; }

    /// <summary>
    /// The list of Characters in this Team in turn order.
    /// </summary>
    protected List<Character> membersInTurnOrder;

    /// <summary>
    /// Public getter for the list of Characters in this Team in turn order.
    /// </summary>
    public List<Character> MembersInTurnOrder { get => membersInTurnOrder; }

    public Team()
    {
        members = new List<Character>();
    }

    public Team(Character character)
    {
        members = new List<Character>
        {
            character
        };
    }

    /// <summary>
    /// Adds a Character to the Team.
    /// </summary>
    /// <param name="character">The Character to add.</param>
    public virtual void AddMember(Character character)
    {
        members.Add(character);
    }

    /// <summary>
    /// Remove a Character at an index.
    /// </summary>
    /// <param name="index">The index to remove at.</param>
    public virtual void RemoveMemberAt(int index)
    {
        members.RemoveAt(index);
    }

    /// <summary>
    /// Remove a Character by reference.
    /// </summary>
    /// <param name="character">The Character to remove.</param>
    /// <returns>Whether the Character was found and could be removed.</returns>
    public virtual bool RemoveMember(Character character)
    {
        return members.Remove(character);
    }

    public int GetNumberOfAliveMembers()
    {
        int num = 0;
        foreach (Character member in members)
        {
            if (member.IsAlive)
                num++;
        }
        return members.Count;
    }

    public virtual void CalculateTurnOrder()
    {
        membersInTurnOrder = new List<Character>();
        List<Character> remainingMembers = new List<Character>(members);

        while (remainingMembers.Count > 0)
        {
            Character highestPriorityMember = remainingMembers[0];
            int highestPriority = -1;
            foreach (Character member in remainingMembers)
            {
                if (member.Stats.TurnPriority > highestPriority)
                {
                    highestPriority = member.Stats.TurnPriority;
                    highestPriorityMember = member;
                }
            }

            membersInTurnOrder.Add(highestPriorityMember);
            remainingMembers.Remove(highestPriorityMember);
        }
    }

    public void ResetTurnPriorities()
    {
        foreach (Character member in members)
        {
            member.Stats.TurnPriority = 0;
        }
    }

    public void ResetForTurn()
    {
        foreach (Character member in members)
        {
            member.ResetForTurn();
        }
    }

    public void SetTurnStats()
    {
        foreach (Character member in members)
        {
            member.SetTurnStats();
        }
    }

    public void RestoreTurnStats()
    {
        foreach (Character member in members)
        {
            member.RestoreTurnStats();
        }
    }

    public void CopyStatsFromTurnStatsStorage()
    {
        foreach (Character member in members)
        {
            member.Stats.CopyFrom(member.TurnStatsStorage);
        }
    }
}
