using UnityEngine;

public class PickupItem : Interactable
{
    public string itemID;

    public override void Interact()
    {
        // Prüfen, ob wir vor einem FuseHolder stehen
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            FuseHolder holder = hit.collider.GetComponent<FuseHolder>();
            if (holder != null)
            {
                // Sicherung direkt in den FuseHolder einsetzen
                holder.InsertFuse(itemID);
                gameObject.SetActive(false); // Sicherung verschwindet aus der Welt
                return;
            }
        }

        // Standard: Sicherung ins Inventar aufnehmen
        InventorySystem.Instance.AddItem(this);
        gameObject.SetActive(false);
        Debug.Log($"{itemID} ins Inventar gelegt!");
    }
}