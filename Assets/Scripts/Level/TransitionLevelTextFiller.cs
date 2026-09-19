using UnityEngine;
using TMPro;

public class TransitionLevelTextFiller : MonoBehaviour
{
    public string[] PossibleLines;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = PossibleLines[Random.Range(0, PossibleLines.Length)];
    }

}
