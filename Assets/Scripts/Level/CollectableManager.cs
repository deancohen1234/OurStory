using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    public Collectable[] Colllectables;

    public GameObject[] CollectableIcons;

    private int CollectableIndex;

    public delegate void OnCollected(Collectable collectable);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Colllectables.Length; i++)
        {
            OnCollected onCollectedDelegate = OnCollectableCollected;
            Colllectables[i].AddListener(onCollectedDelegate);
        }
    }

    void OnCollectableCollected(Collectable collectable)
    {
        Debug.Log("Collected!: " + collectable.name);

        CanvasGroup Group = CollectableIcons[CollectableIndex].GetComponent<CanvasGroup>();
        Group.alpha = 1;

        CollectableIndex++;
    }
}
