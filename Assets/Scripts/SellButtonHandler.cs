using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SellButtonHandler : MonoBehaviour
{
    [SerializeField]
    private Button sellButton; // Reference to the button
    [SerializeField]
    private UnityEvent onSellCreatures; // UnityAction to invoke when button is clicked

    private void Start()
    {
        if (sellButton != null)
        {
            sellButton.onClick.AddListener(SellCreatures);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check for the "E" key
        {
            SellCreatures(); // Invoke the sell action
        }
    }

    private void SellCreatures()
    {
        Debug.Log("Selling creatures...");
        onSellCreatures.Invoke(); // Call the UnityAction
    }
}
