using Interface.Elements.Scripts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Editor
{
    [CustomEditor(typeof(ToggleUI))]
    public class ToggleUIEditor : ToggleEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            
            var x = (ToggleUI) target;
            
            x.onColor = EditorGUILayout.ColorField("On Color", x.onColor);
            x.offColor = EditorGUILayout.ColorField("Off Color", x.offColor);
            
            EditorGUILayout.Space();

            x.background = (Image) EditorGUILayout.ObjectField("Background", x.background, typeof(Image), true);
            x.outline = (Image) EditorGUILayout.ObjectField("Outline", x.outline, typeof(Image), true);
            x.highlighter = (Image) EditorGUILayout.ObjectField("Highlighter", x.highlighter, typeof(Image), true);

            EditorGUILayout.Space();
            
            x.onText = (Text) EditorGUILayout.ObjectField("On Text", x.onText, typeof(Text), true);
            x.onImage = (Image) EditorGUILayout.ObjectField("On Image", x.onImage, typeof(Image), true);
            
            x.offText = (Text) EditorGUILayout.ObjectField("Off Text", x.offText, typeof(Text), true);
            x.offImage = (Image) EditorGUILayout.ObjectField("Off Image", x.offImage, typeof(Image), true);
            
            
            EditorGUILayout.Space();

            x.leftIsOn = EditorGUILayout.Toggle("Left Is On", x.leftIsOn);
            
            
            // Save changes for multi-selection
            if (EditorGUI.EndChangeCheck())
            {
                /*foreach (var script in targets)
                {
                    var y = (ToggleUI) script;
                    y.background = x.background;
                    y.highlighter = x.highlighter;
                    y.outline = x.outline;
                    y.offColor = x.offColor;
                    y.offImage = x.offImage;
                    y.offText = x.offText;
                    y.onColor = x.onColor;
                    y.onImage = x.onImage;
                    y.onText = x.onText;
                    y.leftIsOn = x.leftIsOn;
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