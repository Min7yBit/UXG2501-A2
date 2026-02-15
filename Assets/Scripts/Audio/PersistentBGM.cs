using UnityEngine;

public class PersistentBGM : MonoBehaviour
{
    private static PersistentBGM instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
