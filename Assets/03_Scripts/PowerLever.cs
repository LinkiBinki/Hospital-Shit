using UnityEngine;

public class PowerLever : Interactable
{
    [SerializeField] private GameObject On;
    [SerializeField] private GameObject Off;

    [Header("Welche Sicherungskästen werden gesteuert?")]
    public FuseHolder[] fuseHolders;

    [Header("Hebelzustand")]
    public bool isOn = false;

    public override void Interact()
    {
        if (fuseHolders.Length == 0)
        {
            Debug.LogWarning("Keine FuseHolder zugewiesen!");
            return;
        }

        // Zustand wechseln
        isOn = !isOn;

        // ALLE FuseHolder setzen
        foreach (FuseHolder holder in fuseHolders)
        {
            if (holder != null)
                holder.powerOn = isOn;
        }

        // Hebel Grafik
        if (isOn)
        {
            On.SetActive(true);
            Off.SetActive(false);
        }
        else
        {
            On.SetActive(false);
            Off.SetActive(true);
        }

        Debug.Log(isOn ? "Strom eingeschaltet!" : "Strom ausgeschaltet!");
    }
}