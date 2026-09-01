using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SubTagTester : MonoBehaviour
{

    public SubTagDirectory directory;

    public List<string> strings;

    void Start()
    {

        TestSubTags();
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // EditorApplication.isPlaying need to be set to false to end the game
        EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }


    public void TestSubTags()
    {
        Debug.Log("----------");
        Debug.Log("Sub Tag Testing Starting");

        for (int i = 0; i < strings.Count; i++)
        {
            Debug.Log("String: " + strings[i]);
            SubTagResult result = directory.GetAllSubTagResults(strings[i]);
            Debug.Log(result.replaceString);

            Debug.Log("Descriptions: ");

            foreach (string subDescription in result.subDescriptions)
            {
                Debug.Log(subDescription);
            }
        }

    }
}
