using UnityEngine;

//Set data store to what level we are now in
public class LevelIdentifier : MonoBehaviour
{
    public int LevelIndex = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DataStore.Instance != null)
        {
            DataStore.Instance.SetCurrentLevel(LevelIndex); 
        }
    }
}
