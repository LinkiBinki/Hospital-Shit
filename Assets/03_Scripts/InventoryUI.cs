using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image fuseIcon;       // Das Image für die Sicherung
    public FuseHolder fuseHolder; // Referenz zum FuseHolder

    void Update()
    {
        if (fuseHolder == null || fuseIcon == null) return;

        // Zeigt Icon nur an, wenn Sicherung **im Inventar** liegt, nicht im FuseHolder
        bool inInventory = InventorySystem.Instance.HasItem(fuseHolder.requiredItemID);

        fuseIcon.enabled = inInventory;
    }
}