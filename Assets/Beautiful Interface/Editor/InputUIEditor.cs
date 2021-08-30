using Interface.Elements.Scripts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Editor
{
    [CustomEditor(typeof(InputUI))]
    public class InputUIEditor : InputFieldEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUI.BeginChangeCheck();

            var x = (InputUI) target;
            
            x.hidePlaceholderOnSelect = EditorGUILayout.Toggle("Hide Placeholder OnSelect", x.hidePlaceholderOnSelect);
            x.differentTextColorOnHighlight = EditorGUILayout.Toggle("Text Highlight Color", x.differentTextColorOnHighlight);
            
            EditorGUILayout.Space();
            
            x.primaryColor = EditorGUILayout.ColorField("Background Color", x.primaryColor);
            x.secondaryColor = EditorGUILayout.ColorField("Text Color", x.secondaryColor);
            if (x.differentTextColorOnHighlight)
                x.highlightTextColor = EditorGUILayout.ColorField("Text Highlight Color", x.highlightTextColor);
            
            EditorGUILayout.Space();

            x.background = (Image) EditorGUILayout.ObjectField("Background", x.background, typeof(Image), true);
            
            
            // Save changes for multi-selection
            if (EditorGUI.EndChangeCheck())
            {
                /*foreach (var script in targets)
                {
                    var y = (InputUI) script;
                    y.background = x.background;
                    y.primaryColor = x.primaryColor;
                    y.secondaryColor = x.secondaryColor;
                    y.highlightTextColor = x.highlightTextColor;
                    y.hidePlaceholderOnSelect = x.hidePlaceholderOnSelect;
                    y.differentTextColorOnHighlight = x.differentTextColorOnHighlight;
                }*/
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