using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    private GameData GameData;

    private List<IPersistentData> DataPersistenceObjects;

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
        DataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.GameData = new GameData();
    }

    public void LoadGame()
    {
        if (GameData == null)
        {
            Debug.Log("No Game Data found... Making new Game!"); 
            NewGame();
        }

        foreach (IPersistentData data in DataPersistenceObjects)
        {
            data.LoadData(GameData);
        }

        Debug.Log("Loaded Data: " + GameData.TotalCoins);
    }

    public void SaveGame()
    {
        foreach (IPersistentData data in DataPersistenceObjects)
        {
            Debug.Log(data);
            data.SaveData(ref GameData);
        }

        Debug.Log("Saved Data: " + GameData.TotalCoins);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private List<IPersistentData> FindAllDataPersistenceObjects()
    {
        IEnumerable<IPersistentData> persistentDataObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPersistentData>();

        return new List<IPersistentData>(persistentDataObjects);
    }
}
