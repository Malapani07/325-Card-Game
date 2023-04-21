using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    // Start is called before the first frame update
    public AudioSource[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
