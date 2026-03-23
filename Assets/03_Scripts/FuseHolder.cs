using UnityEngine;

public class FuseHolder : Interactable
{
    public string requiredItemID;
    private bool isFuseInserted = false;
    [SerializeField] private GameObject fuse;

    [Header("Strom")]
    public bool powerOn = false;

    // Wird vom PlayerInteractor aufgerufen
    public override void Interact()
    {
        // Immer möglich: Sicherung ein- oder ausstecken
        if (isFuseInserted)
        {
            RemoveFuse();
        }
        else
        {
            // Prüfen, ob Sicherung im Inventar liegt
            if (InventorySystem.Instance.HasItem(requiredItemID))
            {
                InsertFuse(requiredItemID);
            }
            else
            {
                Debug.Log("Keine Sicherung im Inventar!");
            }
        }
    }

    // Sicherung einsetzen
    public void InsertFuse(string itemID)
    {
        fuse.gameObject.SetActive(true);
        isFuseInserted = true;
        InventorySystem.Instance.RemoveItem(itemID);
        Debug.Log("Sicherung eingesetzt!");
    }

    // Sicherung wieder rausnehmen
    public void RemoveFuse()
    {
        fuse.gameObject.SetActive(false);
        isFuseInserted = false;
        InventorySystem.Instance.AddItemByID(requiredItemID);
        Debug.Log("Sicherung wieder entfernt und ins Inventar gelegt!");
    }

    // Prüfen, ob Tor aktiviert werden darf
    public bool CanActivateGate()
    {
        if (!powerOn)
        {
            Debug.Log("Kein Strom!");
            return false;
        }
        if (!isFuseInserted)
        {
            Debug.Log("Keine Sicherung gefunden!");
            return false;
        }
        return true;
    }

    public bool IsFuseInserted()
    {
        return isFuseInserted;
    }

    public bool IsPowered()
    {
        return powerOn;
    }
}