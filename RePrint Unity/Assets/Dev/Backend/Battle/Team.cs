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
    private List<Character> members;

    /// <summary>
    /// Public getter for the list of Characters in this Team.
    /// </summary>
    public List<Character> Members { get => members; }

    public Team()
    {
        members = new List<Character>();
    }

    public Team(Character character)
    {
        members = new List<Character>();
        members.Add(character);
    }

    /// <summary>
    /// Adds a Character to the Team.
    /// </summary>
    /// <param name="character">The Character to add.</param>
    public void AddMember(Character character)
    {
        members.Add(character);
    }

    /// <summary>
    /// Remove a Character at an index.
    /// </summary>
    /// <param name="index">The index to remove at.</param>
    public void RemoveCardAt(int index)
    {
        members.RemoveAt(index);
    }

    /// <summary>
    /// Remove a Character by reference.
    /// </summary>
    /// <param name="character">The Character to remove.</param>
    /// <returns>Whether the Character was found and could be removed.</returns>
    public bool RemoveCard(Character character)
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
}
