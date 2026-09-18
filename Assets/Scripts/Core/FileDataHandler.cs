using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private string CombinedFullString;
    

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;

        CombinedFullString = Path.Combine(dataDirPath, dataFileName);
    }

    public GameData Load()
    {
        GameData loadedData = null;

        if (File.Exists(CombinedFullString))
        {
            try
            {
                //load the serialized data from file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(CombinedFullString, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Error loading data: " + e);
            }
        }

        return loadedData;
    }

    public void SaveData(GameData data)
    {
        try
        {
            //create directory path
            Directory.CreateDirectory(Path.GetDirectoryName(CombinedFullString));

            //serialize json data
            string dataToStore = JsonUtility.ToJson(data, true);

            //write file
            using (FileStream stream = new FileStream(CombinedFullString, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }

            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error saving data: " + e);
        }
    }
}
