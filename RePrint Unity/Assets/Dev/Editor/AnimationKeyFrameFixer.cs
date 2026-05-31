using UnityEditor;
using UnityEngine;

public class AnimationKeyFrameFixer : AssetPostprocessor
{

    void OnPostprocessAnimation(GameObject root, AnimationClip clip)
    {
        // Get all curves on the clip
        Debug.Log("Fixing animation on " + clip.name);

        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

        foreach (EditorCurveBinding binding in bindings)
        {
            if (binding.path.Contains("Unity_PLC"))
            {
                continue;
            }

            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);

            // Set all keys to constant/stepped tangents
            for (int i = 0; i < curve.keys.Length; i++)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
            }

            AnimationUtility.SetEditorCurve(clip, binding, curve);
        }
    }
}
