using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    private HashSet<string> items = new HashSet<string>(); // IDs der aufgenommenen Items

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(PickupItem item)
    {
        items.Add(item.itemID);
        Debug.Log($"Inventar: {item.itemID} hinzugefügt.");
    }

    public bool HasItem(string itemID)
    {
        return items.Contains(itemID);
    }
    public void RemoveItem(string itemID)
    {
        if (items.Contains(itemID))
        {
            items.Remove(itemID);
            Debug.Log($"{itemID} aus Inventar entfernt");
        }
    }

    public HashSet<string> GetAllItemIDs()
    {
        return items;
    }
    public void AddItemByID(string itemID)
    {
        if (!items.Contains(itemID))
        {
            items.Add(itemID);
            Debug.Log($"{itemID} ins Inventar gelegt (per ID)");
        }
    }
}