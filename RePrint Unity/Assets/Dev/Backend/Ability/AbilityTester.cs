using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AbilityTester : MonoBehaviour
{

    [SerializeField] private List<PlayerAbilityData> abilities;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        TestTruthTables();
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // EditorApplication.isPlaying need to be set to false to end the game
        EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }


    public void TestTruthTables()
    {
        Debug.Log("----------");
        Debug.Log("Truth Table Testing Starting");

        for (int i = 0; i < abilities.Count; i++)
        {
            PlayerAbility ability = new PlayerAbility(abilities[i]);

            Debug.Log("Ability: " + ability.Name);

            List<List<bool>> combinations = ability.GetBehaviorCombinations(ability.GetAbilityBehaviors(0));

            for (int j = 0; j < combinations.Count; j++)
            {
                string row = "";
                for (int k = 0; k < combinations[j].Count; k++)
                {
                    row += combinations[j][k] ? "T " : "F ";
                }

                Debug.Log(j + ": " + row);
            }
        }

    }
}
