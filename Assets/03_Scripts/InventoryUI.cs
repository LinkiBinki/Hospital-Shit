using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Images")]
    public Image fuseImage;
    public Image redKeyImage;
    public Image blueKeyImage;
    public Image SchraubendreherImage;

    void Update()
    {
        fuseImage.enabled =
            InventorySystem.Instance.HasItem("Sicherung");

        redKeyImage.enabled =
            InventorySystem.Instance.HasItem("S1_Karte");

        blueKeyImage.enabled =
            InventorySystem.Instance.HasItem("S2_Karte");

        blueKeyImage.enabled =
             InventorySystem.Instance.HasItem("Schraubendreher");
    }
}