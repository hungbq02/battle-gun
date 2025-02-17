using UnityEngine;

public class Singleton<T> : BaseMonoBehaviour where T : BaseMonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Find singleton instance in scene
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"Singleton of {typeof(T).Name} has not been created!");
                }
            }
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        this.LoadInstance();
    }

    protected virtual void LoadInstance()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (transform.parent == null) DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Another instance of {typeof(T).Name} already exists! Destroying duplicate.");
            Destroy(gameObject); //Destroy duplicate
        }
    }
}

/*public class ObjectB : Singleton<ObjectB>
{

}*/