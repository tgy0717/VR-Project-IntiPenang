using System.Collections.Generic;
using UnityEngine;

public class Sidebar : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed of auto-rotation
    private bool isAutoRotateEnabled = false; // Tracks if auto-rotate is active
    private bool isUIVisible = true; // Tracks the visibility state
    public GameObject topBar; // Reference to the top bar UI element
    public GameObject dropDownMenu; // Reference to the dropdown menu
    public GameObject bottomBar; // Reference to the dropdown menu

    private bool isNextSiteHidden = false; // Tracks the visibility state of "NextSite" objects
    private List<GameObject> nextSiteObjects = new List<GameObject>(); // List to store active "NextSite" objects

    void Update()
    {
        // Check if auto-rotate is enabled and no mouse input is detected
        if (isAutoRotateEnabled && !Input.GetMouseButton(0))
        {
            // Rotate the camera to the right
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    // Method to toggle auto-rotate
    public void ToggleAutoRotate()
    {
        isAutoRotateEnabled = !isAutoRotateEnabled;
    }

    public void ToggleUI()
    {
        // Toggle visibility state
        isUIVisible = !isUIVisible;
        topBar.SetActive(isUIVisible);
        dropDownMenu.SetActive(isUIVisible);
        bottomBar.SetActive(isUIVisible);
    }

    public void ToggleTag()
    {
        if (!isNextSiteHidden)
        {
            // Find all active objects with the tag "NextSite" and store them in the list
            nextSiteObjects.Clear(); // Clear previous list
            GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("NextSite");
            foreach (GameObject obj in foundObjects)
            {
                if (obj.activeSelf)
                {
                    nextSiteObjects.Add(obj);
                    obj.SetActive(false); // Hide the object
                }
            }
        }
        else
        {
            // Unhide all stored objects
            foreach (GameObject obj in nextSiteObjects)
            {
                obj.SetActive(true); // Unhide the object
            }
            nextSiteObjects.Clear(); // Clear the list after unhiding
        }

        // Update the visibility state tracker
        isNextSiteHidden = !isNextSiteHidden;
    }

    // Function to reset everything to the initial state
    public void ResetAll()
    {
        // Reset auto-rotate to off
        isAutoRotateEnabled = false;

        // Reset UI visibility to true
        isUIVisible = true;
        topBar.SetActive(true);
        dropDownMenu.SetActive(true);
        bottomBar.SetActive(true);

        // Reset "NextSite" objects visibility to visible
        if (isNextSiteHidden)
        {
            foreach (GameObject obj in nextSiteObjects)
            {
                obj.SetActive(true); // Unhide the object
            }
            nextSiteObjects.Clear(); // Clear the list after unhiding
            isNextSiteHidden = false;
        }
    }
}
