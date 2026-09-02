using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    private GameData GameData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Data Persistance Manager found in scene!");
        }

        Instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {

    }

    public void LoadGame()
    {

    }

    public void SaveGame()
    {

    }

}
