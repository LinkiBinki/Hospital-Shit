using UnityEngine;

public class GateController : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 5, 0); // Wie weit sich das Tor öffnen soll
    public float openSpeed = 2f; // Einheiten pro Sekunde

    private Vector3 closedPos;
    private Vector3 targetPos;
    private bool isOpening = false;

    void Start()
    {
        closedPos = transform.position;
        targetPos = closedPos;
    }

    void Update()
    {
        if (isOpening)
        {
            // Bewegt das Tor konstant in Richtung targetPos
            transform.position = Vector3.MoveTowards(transform.position, targetPos, openSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                transform.position = targetPos;
                isOpening = false;
            }
        }
    }

    public void OpenGate()
    {
        targetPos = closedPos + openOffset;
        isOpening = true;
    }
}