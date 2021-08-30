using Interface.Elements.Scripts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Editor
{
    [CustomEditor(typeof(ButtonUI))]
    public class ButtonUIEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.Space();
            
            var x = (ButtonUI) target;

            x.duration = EditorGUILayout.FloatField("Transition Time", x.duration);

            EditorGUILayout.Space();
            
            x.hasSlider = EditorGUILayout.Toggle("Slider Effect", x.hasSlider);
            if (x.hasSlider) x.slider = (Slider) EditorGUILayout.ObjectField("Slider", x.slider, typeof(Slider), true);

            EditorGUILayout.Space();

            var propNormal = serializedObject.FindProperty("normalStates");
            EditorGUILayout.PropertyField(propNormal);
            
            EditorGUILayout.Space();

            var propHighlight = serializedObject.FindProperty("highlightStates");
            EditorGUILayout.PropertyField(propHighlight);

            EditorGUILayout.Space();

            var propClick = serializedObject.FindProperty("clickStates");
            EditorGUILayout.PropertyField(propClick);
            
            EditorGUILayout.Space();

            x.hasHoverSound = EditorGUILayout.Toggle("Hover Sound", x.hasHoverSound);
            if (x.hasHoverSound) x.onHoverAudio = (AudioClip) EditorGUILayout.ObjectField(x.onHoverAudio, typeof(AudioClip), true);

            x.hasClickSound = EditorGUILayout.Toggle("Click Sound", x.hasClickSound);
            if (x.hasClickSound) x.onClickAudio = (AudioClip) EditorGUILayout.ObjectField(x.onClickAudio, typeof(AudioClip), true);

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

            serializedObject.ApplyModifiedProperties();
        }
    }
}