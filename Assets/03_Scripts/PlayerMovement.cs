using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchLerp = 8f;
    public float cameraCrouchOffset = 0.6f; // Wie tief die Kamera sinkt

    [Header("Headbob")]
    public Transform cameraHolder;
    public float bobSpeed = 7f;
    public float bobAmount = 0.05f;

    Rigidbody rb;
    CapsuleCollider col;

    Vector2 input;
    bool sprint;
    bool crouchPressed;
    bool isCrouching;

    float currentSpeed;
    float defaultCamY;
    float currentCamTargetY; // Die Basis-Höhe (geduckt oder stehend)
    float bobTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (cameraHolder != null)
        {
            defaultCamY = cameraHolder.localPosition.y;
            currentCamTargetY = defaultCamY;
        }
    }

    void Update()
    {
        ReadInput();
        HandleCrouch();
        Headbob();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ReadInput()
    {
        float x = 0;
        float z = 0;

        if (Keyboard.current.aKey.isPressed) x = -1;
        if (Keyboard.current.dKey.isPressed) x = 1;
        if (Keyboard.current.wKey.isPressed) z = 1;
        if (Keyboard.current.sKey.isPressed) z = -1;

        input = new Vector2(x, z);
        sprint = Keyboard.current.leftShiftKey.isPressed;

        if (Keyboard.current.cKey.wasPressedThisFrame)
            crouchPressed = true;
    }

    void Move()
    {
        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (sprint)
            currentSpeed = sprintSpeed;
        else
            currentSpeed = walkSpeed;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 vel = rb.linearVelocity;
        Vector3 target = move * currentSpeed;

        Vector3 newVel = new Vector3(target.x, vel.y, target.z);

        rb.linearVelocity = Vector3.Lerp(vel, newVel, 10f * Time.fixedDeltaTime);
    }

    void HandleCrouch()
    {
        if (crouchPressed)
        {
            isCrouching = !isCrouching;
            crouchPressed = false;
        }

        // Ziel-Werte berechnen
        float targetColHeight = isCrouching ? crouchHeight : standHeight;
        float targetCamY = isCrouching ? (defaultCamY - cameraCrouchOffset) : defaultCamY;

        // Collider sanft anpassen
        col.height = Mathf.Lerp(col.height, targetColHeight, Time.deltaTime * crouchLerp);

        // Die Basis-Höhe für die Kamera sanft anpassen (wichtig für Headbob)
        currentCamTargetY = Mathf.Lerp(currentCamTargetY, targetCamY, Time.deltaTime * crouchLerp);
    }

    void Headbob()
    {
        if (cameraHolder == null) return;

        Vector3 vel = rb.linearVelocity;
        Vector3 flat = new Vector3(vel.x, 0, vel.z);
        Vector3 pos = cameraHolder.localPosition;

        if (flat.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * (isCrouching ? bobSpeed * 0.7f : bobSpeed);
            float yOffset = Mathf.Sin(bobTimer) * bobAmount;

            // Wir addieren den Bobbing-Sinus auf die aktuell berechnete Crouch-Höhe
            pos.y = currentCamTargetY + yOffset;
        }
        else
        {
            bobTimer = 0;
            // Wenn wir stehen, lerpen wir nur zur Basis-Höhe (geduckt oder stehend)
            pos.y = Mathf.Lerp(pos.y, currentCamTargetY, Time.deltaTime * 5f);
        }

        cameraHolder.localPosition = pos;
    }
}