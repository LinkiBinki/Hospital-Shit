using UnityEngine;
using System.Collections;

public class LiftController : MonoBehaviour
{
    [Header("Platform")]
    public Transform platform;

    public Transform bottomPoint;
    public Transform topPoint;

    public float speed = 2f;

    [Header("Power / Fuse")]
    public bool requirePower = true;
    public bool requireFuse = true;

    public FuseHolder fuseHolder;

    bool isMoving = false;
    bool isUp = false;


    public void MoveUp()
    {
        if (isMoving) return;

        if (!CanUse())
            return;

        if (!isUp)
            StartCoroutine(Move(topPoint.position, true));
    }


    public void MoveDown()
    {
        if (isMoving) return;

        if (!CanUse())
            return;

        if (isUp)
            StartCoroutine(Move(bottomPoint.position, false));
    }


    bool CanUse()
    {
        if (fuseHolder == null)
        {
            Debug.Log("FuseHolder fehlt!");
            return false;
        }

        if (requireFuse && !fuseHolder.IsFuseInserted())
        {
            Debug.Log("Keine Sicherung!");
            return false;
        }

        if (requirePower && !fuseHolder.IsPowered())
        {
            Debug.Log("Kein Strom!");
            return false;
        }

        return true;
    }


    IEnumerator Move(Vector3 target, bool goingUp)
    {
        isMoving = true;

        while (Vector3.Distance(platform.position, target) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(
                platform.position,
                target,
                speed * Time.deltaTime
            );

            yield return null;
        }

        platform.position = target;

        isUp = goingUp;
        isMoving = false;
    }
}