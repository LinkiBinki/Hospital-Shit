using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public Camera cam;

    [Header("Raycast")]
    public float range = 3f;
    public LayerMask interactMask;

    [Header("Crosshair")]
    public Image normalCrosshair;
    public Image interactCrosshair;

    Interactable current;
    public bool showDebugRay = true;

    void Start()
    {
        SetNormal();
    }

    void Update()
    {
        CheckInteractable();
        HandleInput();
    }

    void CheckInteractable()
    {
        current = null;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, range, interactMask);

        // Debug Ray anzeigen
        if (showDebugRay)
        {
            Color color = hitSomething ? Color.green : Color.red;
            Debug.DrawRay(ray.origin, ray.direction * range, color);
        }

        if (hitSomething)
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                current = hit.collider.GetComponent<Interactable>();

                if (current != null)
                {
                    SetInteract();
                    return;
                }
            }
        }
        if (hitSomething)
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }

        SetNormal();
    }

    void HandleInput()
    {
        if (current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            current.Interact();
        }
    }

    void SetNormal()
    {
        normalCrosshair.enabled = true;
        interactCrosshair.enabled = false;
    }

    void SetInteract()
    {
        normalCrosshair.enabled = false;
        interactCrosshair.enabled = true;
    }
}