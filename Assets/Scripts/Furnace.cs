using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public List<GameObject> storedCreatures = new List<GameObject>();
    [SerializeField]
    private float transferSpeed = 0.5f; // Time delay between each creature transfer

    private bool playerInRange = false; // Track if the player/vacuum is within range
    private bool isTransferring = false; // Track if creatures are currently being transferred

    void Update()
    {
        // Check if RMB is pressed while player is in range
        if (playerInRange && Input.GetMouseButton(1)) // Right Mouse Button held down
        {
            if (!isTransferring) // Start transferring if not already in progress
            {
                Debug.Log("RMB in range, starting transfer");
                VacuumCleaner vacuum = FindObjectOfType<VacuumCleaner>(); // Find the VacuumCleaner component
                if (vacuum != null)
                {
                    StartCoroutine(TransferCreatures(vacuum));
                }
            }
        }
        else if (!Input.GetMouseButton(1) && isTransferring) // Stop transferring if RMB is released
        {
            StopAllCoroutines();
            isTransferring = false; // Reset transferring state
            Debug.Log("Transfer stopped.");
        }
    }

    private IEnumerator TransferCreatures(VacuumCleaner vacuum)
    {
        isTransferring = true; // Set transferring state
        List<GameObject> creaturesToTransfer = vacuum.GetStoredCreatures();
        Debug.Log("Starting transfer. Creatures to transfer: " + creaturesToTransfer.Count);

        while (creaturesToTransfer.Count > 0 && Input.GetMouseButton(1)) // Continue while RMB is held down
        {
            GameObject creature = creaturesToTransfer[0];
            creaturesToTransfer.RemoveAt(0);
            storedCreatures.Add(creature);

            // Reactivate and move the creature to the Furnace position
            creature.SetActive(true);
            creature.transform.position = transform.position; // Snap to the Furnace position

            // Prevent the creature from leaving the furnace area
            creature.AddComponent<StayInFurnace>().SetFurnaceTrigger(GetComponent<Collider>());

            Debug.Log("Transferred creature: " + creature.name);
            Debug.Log("Creatures remaining in Vacuum: " + creaturesToTransfer.Count);
            Debug.Log("Total creatures in Furnace: " + storedCreatures.Count);

            yield return new WaitForSeconds(transferSpeed); // Wait before transferring the next creature
        }

        Debug.Log("Transfer complete.");
        isTransferring = false; // Reset transferring state
    }

    // Detect when the player enters the Furnace area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure VacuumCleaner has the tag "Player"
        {
            playerInRange = true;
            Debug.Log("Player entered Furnace range");
        }
    }

    // Detect when the player exits the Furnace area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure VacuumCleaner has the tag "Player"
        {
            playerInRange = false;
            Debug.Log("Player exited Furnace range");
        }
    }
}
