using UnityEngine;

public class PowerLever : Interactable
{
    [Header("Welcher Sicherungskasten wird gesteuert?")]
    public FuseHolder fuseHolder;

    [Header("Hebelzustand")]
    public bool isOn = false; // Aktueller Zustand des Hebels

    public override void Interact()
    {
        if (fuseHolder == null)
        {
            Debug.LogWarning("FuseHolder nicht zugewiesen!");
            return;
        }

        // Hebel umschalten
        isOn = !isOn;
        fuseHolder.powerOn = isOn;

        Debug.Log(isOn ? "Strom eingeschaltet!" : "Strom ausgeschaltet!");
    }
}