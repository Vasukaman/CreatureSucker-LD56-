using UnityEngine;

public class TransferZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            Debug.Log("Player entered the Transfer Zone");
            // You can add logic to notify the Furnace script here if needed
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the Transfer Zone");
            // You can add logic to notify the Furnace script here if needed
        }
    }
}
