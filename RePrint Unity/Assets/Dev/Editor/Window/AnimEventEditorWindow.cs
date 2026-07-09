using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimEventEditorWindow : EditorWindow
{

    private bool changesNotBeenApplied;
    private string path;
    private GameObject selectedObject;

    private ModelImporter modelImporter;
    private Dictionary<ModelImporterClipAnimation, AnimationEvent[]> dataDict;
    private ModelImporterClipAnimation[] animationClips;

    private Vector2 scrollPosition = new Vector2();

    [MenuItem("Tools/AnimEvent Editor")]
    public static void Open()
    {
        AnimEventEditorWindow window = GetWindow<AnimEventEditorWindow>();
        window.titleContent = new GUIContent("AnimEvent Editor");
        window.minSize = new Vector2(700, 450);
        window.Show();
    }

    void OnBecameVisible()
    {
        ReloadWindow();
    }

    void OnEnable()
    {
        ReloadWindow();
    }

    private void OnGUI()
    {
        DrawElements();
    }

    void OnSelectionChange()
    {
        if (changesNotBeenApplied)
        {
            bool answer = EditorUtility.DisplayDialog("Unapplied Changes", "Changes have not been applied yet. Would you like to apply changes?", "Apply Changes", "Revert Changes");
            if (answer)
            {
                ApplyChanges();
            }
        }

        ReloadWindow();
    }

    private void ReloadWindow()
    {
        scrollPosition = new Vector2();
        changesNotBeenApplied = false;
        LoadData();
        Repaint();
    }

    private void LoadData()
    {
        if (Selection.activeGameObject)
        {
            selectedObject = Selection.activeGameObject;
            path = AssetDatabase.GetAssetPath(selectedObject);

            AssetImporter assetImporter = AssetImporter.GetAtPath(path);

            if (!(assetImporter is ModelImporter))
            {
                path = "";
                return;
            }

            modelImporter = assetImporter as ModelImporter;

            dataDict = new Dictionary<ModelImporterClipAnimation, AnimationEvent[]>();

            animationClips = modelImporter.clipAnimations;

            foreach (ModelImporterClipAnimation animation in animationClips)
            {
                dataDict[animation] = new List<AnimationEvent>(animation.events).ToArray();
            }
        }
    }

    private void DrawElements()
    {
        EditorGUIUtility.labelWidth = 50;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (Selection.activeGameObject && path != "")
        {
            EditorGUILayout.LabelField(Selection.activeGameObject.name);
            foreach (ModelImporterClipAnimation animation in animationClips)
            {
                AnimationEvent[] events = dataDict[animation];
                EditorGUILayout.LabelField(animation.name, EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();

                for (int i = 0; i < events.Length; i++)
                {
                    AnimationEvent animEvent = events[i];

                    EditorGUILayout.BeginHorizontal();
                    animEvent.functionName = EditorGUILayout.TextField("Function", animEvent.functionName);
                    if (GUILayout.Button("X", GUILayout.Width(25)))
                    {
                        List<AnimationEvent> newEvents = new List<AnimationEvent>(events);
                        newEvents.RemoveAt(i);
                        dataDict[animation] = newEvents.ToArray();
                    }
                    EditorGUILayout.EndHorizontal();

                    animEvent.time = EditorGUILayout.Slider("Time", animEvent.time, 0f, 1f);
                    EditorGUILayout.BeginHorizontal();
                    animEvent.intParameter = EditorGUILayout.IntField("Int", animEvent.intParameter);
                    animEvent.floatParameter = EditorGUILayout.FloatField("Float", animEvent.floatParameter);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    animEvent.stringParameter = EditorGUILayout.TextField("String", animEvent.stringParameter);
                    animEvent.objectReferenceParameter = (GameObject)EditorGUILayout.ObjectField("Object", animEvent.objectReferenceParameter, typeof(GameObject), false);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Sort", GUILayout.Width(75)))
                {
                    List<AnimationEvent> unsortedEvents = new List<AnimationEvent>(events);
                    List<AnimationEvent> sortedEvents = new List<AnimationEvent>(events.Length);
                    while (unsortedEvents.Count > 0)
                    {
                        int indexOfEarliest = -1;
                        float earliestTime = 2;
                        for (int i = 0; i < unsortedEvents.Count; i++)
                        {
                            if (unsortedEvents[i].time < earliestTime)
                            {
                                indexOfEarliest = i;
                                earliestTime = unsortedEvents[indexOfEarliest].time;
                            }
                        }
                        sortedEvents.Add(unsortedEvents[indexOfEarliest]);
                        unsortedEvents.RemoveAt(indexOfEarliest);
                    }
                    dataDict[animation] = sortedEvents.ToArray();
                }

                GUIStyle buttonStyle = EditorStyles.miniButton;
                buttonStyle.fontSize = 18;
                if (GUILayout.Button("+", buttonStyle, GUILayout.Width(25)))
                {
                    List<AnimationEvent> newEvents = new List<AnimationEvent>(events)
                    {
                        new AnimationEvent()
                    };
                    dataDict[animation] = newEvents.ToArray();
                }
                EditorGUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    changesNotBeenApplied = true;
                }
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!changesNotBeenApplied);
                if (GUILayout.Button("Revert", GUILayout.Width(100)))
                {
                    ReloadWindow();
                }
                if (GUILayout.Button("Apply"))
                {
                    ApplyChanges();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

            }
        }
        else
        {
            EditorGUILayout.LabelField("Nothing selected");
        }
        EditorGUILayout.EndScrollView();
        EditorGUIUtility.labelWidth = 0;
    }

    private void ApplyChanges()
    {
        foreach (ModelImporterClipAnimation animation in animationClips)
        {
            animation.events = dataDict[animation];
        }
        modelImporter.clipAnimations = animationClips;
        EditorUtility.SetDirty(modelImporter);
        modelImporter.SaveAndReimport();

        ReloadWindow();
    }
}
