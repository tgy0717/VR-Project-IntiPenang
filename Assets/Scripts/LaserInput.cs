using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class LaserInput : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose; // Pose component to track controller
    public SteamVR_Action_Boolean interactAction; // Action for interaction
    public SteamVR_Action_Vibration hapticAction; // Action for haptic feedback
    public GameObject Mainpage;
    public GameObject ImageMenu;
    public GameObject[] objSites;
    public GameObject topBar; // Reference to the top bar UI element

    // Reference to the UI elements
    public Canvas objectNameCanvas;  // Reference to the Canvas 
    public TextMeshProUGUI objectNameText;  // Reference to the Text component on the Canvas

    private GameObject lastHoveredObject = null; // Store the last hovered object to reset it
    private Vector3 originalScale; // Store original scale of the object

    private void Start()
    {
        // Ensure the pose component is assigned
        if (pose == null)
            pose = this.GetComponent<SteamVR_Behaviour_Pose>();

        if (pose == null)
            Debug.LogError("No SteamVR_Behaviour_Pose component found on this object", this);

        // Assign the interact action
        if (interactAction == null)
            interactAction = SteamVR_Input.GetBooleanAction("InteractUI");

        // Assign the haptic action (ensure this is set in the SteamVR Input window)
        if (hapticAction == null)
            hapticAction = SteamVR_Input.GetVibrationAction("HapticPulse");

        // Hide the canvas initially
        if (objectNameCanvas != null)
        {
            objectNameCanvas.gameObject.SetActive(false);
        }
         
    }

    private void Update()
    {
        // Cast a ray from the controller
        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (bHit)
        {
            GameObject hitObject = hit.transform.gameObject;

            // Handle hover logic
            if (hitObject.CompareTag("NextSite"))
            {
                if (lastHoveredObject != hitObject)
                {
                    // Reset the last hovered object if it's different from the current one
                    if (lastHoveredObject != null)
                    {
                        ResetObject(lastHoveredObject);
                    }

                    // Highlight the current object
                    HighlightObject(hitObject);

                    // Show the canvas with the object's name
                    ShowObjectNameCanvas(hitObject.name);

                    // Trigger haptic feedback when hovering over the object
                    TriggerHapticFeedback();

                    // Store the current object as the last hovered object
                    lastHoveredObject = hitObject;
                }
            }
            else
            {
                // Reset the last hovered object and hide the canvas if not hovering over a "NextSite"
                if (lastHoveredObject != null)
                {
                    ResetObject(lastHoveredObject);
                    lastHoveredObject = null;
                }

                // Hide the canvas when not hovering over any valid object
                HideObjectNameCanvas();
            }

            // Check if the interact button is pressed
            if (interactAction.GetStateUp(pose.inputSource))
            {
                Debug.Log("Controller clicked on: " + hit.transform.name);

                // Handle the hit object (e.g., button click, etc.)
                if (hitObject.CompareTag("Button"))
                {
                    Mainpage.SetActive(false);
                    ImageMenu.SetActive(true);
                    Debug.Log("Button clicked!");
                }
                else if (hitObject.CompareTag("NextSite"))
                {
                    Debug.Log("Next site clicked.");
                    int siteToLoad = hitObject.GetComponent<NewSites>().GetSiteToload();
                    Debug.Log("Loading site number: " + siteToLoad);
                    LoadSite(siteToLoad);
                }
            }
        }
        else
        {
            // If no object is hit, reset the last hovered object and hide the canvas
            if (lastHoveredObject != null)
            {
                ResetObject(lastHoveredObject);
                lastHoveredObject = null;
            }

            HideObjectNameCanvas();
        }
    }

    // Trigger haptic feedback on the controller
    void TriggerHapticFeedback()
    {
        // Trigger a simple haptic pulse on the controller
        hapticAction.Execute(0, 0.1f, 150f, 75f, pose.inputSource);
        // Parameters:
        // (startDelay, duration, frequency, amplitude, inputSource)
    }

    // Highlight the object (change color and increase size)
    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray;  // Change to highlight color
             
        }

        // Store the original scale before increasing the size
        originalScale = obj.transform.localScale;

        // Increase object size
        obj.transform.localScale *= 1.2f;  // Adjust scale multiplier as needed
    }

    // Reset the object to its original color and size
    void ResetObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;  // Reset to original color
        }

        // Reset object size to its original scale
        obj.transform.localScale = originalScale;
    }

    // Show the object name on the fixed-position canvas
    void ShowObjectNameCanvas(string objectName)
    {
        if (objectNameCanvas != null && objectNameText != null && ImageMenu == false)
        {
            objectNameCanvas.gameObject.SetActive(true); // Show the canvas
            objectNameText.text = objectName; // Set the text to the object's name
        }
        else if (objectNameCanvas != null && objectNameText != null )
        {
            objectNameCanvas.gameObject.SetActive(true); // Show the canvas
            objectNameText.text = "Go to " + objectName; // Set the text to the object's name
        }
    }

    // Hide the object name canvas
    void HideObjectNameCanvas()
    {
        if (objectNameCanvas != null)
        {
            objectNameCanvas.gameObject.SetActive(false); // Hide the canvas
        }
    }

    // Load a site by hiding other sites and showing the selected one
    public void LoadSite(int siteNumber)
    {
        // Hide all sites
        for (int i = 0; i < objSites.Length; i++)
        {
            objSites[i].SetActive(false);
        }
        // Show the selected site
        objSites[siteNumber].SetActive(true);
        // Hide the main menu
        ImageMenu.SetActive(false);
        // Show the top bar (if needed)
        //topBar.SetActive(true);
        // Enable camera movement or any other behavior
    }
}
