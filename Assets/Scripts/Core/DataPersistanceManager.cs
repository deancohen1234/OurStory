using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField]
    private string FileName;

    private GameData GameData;
    private FileDataHandler FileDataHandler;

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
        FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName);

        DataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.GameData = new GameData();
    }

    public void LoadGame()
    {
        //get data from saved file
        GameData = FileDataHandler.Load();

        if (GameData == null)
        {
            Debug.Log("No Game Data found... Making new Game!"); 
            NewGame();
        }

        foreach (IPersistentData data in DataPersistenceObjects)
        {
            data.LoadData(GameData);
        }
    }

    public void SaveGame()
    {
        foreach (IPersistentData data in DataPersistenceObjects)
        {
            data.SaveData(ref GameData);
        }

        //write to file
        FileDataHandler.SaveData(GameData);
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
