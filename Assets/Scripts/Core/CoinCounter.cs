using UnityEngine;

using TMPro;

public class CoinCounter : CounterContainer
{

    private int TotalCoinsCollected;

    protected override void OnCollected(Pickupable coin)
    {
        TotalCoinsCollected++;
        Text.text = TotalCoinsCollected.ToString();
    }

    public override void LoadData(GameData data)
    {
        TotalCoinsCollected = data.TotalCoins;
    }

    // Update is called once per frame
    public override void SaveData(ref GameData data)
    {
        data.TotalCoins = TotalCoinsCollected;
        data.TotalCollectablesObtained = 50;
    }
}
