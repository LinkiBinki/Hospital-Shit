using UnityEngine;
using System.Collections;

public class HatchInteractable : Interactable
{
    [Header("Hatch")]
    public Transform hatch;
    public Vector3 openRotation;
    public float speed = 120f;

    [Header("Lock")]
    public bool isLocked = false;
    public string requiredItemID = "";

    bool isOpen = false;
    bool isMoving = false;

    Quaternion closedRot;
    Quaternion openRot;


    void Start()
    {
        closedRot = hatch.localRotation;
        openRot = closedRot * Quaternion.Euler(openRotation);
    }


    public override void Interact()
    {
        if (isMoving) return;

        // 🔹 nicht locked → einfach öffnen
        if (!isLocked)
        {
            StartCoroutine(RotateHatch());
            return;
        }

        // 🔹 locked aber kein key nötig → bleibt zu
        if (string.IsNullOrEmpty(requiredItemID))
        {
            Debug.Log("Klappe ist abgeschlossen");
            return;
        }

        // 🔹 key nötig
        if (InventorySystem.Instance.HasItem(requiredItemID))
        {
            Debug.Log("Item benutzt");

            isLocked = false;

            // optional verbrauchen
            // InventorySystem.Instance.RemoveItem(requiredItemID);

            StartCoroutine(RotateHatch());
        }
        else
        {
            Debug.Log("Item fehlt");
        }
    }


    IEnumerator RotateHatch()
    {
        isMoving = true;

        Quaternion target =
            isOpen ? closedRot : openRot;

        while (Quaternion.Angle(hatch.localRotation, target) > 0.1f)
        {
            hatch.localRotation = Quaternion.RotateTowards(
                hatch.localRotation,
                target,
                speed * Time.deltaTime
            );

            yield return null;
        }

        hatch.localRotation = target;

        isOpen = !isOpen;
        isMoving = false;
    }
}