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

    [Header("Crouch & Ceiling Check")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchLerp = 8f;
    public float cameraCrouchOffset = 0.6f;
    public float ceilingCheckDistance = 1.0f; // Wie weit nach oben geprüft wird
    public LayerMask crouchMask;              // Layer, die als "Decke" gelten (meist Default)

    [Header("Step System")]
    public float stepHeight = 0.45f;
    public float stepCheckDistance = 0.4f;
    public float stepUpSpeed = 8f;
    public float groundSnapForce = 5f;
    public LayerMask groundMask;

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
    float currentCamTargetY;
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
        HandleSteps();
        GroundSnap();
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

        // Nur wenn C gedrückt wird, geben wir den Befehl zum Toggeln
        if (Keyboard.current.cKey.wasPressedThisFrame)
            crouchPressed = true;
    }
    void HandleSteps()
    {
        Vector3 velocity = rb.linearVelocity;

        if (velocity.magnitude < 0.1f)
            return;

        Vector3 dir = new Vector3(velocity.x, 0, velocity.z).normalized;

        Vector3 originLow = transform.position + Vector3.up * 0.05f;
        Vector3 originHigh = transform.position + Vector3.up * stepHeight;

        // check obstacle
        if (Physics.Raycast(originLow, dir, stepCheckDistance, groundMask))
        {
            // check free space above
            if (!Physics.Raycast(originHigh, dir, stepCheckDistance, groundMask))
            {
                // smooth step up via velocity
                rb.linearVelocity = new Vector3(
                    velocity.x,
                    stepUpSpeed,
                    velocity.z
                );
            }
        }
    }
    void GroundSnap()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.6f, groundMask))
        {
            if (hit.distance > 0.05f)
            {
                rb.linearVelocity += Vector3.down * groundSnapForce;
            }
        }
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
        // Wenn wir aufstehen wollen (isCrouching ist true), prüfen wir, ob Platz ist
        if (isCrouching && crouchPressed)
        {
            if (CanStandUp())
            {
                isCrouching = false;
            }
            // Falls kein Platz ist, ignorieren wir den Tastendruck einfach
            crouchPressed = false;
        }
        else if (!isCrouching && crouchPressed)
        {
            isCrouching = true;
            crouchPressed = false;
        }

        float targetColHeight = isCrouching ? crouchHeight : standHeight;
        float targetCamY = isCrouching ? (defaultCamY - cameraCrouchOffset) : defaultCamY;

        col.height = Mathf.Lerp(col.height, targetColHeight, Time.deltaTime * crouchLerp);
        currentCamTargetY = Mathf.Lerp(currentCamTargetY, targetCamY, Time.deltaTime * crouchLerp);
    }

    // Die Logik für den Decken-Check
    bool CanStandUp()
    {
        float radius = col.radius * 0.9f;

        Vector3 origin = transform.position + Vector3.up * (col.height * 0.5f);

        float checkDistance = standHeight - crouchHeight;

        bool hit = Physics.SphereCast(
            origin,
            radius,
            Vector3.up,
            out RaycastHit h,
            checkDistance,
            crouchMask,
            QueryTriggerInteraction.Ignore
        );

        return !hit;
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
            pos.y = currentCamTargetY + yOffset;
        }
        else
        {
            bobTimer = 0;
            pos.y = Mathf.Lerp(pos.y, currentCamTargetY, Time.deltaTime * 5f);
        }

        cameraHolder.localPosition = pos;
    }

    // Hilfreich, um den Raycast im Editor zu sehen
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.up * ceilingCheckDistance);
    }
}