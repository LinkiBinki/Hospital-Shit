using UnityEngine;
using System.Collections;

public class DoorInteractable : Interactable
{
    [Header("Door")]
    public Transform door;
    public float openAngle = 90f;
    public float speed = 120f;

    [Header("Lock")]
    public bool isLocked = false;

    [Tooltip("Wenn leer = kein Schlüssel nötig")]
    public string requiredItemID = "";

    public float shakeAngle = 10f;
    public int shakeCount = 2;
    public float shakeSpeed = 300f;

    bool isOpen = false;
    bool isMoving = false;

    Quaternion closedRot;
    Quaternion openRot;


    void Start()
    {
        closedRot = door.localRotation;
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);
    }

    public override void Interact()
    {
        Debug.Log("Interact");
        if (isMoving) return;

        // 1️⃣ Tür ist NICHT locked → immer öffnen
        if (!isLocked)
        {
            StartCoroutine(RotateDoor());
            Debug.Log("Locked!");
            return;
        }

        // 2️⃣ Tür ist locked

        // braucht keinen key → bleibt locked
        if (string.IsNullOrEmpty(requiredItemID))
        {
            Debug.Log("Tür ist abgeschlossen");
            StartCoroutine(ShakeDoor());
            return;
        }

        // braucht key → prüfen
        if (InventorySystem.Instance.HasItem(requiredItemID))
        {
            Debug.Log("Schlüssel passt");

            isLocked = false;

            // optional Key verbrauchen
            // InventorySystem.Instance.RemoveItem(requiredItemID);

            StartCoroutine(RotateDoor());
        }
        else
        {
            Debug.Log("Schlüssel fehlt");
            StartCoroutine(ShakeDoor());
        }
    }


    IEnumerator RotateDoor()
    {
        isMoving = true;

        Quaternion target =
            isOpen ? closedRot : openRot;

        while (Quaternion.Angle(door.localRotation, target) > 0.1f)
        {
            door.localRotation = Quaternion.RotateTowards(
                door.localRotation,
                target,
                speed * Time.deltaTime
            );

            yield return null;
        }

        door.localRotation = target;

        isOpen = !isOpen;
        isMoving = false;
    }


    IEnumerator ShakeDoor()
    {
        isMoving = true;

        for (int i = 0; i < shakeCount; i++)
        {
            Quaternion forward =
                closedRot * Quaternion.Euler(0, shakeAngle, 0);

            Quaternion back =
                closedRot * Quaternion.Euler(0, -shakeAngle, 0);

            while (Quaternion.Angle(door.localRotation, forward) > 0.5f)
            {
                door.localRotation = Quaternion.RotateTowards(
                    door.localRotation,
                    forward,
                    shakeSpeed * Time.deltaTime
                );

                yield return null;
            }

            while (Quaternion.Angle(door.localRotation, back) > 0.5f)
            {
                door.localRotation = Quaternion.RotateTowards(
                    door.localRotation,
                    back,
                    shakeSpeed * Time.deltaTime
                );

                yield return null;
            }
        }

        while (Quaternion.Angle(door.localRotation, closedRot) > 0.5f)
        {
            door.localRotation = Quaternion.RotateTowards(
                door.localRotation,
                closedRot,
                shakeSpeed * Time.deltaTime
            );

            yield return null;
        }

        door.localRotation = closedRot;

        isMoving = false;
    }
}