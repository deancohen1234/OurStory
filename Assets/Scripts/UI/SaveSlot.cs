using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for handling UI selection events
using TMPro;
using UnityEngine.SceneManagement;


public class SaveSlot : Selectable, ISubmitHandler
{
    public Image FillImage;

    public TextMeshProUGUI PercentText;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI CoinCountText;

    public string SaveDataName;

    protected override void Start()
    {
        base.Start();

        if (!Application.isPlaying)
        {
            return;
        }

        FileDataHandler handler = new FileDataHandler(Application.persistentDataPath, SaveDataName);

        GameData gameData = handler.Load();

        if (gameData == null)
        {
            FillImage.fillAmount = 0;
            PercentText.text = "0%";

            CoinCountText.text = "0";
        }
        else
        {
            float percent = GetCompletionPercentage(gameData);
            FillImage.fillAmount = percent;
            PercentText.text = Mathf.RoundToInt(percent * 100f).ToString() + "%";

            CoinCountText.text = gameData.TotalCoins.ToString();
        }

        
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayerPrefs.SetString("SaveFileName", SaveDataName);

        SceneManager.LoadScene(1);
    }

    private float GetCompletionPercentage(GameData data)
    {
        int orbsCollected = 0;

        foreach (bool orbCollectedBool in data.OrbObtainedMap.Values)
        {
            if (orbCollectedBool)
            {
                orbsCollected++;
            }
        }

        float totalOrbsCollected = (float)orbsCollected / 15f;
        Debug.Log("Orbs Collected: " + data.OrbObtainedMap.Count);
        return totalOrbsCollected;
    } 
}
