using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    private HashSet<string> items = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItemByID(string id)
    {
        if (!items.Contains(id))
        {
            items.Add(id);
        }
    }

    public void RemoveItem(string itemID)
    {
        if (items.Contains(itemID))
        {
            items.Remove(itemID);
        }
    }

    public bool HasItem(string itemID)
    {
        return items.Contains(itemID);
    }

    public List<string> GetItems()
    {
        return new List<string>(items);
    }
}