public class SingletonMono<T> : MonoParent where T : MonoParent
{

    private static T instance = null;

    public static T Instance
    {
        get => instance;
    }

    void CreateObject()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void Awake() => CreateObject();
}
 