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
    public GameObject SettingCanvas; // Reference to the Setting canvas
    public GameObject VRGuideCanvas; // Reference to the VR Guide canvas
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
                    ResetLastHoveredObject();
                    HighlightObject(hitObject);
                    ShowObjectNameCanvas(hitObject.name);
                    TriggerHapticFeedback();
                    lastHoveredObject = hitObject;
                }
            }
            else if (hitObject.CompareTag("ImageMenuButton"))
            {
                if (lastHoveredObject != hitObject)
                {
                    ResetLastHoveredObject();
                    HighlightObject(hitObject);
                    ShowObjectNameCanvas("ImageMenu");
                    TriggerHapticFeedback();
                    lastHoveredObject = hitObject;
                }
            }
            else if (hitObject.CompareTag("SettingButton"))
            {
                if (lastHoveredObject != hitObject)
                {
                    ResetLastHoveredObject();
                    HighlightObject(hitObject);
                    ShowObjectNameCanvas("Settings");
                    TriggerHapticFeedback();
                    lastHoveredObject = hitObject;
                }
            }
            else if (hitObject.CompareTag("VRGuideButton"))
            {
                if (lastHoveredObject != hitObject)
                {
                    ResetLastHoveredObject();
                    HighlightObject(hitObject);
                    ShowObjectNameCanvas("VR Guide");
                    TriggerHapticFeedback();
                    lastHoveredObject = hitObject;
                }
            }
            else
            {
                ResetLastHoveredObject();
                HideObjectNameCanvas();
            }

            // Check if the interact button is pressed
            if (interactAction.GetStateUp(pose.inputSource))
            {
                Debug.Log("Controller clicked on: " + hit.transform.name);

                if (hitObject.CompareTag("ImageMenuButton"))
                {
                    Mainpage.SetActive(false);
                    ImageMenu.SetActive(true);
                    Debug.Log("Button clicked!");
                }
                else if (hitObject.CompareTag("NextSite"))
                {
                    int siteToLoad = hitObject.GetComponent<NewSites>().GetSiteToload();
                    LoadSite(siteToLoad);
                }
                else if (hitObject.CompareTag("SettingButton"))
                {
                    ShowSettingCanvas();
                }
                else if (hitObject.CompareTag("VRGuideButton"))
                {
                    ShowVRGuideCanvas();
                }
            }
        }
        else
        {
            ResetLastHoveredObject();
            HideObjectNameCanvas();
        }
    }

    void ResetLastHoveredObject()
    {
        if (lastHoveredObject != null)
        {
            ResetObject(lastHoveredObject);
            lastHoveredObject = null;
        }
    }

    void TriggerHapticFeedback()
    {
        hapticAction.Execute(0, 0.1f, 150f, 75f, pose.inputSource);
    }

    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray;  // Change to highlight color
        }

        originalScale = obj.transform.localScale;
        obj.transform.localScale *= 1.2f;
    }

    void ResetObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;  // Reset to original color
        }

        obj.transform.localScale = originalScale;
    }

    void ShowObjectNameCanvas(string objectName)
    {
        if (objectNameCanvas != null && objectNameText != null)
        {
            objectNameCanvas.gameObject.SetActive(true);
            objectNameText.text = objectName;
        }
    }

    void HideObjectNameCanvas()
    {
        if (objectNameCanvas != null)
        {
            objectNameCanvas.gameObject.SetActive(false);
        }
    }

    public void LoadSite(int siteNumber)
    {
        foreach (var site in objSites)
        {
            site.SetActive(false);
        }
        objSites[siteNumber].SetActive(true);
        ImageMenu.SetActive(false);
    }

    void ShowSettingCanvas()
    {
        Mainpage.SetActive(false);
        ImageMenu.SetActive(false);
        VRGuideCanvas.SetActive(false);
        SettingCanvas.SetActive(true);
    }

    void ShowVRGuideCanvas()
    {
        Mainpage.SetActive(false);
        ImageMenu.SetActive(false);
        SettingCanvas.SetActive(false);
        VRGuideCanvas.SetActive(true);
    }
}
