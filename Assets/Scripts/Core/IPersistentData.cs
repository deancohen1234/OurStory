using UnityEngine;

public interface IPersistentData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual void LoadData(GameData data)
    {
        
    }

    // Update is called once per frame
    virtual void SaveData(ref GameData data)
    {
        
    }
}
