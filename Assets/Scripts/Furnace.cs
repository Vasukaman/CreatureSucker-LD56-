using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public List<GameObject> storedCreatures = new List<GameObject>();
    [SerializeField]
    private float transferSpeed = 0.5f; // Time delay between each creature transfer

    private bool playerInRange = false; // Track if the player/vacuum is within range

    void Update()
    {
        // Check if RMB is pressed while player is in range
        if (playerInRange && Input.GetMouseButtonDown(1)) // Right Mouse Button
        {
            Debug.Log("RMB in range");
            VacuumCleaner vacuum = FindObjectOfType<VacuumCleaner>(); // Find the VacuumCleaner component
            if (vacuum != null)
            {
                StartCoroutine(TransferCreatures(vacuum));
            }
        }
    }

    private IEnumerator TransferCreatures(VacuumCleaner vacuum)
    {
        List<GameObject> creaturesToTransfer = vacuum.GetStoredCreatures();
        Debug.Log("Starting transfer. Creatures to transfer: " + creaturesToTransfer.Count);

        while (creaturesToTransfer.Count > 0)
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

            yield return new WaitForSeconds(transferSpeed);
        }
        Debug.Log("Transfer complete.");
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
