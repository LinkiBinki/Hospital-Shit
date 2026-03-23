using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Images")]
    public Image fuseImage;
    public Image redKeyImage;
    public Image blueKeyImage;

    void Update()
    {
        fuseImage.enabled =
            InventorySystem.Instance.HasItem("fuse");

        redKeyImage.enabled =
            InventorySystem.Instance.HasItem("key_red");

        blueKeyImage.enabled =
            InventorySystem.Instance.HasItem("key_blue");
    }
}