using Interface.Elements.Scripts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TextEditor = UnityEditor.UI.TextEditor;

namespace Interface.Editor
{
    [CustomEditor(typeof(TextUI))]
    public class TextUIEditor : TextEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.Space();
            
            var x = (TextUI) target;

            x.Capitalize = EditorGUILayout.Toggle("Capitalize", x.Capitalize);
            
            x.animateTypingOnStart = EditorGUILayout.Toggle("Typing Effect", x.animateTypingOnStart);
            if (x.animateTypingOnStart)
            {
                x.addTrailingUnderscore = EditorGUILayout.Toggle("Add Trailing Underscore", x.addTrailingUnderscore);
                x.addRandomCharacters = EditorGUILayout.Toggle("Random Characters", x.addRandomCharacters);
                x.typingSpeed = EditorGUILayout.FloatField("Typing Speed", x.typingSpeed);
                x.typingSound = (AudioClip) EditorGUILayout.ObjectField("Typing Sound", x.typingSound, typeof(AudioClip), true);
            }
            else
            {
                x.animateTypingOnStart = false;
            }

            // Save changes for multi-selection
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
            
            // Mark scene as dirty
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(x.gameObject.scene);
            }
        }
    }
}