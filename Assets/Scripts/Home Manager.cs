using UnityEngine;

public class HomeManager : MonoBehaviour
{
    // Reference to all your canvases
    public GameObject homepageCanvas; // Reference to your homepage canvas
    public GameObject[] otherCanvases; // Array of other canvases

    // Start is called before the first frame update
    void Start()
    {
        // Make sure only the homepage canvas is active initially
        homepageCanvas.SetActive(true);

        // Ensure other canvases are inactive
        foreach (GameObject canvas in otherCanvases)
        {
            canvas.SetActive(false);
        }
    }

    // Function to open a specific canvas and close the homepage
    public void OpenCanvas(int canvasIndex)
    {
        // Close the homepage canvas
        homepageCanvas.SetActive(false);

        // Open the desired canvas based on the index provided
        if (canvasIndex >= 0 && canvasIndex < otherCanvases.Length)
        {
            otherCanvases[canvasIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid canvas index provided.");
        }
    }

    // Optional function to return to the homepage canvas
    public void ReturnToHomePage()
    {
        // Close all other canvases
        foreach (GameObject canvas in otherCanvases)
        {
            canvas.SetActive(false);
        }

        // Open the homepage canvas
        homepageCanvas.SetActive(true);
    }

    // Function to quit the application
    public void QuitApplication()
    {
        // Quit the application
        Debug.Log("Quit button clicked!");
        Application.Quit();

        // This is for testing purposes in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
