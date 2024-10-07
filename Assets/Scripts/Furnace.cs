using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Furnace : MonoBehaviour
{
    public List<GameObject> storedCreatures = new List<GameObject>();
    [SerializeField]
    private float transferSpeed = 0.5f;
    [SerializeField]
    private int maxCapacity = 50;

    [SerializeField]
    private Transform _transferPointPosition;

    [SerializeField]
    private TMP_Text creatureCountText;

    [SerializeField]
    private GameObject _creatureBallPrefab;
    private bool playerInRange = false;
    private bool isTransferring = false;

    void Update()
    {
        if (playerInRange && Input.GetMouseButton(1))
        {
            if (!isTransferring)
            {
                Debug.Log("RMB in range, starting transfer");
                VacuumCleaner vacuum = FindObjectOfType<VacuumCleaner>();
                if (vacuum != null)
                {
                    StartCoroutine(TransferCreatures(vacuum));
                }
            }
        }
        else if (!Input.GetMouseButton(1) && isTransferring)
        {
            StopAllCoroutines();
            isTransferring = false;
            Debug.Log("Transfer stopped.");
        }
    }

    private IEnumerator TransferCreatures(VacuumCleaner vacuum)
    {
        isTransferring = true;
        List<GameObject> creaturesToTransfer = vacuum.GetStoredCreatures();
        Debug.Log("Starting transfer. Creatures to transfer: " + creaturesToTransfer.Count);

        while (creaturesToTransfer.Count > 0 && Input.GetMouseButton(1))
        {
            if (storedCreatures.Count >= maxCapacity)
            {
                Debug.Log("Furnace is full. Cannot store more creatures.");
                break;
            }

            GameObject creature = creaturesToTransfer[0]; // Get the first creature
            creaturesToTransfer.RemoveAt(0); // Remove from the list in VacuumCleaner
            vacuum.RemoveCreature(creature); // Remove the creature from Vacuum's list

            storedCreatures.Add(creature);

            creature.SetActive(true);
            creature.transform.position = _transferPointPosition.position;

            UpdateCreatureCountText();

            Debug.Log("Transferred creature: " + creature.name);
            Debug.Log("Creatures remaining in Vacuum: " + creaturesToTransfer.Count);
            Debug.Log("Total creatures in Furnace: " + storedCreatures.Count);

            yield return new WaitForSeconds(transferSpeed);
        }

        Debug.Log("Transfer complete.");
        isTransferring = false;
    }

    public void SellCreatures()
    {
        int soldCount = storedCreatures.Count;

        foreach (GameObject creature in storedCreatures)
        {
            Destroy(creature);
        }
        storedCreatures.Clear(); // Clear the list of stored creatures
        Debug.Log($"Sold {soldCount} creatures!");

        UpdateCreatureCountText(); // Update the UI text to reflect the new creature count
    }


    private void UpdateCreatureCountText()
    {
        if (creatureCountText != null)
        {
            creatureCountText.text = "Caught: " + storedCreatures.Count;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered Furnace range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited Furnace range");
        }
    }
}
