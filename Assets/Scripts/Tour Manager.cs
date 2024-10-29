using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TourManager : MonoBehaviour
{
    public GameObject[] objSites; // Array of site objects to display
    public GameObject canvasMainMenu; // Reference to the main menu canvas
    public GameObject topBar; // Reference to the top bar UI element
    public GameObject dropDownMenu; // Reference to the dropdown menu
    public GameObject sidebar; // Reference to the dropdown menu
    public bool isCameraMove = true; // Flag to control camera movement

    private GameObject lastHoveredObject = null;
    private GameObject currentHoveredUIElement = null;
    private Camera mainCamera;  // Main Camera reference

    public float zoomDuration = 0.2f;  // Duration for zoom effect
    public float zoomFOV = 3f;  // Target FOV during zoom
    private float originalFOV;  // Store original FOV


    // Reference to the HoverController script
    public HoverController hoverController;

    void Start()
    {
        Debug.Log("TourManager started.");

        // Ensure the top bar is hidden at the start
        topBar.SetActive(false);

        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
        }

        // Find HoverController component
        if (hoverController == null)
        {
            hoverController = FindObjectOfType<HoverController>();
        }
    }

    void Update()
    {
        // Check if pointer is over any UI element (including dropdown)
        if (IsPointerOverUIElement())
        {
            // If hovering over dropdown, disable camera movement
            if (EventSystem.current.currentSelectedGameObject == dropDownMenu)
            {
                Debug.Log("Pointer is over dropdown menu, disabling camera movement.");
                isCameraMove = false;
            }
            else
            {
                Debug.Log("Pointer is over other UI elements, camera movement disabled.");
                isCameraMove = false;
            }
        }
        else
        {
            // If not hovering over UI, enable camera movement
            isCameraMove = true;
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

                // Reset previously hovered object if it exists and is different
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

        // Handle hover over UI elements and adjust image size accordingly
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
            // If no UI element is hovered, reset the last hovered UI element
            if (currentHoveredUIElement != null)
            {
                hoverController.ResetImageSize(currentHoveredUIElement);
                hoverController.ResetHoverDesign(currentHoveredUIElement);
                currentHoveredUIElement = null;
            }
        }
    }

    // Check if the pointer is over any UI element
    bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    IEnumerator ZoomAndLoadSite(int siteNumber)
    {
        // Animate the camera zoom to the target FOV
        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(originalFOV, zoomFOV, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure FOV is exactly the zoomFOV after the loop
        mainCamera.fieldOfView = zoomFOV;

        // After zoom, load the new site
        LoadSite(siteNumber);

        // Reset camera FOV to the original
        elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(zoomFOV, originalFOV, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure FOV is exactly the original FOV after resetting
        mainCamera.fieldOfView = originalFOV;
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
            StartCoroutine(ZoomAndLoadSite(siteToLoad));
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
        dropDownMenu.SetActive(true);
        sidebar.SetActive(true);
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
        dropDownMenu.SetActive(false);
        sidebar.SetActive(false);
        // Disable camera movement
        isCameraMove = false;
        GetComponent<CameraController>().ResetCamera();
    }
}
