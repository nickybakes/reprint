using System.Collections.Generic;
using UnityEngine;

public class CharacterFigureGroup : MonoBehaviour
{
    [SerializeField] private FloatingFigureGroup group;

    private Dictionary<Character, CharacterFigure> characterToFigureReferences;
    private Dictionary<CharacterFigure, Character> figureToCharacterReferences;

    private BattleView battleView;
    private BattleController controller;

    public void AddCharacters(List<Character> _characters, BattleView _battlewView, BattleController _controller)
    {
        characterToFigureReferences = new Dictionary<Character, CharacterFigure>();
        figureToCharacterReferences = new Dictionary<CharacterFigure, Character>();
        battleView = _battlewView;
        controller = _controller;
        foreach (Character character in _characters)
        {
            CharacterFigure figure = Instantiate(character.Profile.Figure);
            figure.Setup(character, battleView);
            characterToFigureReferences.Add(character, figure);
            figureToCharacterReferences.Add(figure, character);
            group.AddFigureToGroup(figure);
        }
    }

    public void AddCharacter(Character character, BattleView _battlewView, BattleController _controller)
    {
        AddCharacters(new List<Character>() { character }, _battlewView, _controller);
    }

    public CharacterFigure GetFigure(Character character)
    {
        return characterToFigureReferences.GetValueOrDefault(character);
    }
}
