using UnityEngine;

public class PickupItem : Interactable
{
    public string itemID;

    public override void Interact()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            FuseHolder holder = hit.collider.GetComponent<FuseHolder>();

            if (holder != null)
            {
                // Nur einsetzen wenn Holder dieses Item braucht
                if (holder.requiredItemID == itemID)
                {
                    holder.InsertFuse(itemID);
                    gameObject.SetActive(false);
                    return;
                }
            }
        }

        // sonst ins Inventar
        InventorySystem.Instance.AddItemByID(itemID);
        gameObject.SetActive(false);

        Debug.Log(itemID + " ins Inventar gelegt");
    }
}