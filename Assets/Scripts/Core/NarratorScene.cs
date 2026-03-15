using UnityEngine;

[CreateAssetMenu(fileName = "NarratorScene", menuName = "ScriptableObjects/NarratorScene", order = 1)]
public class NarratorScene : ScriptableObject
{
    [TextArea]
    public string[] Lines;

    public string GetStringByIndex(int index)
    {
        if (index < Lines.Length)
        {
            return Lines[index];
        }


        Debug.LogError("Invalid index: " + index);
        return "";
    }
}
