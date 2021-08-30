
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyColliderBehavior))]
public class MyColliderBehaviorEditor : BaseEditor<MyColliderBehavior>
{
    public List<string> _tags;
    public List<bool> _ignoreList;
    public MySerializedDictionary<string, bool> _ignoreDictionary;
    
    private void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        _ignoreDictionary = _target._ignoreDictionary ?? new MySerializedDictionary<string, bool>();

        _tags = UnityEditorInternal.InternalEditorUtility.tags.ToList();

        _ignoreList = new List<bool>();
        for (int i = 0; i < _tags.Count; i++)
        {
            _ignoreList.Add(false);
            if (_ignoreDictionary.ContainsKey(_tags[i]))
            {
                _ignoreList[i] = _ignoreDictionary[_tags[i]];
            }
        }
        
        EditorGUI.BeginChangeCheck();
        
        EditorGUILayout.LabelField("Collision Ignore");
        
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField("Tag");
        EditorGUILayout.LabelField("Ignore");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        for(int i = 0; i < _tags.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            EditorGUILayout.LabelField(_tags[i]);
            _ignoreList[i] = EditorGUILayout.Toggle(_ignoreList[i]);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            _ignoreDictionary = new MySerializedDictionary<string, bool>();
            for (int i = 0; i < _tags.Count; i++)
            {
                _ignoreDictionary.Add(_tags[i], _ignoreList[i]);
            }
            
            _target._ignoreDictionary = _ignoreDictionary;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
