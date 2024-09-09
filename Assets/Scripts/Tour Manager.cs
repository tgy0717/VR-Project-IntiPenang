using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    public GameObject[] objSites; // Array of site objects to display
    public GameObject canvasMainMenu; // Reference to the main menu canvas
    public GameObject topBar; // Reference to the top bar UI element
    public bool isCameraMove = false; // Flag to control camera movement

    private GameObject lastHoveredObject = null;
    private GameObject currentHoveredUIElement = null;

    // Reference to the HoverController script
    public HoverController hoverController;

    void Start()
    {
        Debug.Log("TourManager started.");

        // Ensure the top bar is hidden at the start
        topBar.SetActive(false);

        // Find HoverController component
        if (hoverController == null)
        {
            hoverController = FindObjectOfType<HoverController>();
        }
    }

    void Update()
    {
        if (isCameraMove)
        {
            // Check for escape key press
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToMenu();
            }

            // Check for hovering over UI elements or 3D objects
            
        }
        if (hoverController.IsPointerOverUIElement(out GameObject uiElement))
        {
            // Handle hover over UI element with "NextSite" tag
            if (uiElement.CompareTag("NextSite"))
            {
                hoverController.AdjustImageSize(uiElement, true);  // Make image bigger when hovering
                hoverController.ApplyHoverDesign(uiElement);  // Apply hover design

                // Reset the previous UI element if it's different from the current one
                if (currentHoveredUIElement != null && currentHoveredUIElement != uiElement)
                {
                    hoverController.ResetImageSize(currentHoveredUIElement);
                    hoverController.ResetHoverDesign(currentHoveredUIElement);
                }

                // Track the current hovered UI element
                currentHoveredUIElement = uiElement;
            }
        }
        else
        {
            // If nothing is hovered, reset the last hovered UI element immediately
            if (currentHoveredUIElement != null)
            {
                hoverController.ResetImageSize(currentHoveredUIElement);
                hoverController.ResetHoverDesign(currentHoveredUIElement);
                currentHoveredUIElement = null;
            }

            // Handle non-UI element raycasts (like 3D objects)
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                GameObject currentHoveredObject = hit.transform.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    HandleUIClick(currentHoveredObject);
                }

                if (currentHoveredObject.CompareTag("NextSite"))
                {
                    hoverController.DisplayHoverText(currentHoveredObject.name);
                    HighlightObject(currentHoveredObject);

                    if (lastHoveredObject != null && lastHoveredObject != currentHoveredObject)
                    {
                        ResetObject(lastHoveredObject);
                    }

                    lastHoveredObject = currentHoveredObject;
                }
            }
            else
            {
                // If nothing is hovered, reset the last hovered 3D object and UI element
                if (lastHoveredObject != null)
                {
                    ResetObject(lastHoveredObject);
                    lastHoveredObject = null;
                    hoverController.ClearHoverText();
                }
            }
        }
    }

    // Handle UI element click
    void HandleUIClick(GameObject uiElement)
    {
        Debug.Log("Clicked on UI element: " + uiElement.name);

        // Example: GetSiteToload from a script on the UI element
        NewSites newSiteScript = uiElement.GetComponent<NewSites>();
        if (newSiteScript != null)
        {
            int siteToLoad = newSiteScript.GetSiteToload();
            LoadSite(siteToLoad);
        }
    }

    // Highlight a 3D object
    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        { 
            renderer.material.color = UnityEngine.Color.gray;  // Change to a highlight color
        }
    }

    // Reset a 3D object
    void ResetObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = UnityEngine.Color.white;  // Reset to the original color
        } 
    }

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
        canvasMainMenu.SetActive(false);
        // Show the top bar
        topBar.SetActive(true);
        // Enable camera movement
        isCameraMove = true;
        GetComponent<CameraController>().ResetCamera();
    }

    public void ReturnToMenu()
    {
        // Show the main menu
        canvasMainMenu.SetActive(true);
        // Hide all sites
        for (int i = 0; i < objSites.Length; i++)
        {
            objSites[i].SetActive(false);
        }
        // Hide the top bar
        topBar.SetActive(false);
        // Disable camera movement
        isCameraMove = false;
        GetComponent<CameraController>().ResetCamera();
    }
}
