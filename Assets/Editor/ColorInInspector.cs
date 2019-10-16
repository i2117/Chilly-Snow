using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorScript))]
[CanEditMultipleObjects]
public class ColorInInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //if (!GUI.changed)
            //return;

        EditorGUIUtility.LookLikeControls();
        serializedObject.Update();

        ColorScript t = (ColorScript)target as ColorScript;
        t.ColorName = (ColorScript.ColorNames)EditorGUILayout.EnumPopup("ColorName", t.ColorName);
        t.isRandom = EditorGUILayout.Toggle("Is Random", t.isRandom);

        serializedObject.ApplyModifiedProperties();
        EditorGUIUtility.LookLikeInspector();
    }

}
