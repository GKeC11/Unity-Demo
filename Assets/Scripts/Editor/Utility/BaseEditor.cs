using UnityEngine;


public class BaseEditor<T> : UnityEditor.Editor where T : MonoBehaviour
{
    protected T _target
    {
        get
        {
            return target as T;
        }
    }
    
}