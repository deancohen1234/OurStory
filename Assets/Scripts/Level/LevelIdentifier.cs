using UnityEngine;

//Set data store to what level we are now in
public class LevelIdentifier : MonoBehaviour
{
    public int LevelIndex = -1;
    public Color Color1;
    public Color Color2;

    public Material BackgroundMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DataStore.Instance != null)
        {
            DataStore.Instance.SetCurrentLevel(LevelIndex); 
        }

        BackgroundMaterial.SetColor("_Color1", Color1);
        BackgroundMaterial.SetColor("_Color2", Color2);
    }
}
