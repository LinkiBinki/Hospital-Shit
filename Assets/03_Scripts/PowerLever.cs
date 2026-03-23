using UnityEngine;

public class PowerLever : Interactable
{
    [SerializeField] private GameObject On;
    [SerializeField] private GameObject Off;
    
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
        if (isOn)
        {
            On.gameObject.SetActive(true);
            Off.gameObject.SetActive(false);
        }
        else { 
            On.gameObject.SetActive(false);
            Off.gameObject.SetActive(true);
        }
            Debug.Log(isOn ? "Strom eingeschaltet!" : "Strom ausgeschaltet!");
    }
}