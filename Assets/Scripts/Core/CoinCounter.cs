using UnityEngine;

using TMPro;

public class CoinCounter : CounterContainer
{
    public TextMeshProUGUI Text;

    private int TotalCoinsCollected;

    private void Start()
    {
        CollectableManager.SubscribeToCoinCollected(OnCollected);
    }

    protected override void OnCollected(Pickupable coin)
    {
        TotalCoinsCollected++;
        Text.text = TotalCoinsCollected.ToString();
    }

    public override void LoadData(GameData data)
    {
        TotalCoinsCollected = data.TotalCoins;
        Text.text = TotalCoinsCollected.ToString();
    }

    // Update is called once per frame
    public override void SaveData(ref GameData data)
    {
        data.TotalCoins = TotalCoinsCollected;
        data.TotalCollectablesObtained = 50;
    }
}
