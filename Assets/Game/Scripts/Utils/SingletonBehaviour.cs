using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    private static T _Instance;
    
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<T>();

                if (_Instance == null)
                {
                    _Instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
                }

                _Instance.Initialize();
            }

            return _Instance;
        }
    }
    public virtual bool IsDontDestroyOnLoad { get => true; }

    protected virtual void Awake()
    {
        if (IsDontDestroyOnLoad)
            DontDestroyOnLoad(this);
    }

    public virtual void Initialize() { }
}
