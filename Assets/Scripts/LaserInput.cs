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
    public GameObject hoverCaptionUI; // Reference to the hover caption UI element (always in view)

    private TextMeshProUGUI hoverCaptionText; // Text component for displaying hover text
    private GameObject lastHoveredObject = null; // Store the last hovered object to reset it
    private Vector3 originalScale; // Store original scale of the object

    private void Start()
    {
        if (pose == null)
            pose = this.GetComponent<SteamVR_Behaviour_Pose>();

        if (pose == null)
            Debug.LogError("No SteamVR_Behaviour_Pose component found on this object", this);

        if (interactAction == null)
            interactAction = SteamVR_Input.GetBooleanAction("InteractUI");

        if (hapticAction == null)
            hapticAction = SteamVR_Input.GetVibrationAction("HapticPulse");

        if (hoverCaptionUI != null)
        {
            hoverCaptionText = hoverCaptionUI.GetComponentInChildren<TextMeshProUGUI>();
            hoverCaptionUI.SetActive(false); // Hide initially
        }
    }

    private void Update()
    {
        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (bHit)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.CompareTag("NextSite"))
            {
                if (lastHoveredObject != hitObject)
                {
                    ResetLastHoveredObject();
                    HighlightObject(hitObject);
                    TriggerHapticFeedback();
                    lastHoveredObject = hitObject;
                }
                if (!ImageMenu.activeSelf){
                    ShowHoverCaption(hitObject);
                }
            }
            else if (hitObject.CompareTag("ImageMenuButton") || hitObject.CompareTag("SettingButton") || hitObject.CompareTag("VRGuideButton") || hitObject.CompareTag("QuitButton"))
            {
                if (lastHoveredObject != hitObject)
                {
                    ResetLastHoveredObject();
                    HighlightObject(hitObject);
                    TriggerHapticFeedback();
                    lastHoveredObject = hitObject;
                    HideHoverCaption();
                }
            }
            else
            {
                ResetLastHoveredObject();
                HideHoverCaption();
            }

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
                else if (hitObject.CompareTag("QuitButton"))
                {
                    Application.Quit();
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
                    #endif
                }
            }
        }
        else
        {
            ResetLastHoveredObject();
            HideHoverCaption();
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

    void ShowHoverCaption(GameObject targetObject)
    {
        if (hoverCaptionText != null)
        {
            string objectName = targetObject.name;
            hoverCaptionText.text = objectName;
            hoverCaptionUI.SetActive(true);
        }
    }

    void HideHoverCaption()
    {
        if (hoverCaptionUI != null)
        {
            hoverCaptionUI.SetActive(false);
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
