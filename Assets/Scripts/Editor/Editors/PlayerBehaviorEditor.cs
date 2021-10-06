using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.ProBuilder;
using AnimatorController = UnityEditor.Animations.AnimatorController;

[CustomEditor(typeof(PlayerBehavior))]
public class PlayerBehaviorEditor : BaseEditor<PlayerBehavior>
{
    private AnimatorController _animatorController;
    public List<string> _layerNames = new List<string>();
    public List<string> _stateNames = new List<string>();
    public List<int> _states = new List<int>();
    public MySerializedDictionary<int, int> _stateParentLookup;

    public int _stateIndex;
    public int _layerIndex;

    private Animator _animator;
    
    private SerializedProperty _layerIndexProp;

    private ReorderableList _targetStateList;

    private void OnEnable()
    {
        _layerIndexProp = serializedObject.FindProperty("_layerIndex");

        _targetStateList = null;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        _layerNames.Clear();
        _stateNames.Clear();
        _states.Clear();

        _animator = _target._animator;

        EditorGUI.BeginChangeCheck();

        if (_animator != null)
        {
            _animatorController = _target._animator.runtimeAnimatorController as AnimatorController;
        }

        if (_animatorController != null)
        {
            // 添加 Layer
            for (int i = 0; i < _animatorController.layers.Length; i++)
            {
                _layerNames.Add(_animatorController.layers[i].name);
            }

            _layerIndex = _layerIndexProp.intValue;
            _layerIndex = EditorGUILayout.Popup("Layer", _layerIndex, _layerNames.ToArray());
            _layerIndexProp.intValue = _layerIndex;

            // 添加 SubMachine 和 AnimatorState
            var stateMachine = _animatorController.layers[_layerIndex].stateMachine;
            CollectStateNames(stateMachine);

            if (_targetStateList == null)
            {
                InitTargetStateList();
            }


            _targetStateList.DoLayoutList();
        }

        if (EditorGUI.EndChangeCheck())
        {
            _target._stateParentLookup = _stateParentLookup;

            serializedObject.ApplyModifiedProperties();
            Debug.Log("Editor Change");
        }
    }

    private void CollectStateNames(AnimatorStateMachine stateMachine)
    {
        _stateParentLookup = new MySerializedDictionary<int, int>();
        var _name = stateMachine.name;
        var hash = Animator.StringToHash(_name);

        GetAllStateMachine(stateMachine, $"{_name}.", hash, "");
    }

    private void GetAllStateMachine(AnimatorStateMachine stateMachine, string hashPrefix, int parentHash,
        string displayPrefix)
    {
        if (!stateMachine)
        {
            return;
        }

        GetAllState(stateMachine, hashPrefix, parentHash, displayPrefix);

        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            int hash = Animator.StringToHash($"{hashPrefix}{subStateMachine.stateMachine.name}");
            string _name = subStateMachine.stateMachine.name;

            AddState(hash, parentHash, _name);
            GetAllStateMachine(subStateMachine.stateMachine, $"{hashPrefix}{_name}.", hash, $"{displayPrefix}{_name}.");
        }
    }

    private void GetAllState(AnimatorStateMachine stateMachine, string hashPrefix, int parentHash, string displayPrefix)
    {
        foreach (var state in stateMachine.states)
        {
            AddState(Animator.StringToHash($"{hashPrefix}{state.state.name}")
                , parentHash
                , $"{displayPrefix}{state.state.name}");
        }
    }

    private void AddState(int hash, int parentHash, string displayName)
    {
        if (parentHash != 0)
        {
            _stateParentLookup.Add(hash, parentHash);
        }

        _stateNames.Add(displayName);
        _states.Add(hash);
    }

    private readonly string[] _playerStateArray = new[] {"Normal", "Combat", "Dead"};
    private void InitTargetStateList()
    {
        _targetStateList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("_targetStates"),
            true, true, true, true);

        float vSpace = 2;
        float hSpace = 3;

        _targetStateList.drawHeaderCallback = (rect) => { EditorGUI.LabelField(rect, "State"); };

        _targetStateList.drawElementCallback = (rect, index, active, focused) =>
        {
            rect.y += vSpace;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width /= 2;

            var targetPair = _targetStateList.serializedProperty.GetArrayElementAtIndex(index);
            var targetState = targetPair.FindPropertyRelative("_targetState");
            var cur = GetStateHashIndex(targetState.intValue);
            int select = EditorGUI.Popup(rect, cur, _stateNames.ToArray());
            targetState.intValue = _states[select];
            
            rect.x += rect.width + hSpace; 
            var playerState = targetPair.FindPropertyRelative("_playerState");
            var curPlayerState = playerState.intValue;
            int selectPlayerState = EditorGUI.Popup(rect, curPlayerState, _playerStateArray);
            playerState.intValue = selectPlayerState;
        };
    }

    private int GetStateHashIndex(int hash)
    {
        int index = -1;

        foreach (var state in _states)
        {
            index++;
            if (state == hash)
            {
                return index;
            }
        }

        return 0;
    }
}