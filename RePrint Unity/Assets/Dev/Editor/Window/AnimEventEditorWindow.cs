using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimEventEditorWindow : EditorWindow
{
    private static Color midBGColor = new Color(0, 0, 0, .09f);
    private static Color darkBGColor = new Color(0, 0, 0, .15f);
    private static List<string> functionNames;

    private static List<string> displayNames;

    private static Dictionary<string, AnimEventParameters> animEventParameters;

    private AnimEventParameters noParameters = new AnimEventParameters();


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
                foreach (ModelImporterClipAnimation animation in animationClips)
                {
                    SortEventsByTime(animation, dataDict[animation]);
                }
                ApplyChanges();
            }
        }

        ReloadWindow();
    }

    private void ReloadWindow()
    {
        functionNames = new List<string>() { "AnimEventFinishAbility", "AnimEventReturnToIdle", "AnimEventUpdateStats", "AnimEventCameraFocusEnemies", "AnimEventCameraFocusDefault", "AnimEventVFX", "AnimEventMoveCharacter", "" };

        displayNames = new List<string>() { "Finish Ability", "Return To Idle", "Update Stats", "Focus Camera on Enemies", "Focus Camera on Default", "Play VFX", "Move Character", "None" };

        animEventParameters = new Dictionary<string, AnimEventParameters>
        {
            { "AnimEventVFX", new AnimEventParameters(1000) },
            { "AnimEventMoveCharacter", new AnimEventParameters(0010) }
        };

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
                EditorGUILayout.Separator();

                for (int i = 0; i < events.Length; i++)
                {
                    AnimationEvent animEvent = events[i];
                    AnimEventParameters parameters = noParameters;
                    if (animEventParameters.ContainsKey(animEvent.functionName))
                        parameters = animEventParameters[animEvent.functionName];

                    Rect rect = GUILayoutUtility.GetLastRect();
                    float height = (parameters.GetNumLines() + 2.8f) * EditorGUIUtility.singleLineHeight;
                    rect.height = height;
                    EditorGUI.DrawRect(rect, i % 2 == 0 ? midBGColor : darkBGColor);

                    EditorGUILayout.BeginHorizontal();

                    int funcNameIndex = functionNames.IndexOf(animEvent.functionName);

                    if (funcNameIndex != -1)
                    {
                        animEvent.functionName = functionNames[EditorGUILayout.Popup("Function", funcNameIndex, displayNames.ToArray())];
                    }
                    else
                    {
                        animEvent.functionName = EditorGUILayout.TextField("Function", animEvent.functionName);
                    }


                    if (GUILayout.Button("X", GUILayout.Width(25)))
                    {
                        List<AnimationEvent> newEvents = new List<AnimationEvent>(events);
                        newEvents.RemoveAt(i);
                        dataDict[animation] = newEvents.ToArray();
                    }


                    EditorGUILayout.EndHorizontal();

                    animEvent.time = EditorGUILayout.Slider("Time", animEvent.time, 0f, 1f);

                    EditorGUILayout.BeginHorizontal();
                    if (parameters.intParam)
                        animEvent.intParameter = EditorGUILayout.IntField("Int", animEvent.intParameter);
                    if (parameters.floatParam)
                        animEvent.floatParameter = EditorGUILayout.FloatField("Float", animEvent.floatParameter);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    if (parameters.stringParam)
                        animEvent.stringParameter = EditorGUILayout.TextField("String", animEvent.stringParameter);
                    if (parameters.objectParam)
                        animEvent.objectReferenceParameter = (GameObject)EditorGUILayout.ObjectField("Object", animEvent.objectReferenceParameter, typeof(GameObject), false);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Event", GUILayout.Width(100)))
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
                if (GUILayout.Button("Sort and Apply"))
                {
                    SortEventsByTime(animation, events);
                    ApplyChanges();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

            }
        }
        else
        {
            EditorGUILayout.LabelField("No Animation/Model selected");
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

    private void SortEventsByTime(ModelImporterClipAnimation animation, AnimationEvent[] events)
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
}

class AnimEventParameters
{
    public bool intParam = false;
    public bool floatParam = false;
    public bool stringParam = false;
    public bool objectParam = false;

    public AnimEventParameters()
    {

    }

    public AnimEventParameters(bool _intParam, bool _floatParam, bool _stringParam, bool _objectParam)
    {
        intParam = _intParam;
        floatParam = _floatParam;
        stringParam = _stringParam;
        objectParam = _objectParam;
    }

    public AnimEventParameters(int code)
    {
        string codeString = code.ToString();

        while (codeString.Length < 4)
        {
            codeString = "0" + codeString;
        }

        intParam = codeString[0] == '1';
        floatParam = codeString[1] == '1';
        stringParam = codeString[2] == '1';
        objectParam = codeString[3] == '1';
    }

    public float GetNumLines()
    {
        int lines = 0;

        if (intParam || floatParam)
            lines++;

        if (stringParam || objectParam)
            lines++;

        return lines;
    }
}