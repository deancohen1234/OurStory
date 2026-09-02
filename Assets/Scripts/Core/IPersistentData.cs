using UnityEngine;

public interface IPersistentData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void LoadData(GameData data)
    {
        
    }

    // Update is called once per frame
    void SaveData(ref GameData data)
    {
        
    }
}
