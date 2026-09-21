using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField]
    private string DefaultFileName;

    private GameData GameData;
    private FileDataHandler FileDataHandler;

    private List<IPersistentData> DataPersistenceObjects;

    private bool bBlockLoading = false;

    private void Awake()
    {
        if (Instance != null)
        {
            //this is the extra data manager
            bBlockLoading = true;
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        //if there already is a Data Persistence Manager running, then don't try and load in objects
        if (bBlockLoading)
        {
            return;
        }

        //use save data if it exists
        string fileName = DefaultFileName;
        if (PlayerPrefs.HasKey("SaveFileName"))
        {
            fileName = PlayerPrefs.GetString("SaveFileName");
        }

        FileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        DataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    //clear old DataPersistenceObjects and then load game
    //called from DataStore
    public void LoadNewLevel()
    {
        DataPersistenceObjects.Clear();
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
