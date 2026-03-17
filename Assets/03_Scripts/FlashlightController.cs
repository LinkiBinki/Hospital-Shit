using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Target")]
    public Transform cameraTransform;   // Hier die Main Camera reinziehen

    [Header("Light Settings")]
    public GameObject lightObject;
    public bool isOn = true;

    [Header("Smooth Settings")]
    public float followSpeed = 10f;     // Rotation Speed
    public float positionLerpSpeed = 20f; // Wie schnell sie an der Kamera klebt

    [Header("Offset")]
    public Vector3 positionOffset = new Vector3(0.3f, -0.3f, 0.5f);

    void Start()
    {
        // Wir lösen die Taschenlampe beim Start von jedem Parent, 
        // damit sie völlig frei im Weltraum ist.
        transform.SetParent(null);

        if (lightObject != null)
            lightObject.SetActive(isOn);
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        HandleInput();

        // 1. POSITION FOLGEN
        // Wir berechnen die Zielposition relativ zur Kamera
        Vector3 targetPos = cameraTransform.TransformPoint(positionOffset);
        // Wir nutzen Lerp für die Position, damit auch kleine Ruckler beim Laufen geschmeidig werden
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * positionLerpSpeed);

        // 2. ROTATION FOLGEN (Das ist jetzt für ALLE Achsen smooth)
        // Da wir kein Kind mehr sind, hat der Player keinen Einfluss auf uns.
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            cameraTransform.rotation,
            Time.deltaTime * followSpeed
        );
    }

    void HandleInput()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isOn = !isOn;
            if (lightObject != null)
                lightObject.SetActive(isOn);
        }
    }
}