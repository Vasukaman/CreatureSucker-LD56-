using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VacuumCleanerCounter : MonoBehaviour
{
    [SerializeField]
    private VacuumCleaner vacuumCleaner; // Reference to the VacuumCleaner script
    [SerializeField]
    private TMP_Text creatureCountText; // Reference to the UI text element

    private void Update()
    {
        UpdateCreatureCountText();
    }

    private void UpdateCreatureCountText()
    {
        if (creatureCountText != null && vacuumCleaner != null)
        {
            int count = vacuumCleaner.GetStoredCreatures().Count; // Get the count of stored creatures
            creatureCountText.text = "" + count; // Update the UI text
        }
    }
}
