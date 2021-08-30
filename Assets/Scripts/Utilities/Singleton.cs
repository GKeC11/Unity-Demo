using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }
            
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.Log($"错误：存在多个{typeof(T).ToString()}实例");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString();
                        Debug.Log($"{typeof(T).ToString()}已创建");
                    }
                    else
                    {
                        
                    }
                }

                return _instance;
            }
        }
    }
    
    private static bool applicationIsQuitting = false;

    private static bool IsDontDestroyOnLoad()
    {
        if (_instance == null)
        {
            return false;
        }

        if ((_instance.gameObject.hideFlags & HideFlags.DontSave) == HideFlags.DontSave)
        {
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        if (IsDontDestroyOnLoad())
        {
            applicationIsQuitting = true;
        }
    }
}
