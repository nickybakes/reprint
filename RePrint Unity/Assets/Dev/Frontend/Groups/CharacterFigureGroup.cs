using System.Collections.Generic;
using UnityEngine;

public class CharacterFigureGroup : MonoBehaviour
{
    [SerializeField] private FloatingFigureGroup group;

    [SerializeField] private CharacterFigure characterFigurePrefab;

    private Dictionary<Character, CharacterFigure> characterToFigureReferences;
    private Dictionary<CharacterFigure, Character> figureToCharacterReferences;

    private BattleController controller;

    public void AddCharacters(List<Character> _characters, BattleController _controller)
    {
        characterToFigureReferences = new Dictionary<Character, CharacterFigure>();
        figureToCharacterReferences = new Dictionary<CharacterFigure, Character>();
        controller = _controller;
        foreach (Character character in _characters)
        {
            CharacterFigure figure = Instantiate(characterFigurePrefab);
            characterToFigureReferences.Add(character, figure);
            figureToCharacterReferences.Add(figure, character);
            group.AddFigureToGroup(figure);
        }
    }

    public void AddCharacter(Character character, BattleController _controller)
    {
        AddCharacters(new List<Character>() { character }, _controller);
    }

    public CharacterFigure GetFigure(Character character)
    {
        return characterToFigureReferences.GetValueOrDefault(character);
    }
}
