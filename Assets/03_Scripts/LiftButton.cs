using UnityEngine;

public class LiftButton : Interactable
{
    public LiftController lift;

    public bool moveUp;

    public override void Interact()
    {
        if (moveUp)
            lift.MoveUp();
        else
            lift.MoveDown();
    }
}