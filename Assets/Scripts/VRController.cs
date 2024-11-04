using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class VRController : MonoBehaviour
{
    public ScrollRect scrollRect;  // Reference to the ScrollRect component
    public SteamVR_Action_Vector2 scrollAction;  // The action for scrolling (e.g., thumbstick input)
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.RightHand;  // Set to right hand by default

    public GameObject imageMenuCanvas;  // Reference to the ImageMenu canvas
    public GameObject mainPageCanvas;   // Reference to the MainPage canvas
    public GameObject settingsCanvas;   // Reference to the Settings canvas
    public GameObject vrGuideCanvas;    // Reference to the VR Guide canvas
    public GameObject[] objSites;       // Array of site objects
    public SteamVR_Action_Boolean backAction;  // The action for the "Back" button (e.g., trigger press)

    // Speed of scrolling
    public float scrollSpeed = 1f;

    void Update()
    {
        // Activate SteamVR actions if necessary
        SteamVR_Actions.platformer.Activate();

        // Get the vector2 input from the controller (for scrolling)
        Vector2 scrollInput = scrollAction.GetAxis(inputSource);

        // Scroll the content if there's input
        if (scrollInput.y != 0 && imageMenuCanvas.activeSelf)
        {
            // Adjust the vertical scroll position based on the input
            scrollRect.verticalNormalizedPosition += scrollInput.y * scrollSpeed * Time.deltaTime;

            // Clamp the scroll position between 0 and 1
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0, 1);
        }

        // Check if the back button is pressed
        if (backAction.GetStateDown(inputSource))
        {
            HandleBackButtonPress();
        }
    }

    // Handles the logic when the back button is pressed
    private void HandleBackButtonPress()
    {
        // Priority for closing modals (Settings and VR Guide) before other canvases
        if (settingsCanvas.activeSelf)
        {
            // If Settings is active, close it and go back to the previous canvas (ImageMenu or MainPage)
            settingsCanvas.SetActive(false);
            mainPageCanvas.SetActive(true);
        }
        else if (vrGuideCanvas.activeSelf)
        {
            // If VR Guide is active, close it and go back to the previous canvas
            vrGuideCanvas.SetActive(false);
            mainPageCanvas.SetActive(true);
        }
        else if (imageMenuCanvas.activeSelf)
        {
            // If ImageMenu is active, deactivate it and activate MainPage
            imageMenuCanvas.SetActive(false);
            mainPageCanvas.SetActive(true);
        }
        else if (!imageMenuCanvas.activeSelf && !mainPageCanvas.activeSelf)
        {
            // If neither ImageMenu nor MainPage are active, activate ImageMenu
            foreach (GameObject site in objSites)
            {
                site.SetActive(false);
            }
            imageMenuCanvas.SetActive(true);
        }
    }

    // Helper function to determine which canvas to go back to
    private void ReturnToPreviousCanvas()
    {
        // If both ImageMenu and MainPage are inactive, activate ImageMenu by default
        if (!imageMenuCanvas.activeSelf && !mainPageCanvas.activeSelf)
        {
            imageMenuCanvas.SetActive(true);
        }
    }

    // Additional methods for opening Settings and VR Guide
    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);
        imageMenuCanvas.SetActive(false);
        mainPageCanvas.SetActive(false);
    }

    public void OpenVRGuide()
    {
        vrGuideCanvas.SetActive(true);
        imageMenuCanvas.SetActive(false);
        mainPageCanvas.SetActive(false);
    }
}
