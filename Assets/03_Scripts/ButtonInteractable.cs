using UnityEngine;

public class ButtonInteractable : Interactable
{
    public GateController gate;
    public FuseHolder fuseHolder;

    public override void Interact()
    {
        if (fuseHolder == null)
        {
            Debug.LogWarning("FuseHolder nicht zugewiesen!");
            return;
        }

        if (fuseHolder.CanActivateGate())
        {
            if (gate != null)
            {
                gate.OpenGate();
            }
            else
            {
                Debug.LogWarning("Kein GateController zugewiesen!");
            }
        }
    }
}