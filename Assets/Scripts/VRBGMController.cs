using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class VRVolumeController : MonoBehaviour
{
    public SteamVR_Action_Vector2 scrollAction;  // The action for scrolling (e.g., thumbstick or touchpad input)
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.LeftHand;  // Set to right hand by default

    public Slider volumeSlider; // Reference to the volume slider
    public GameObject settingsPage; // Reference to the settings page UI
    public float volumeAdjustSpeed = 0.01f; // Sensitivity of volume adjustment with scroll input

    void Start()
    {
        // Activate the scroll action if necessary
        SteamVR_Actions.platformer.Activate();

        // Check if volumeSlider and scrollAction are assigned
        if (volumeSlider == null)
        {
            Debug.LogError("Volume slider is not assigned in VRVolumeController script.");
        }
        if (scrollAction == null)
        {
            Debug.LogError("Scroll action is not assigned in VRVolumeController script.");
        }
    }

    void Update()
    {
        // Check if the settings page is active to allow volume control
        if (settingsPage.activeSelf)
        {
            AdjustVolumeWithScrollInput();
        }
    }

    private void AdjustVolumeWithScrollInput()
    {
        // Get the vector2 input from the controller's thumbstick or touchpad
        Vector2 scrollInput = scrollAction.GetAxis(inputSource);

        // Use the Y-axis of the scroll input to adjust the volume slider
        if (scrollInput.y != 0)  // Only adjust if there is a scroll input on the Y-axis
        {
            volumeSlider.value = Mathf.Clamp(volumeSlider.value + scrollInput.y * volumeAdjustSpeed, 0f, 1f);
        }
    }
}