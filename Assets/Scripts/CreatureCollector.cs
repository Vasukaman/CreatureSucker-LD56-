using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCollector : MonoBehaviour
{
    [SerializeField]
    private Furnace furnace; // Reference to the Furnace script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a creature
        if (other.CompareTag("Creature"))
        {
            GameObject creature = other.gameObject;

            // Add the creature to the Furnace's stored creatures list
            if (furnace != null && !furnace.storedCreatures.Contains(creature))
            {
                furnace.storedCreatures.Add(creature);
                furnace.UpdateCreatureCountText();

                // Optionally, deactivate the creature if you want to hide it after collection
                creature.SetActive(false); 

                Debug.Log("Creature collected by Furnace!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // You can implement additional functionality if needed when the creature exits the trigger.
    }
}
