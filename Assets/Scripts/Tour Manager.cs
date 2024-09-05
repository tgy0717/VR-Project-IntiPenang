using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    public GameObject[] objSites; // Array of site objects to display
    public GameObject canvasMainMenu; // Reference to the main menu canvas
    public GameObject topBar; // Reference to the top bar UI element

    public bool isCameraMove = false; // Flag to control camera movement

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TourManager started.");

        // Ensure the top bar is hidden at the start
        topBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCameraMove)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Mouse button pressed in InputTest.");
                ReturnToMenu();
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.Log("Mouse button pressed.");

                if (Physics.Raycast(ray, out hit, 100.0f)) 
                {
                    if (hit.transform.gameObject.tag == "NextSite")
                    {
                        Debug.Log("Next site clicked.");
                        int siteToLoad = hit.transform.gameObject.GetComponent<NewSites>().GetSiteToload();
                        Debug.Log("Loading site number: " + siteToLoad);
                        LoadSite(siteToLoad);
                    }
                }
            }



        }
        
        
    }

    public void LoadSite(int siteNumber)
    {
        //hide site
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
        Debug.Log("isCameraMove set to true.");
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
    }
}
