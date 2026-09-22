using UnityEngine;
using UnityEngine.InputSystem;
public class Orb : Pickupable, IPersistentData
{
    [SerializeField]
    private string ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    private bool bIsCollected = false;

    private void Update()
    {
        if (Keyboard.current.nKey.IsPressed())
        {
            OnCollected();
        }
    }

    protected override void OnCollected()
    {
        base.OnCollected();

        bIsCollected = true;

        this.CollectableManager.OnOrbCollected(this);
    }

    public virtual void LoadData(GameData data)
    {
        if (data.OrbObtainedMap.ContainsKey(ID))
        {
            bIsCollected = data.OrbObtainedMap[ID];
        }

        //destroy immediately if collectable is already gotten
        if (bIsCollected)
        {
            DestroyPickupable();
        }
    }

    public virtual void SaveData(ref GameData data)
    {
        if (!data.OrbObtainedMap.ContainsKey(ID))
        {
            data.OrbObtainedMap.Add(ID, bIsCollected);
        }
        else
        {
            data.OrbObtainedMap[ID] = bIsCollected;
        }
        
    }

    public string GetGuid()
    {
        return ID;
    }
}
