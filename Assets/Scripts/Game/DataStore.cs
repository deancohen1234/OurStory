using UnityEngine;

public class DataStore : MonoBehaviour
{
    public static DataStore Instance;

    //level data
    private int CurrentLevel = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public void SetCurrentLevel(int NewLevel)
    {
        CurrentLevel = NewLevel;
    }
}
