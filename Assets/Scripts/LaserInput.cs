using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class LaserInput : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose; // Pose component to track controller
    public SteamVR_Action_Boolean interactAction; // Action for interaction
    public GameObject Mainpage;
    public GameObject ImageMenu;
    public GameObject[] objSites;
    public GameObject topBar; // Reference to the top bar UI element

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
    }

    private void Update()
    {
        // Cast a ray from the controller
        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (bHit)
        {
            // Check if the interact button is pressed
            if (interactAction.GetStateUp(pose.inputSource))
            {
                Debug.Log("Controller clicked on: " + hit.transform.name);

                // Handle the hit object (e.g., button click, etc.)
                if (hit.transform.tag == "Button")
                {
                    Mainpage.SetActive(false);
                    ImageMenu.SetActive(true);
                    Debug.Log("Button clicked!");
                }
                else if (hit.transform.gameObject.tag == "NextSite") 
                { 
                    Debug.Log("Next site clicked.");
                    int siteToLoad = hit.transform.gameObject.GetComponent<NewSites>().GetSiteToload();
                    Debug.Log("Loading site number: " + siteToLoad);
                    LoadSite(siteToLoad);
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
        ImageMenu.SetActive(false);
        // Show the top bar
        //topBar.SetActive(true);
        // Enable camera movement 
    }
}
