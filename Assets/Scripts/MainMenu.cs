using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using UnityEngine.UI; // Needed for UI components

public class MainMenu : MonoBehaviour
{
    // Method to start the game
    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with the actual scene name)
        SceneManager.LoadScene("Playground");
    }

    // Method to exit the game
    public void ExitGame()
    {
#if UNITY_EDITOR
        // If we're in the editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If we're in a built application, quit the application
            Application.Quit();
#endif
    }
}
