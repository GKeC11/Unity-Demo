using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        public static T _instance;

        public bool _isDontDestroyOnLoad;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            
            if(_isDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            
            Debug.Log($"{gameObject.name} 初始化");
        }
    }
}
