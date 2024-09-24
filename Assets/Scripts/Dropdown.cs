using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // For handling UI elements like Dropdowns

public class DropdownMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDownMenu; // Reference to the Dropdown UI
    [SerializeField] private GameObject[] objSites; // Array of GameObjects for the different sites 
     
    public int levelNumber; // Manually set the level number (e.g., 2 or 6)

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to the dropdown menu to call HandleInputData when dropdown value changes
        dropDownMenu.onValueChanged.AddListener(delegate { HandleInputData(dropDownMenu.value); });
    }

    // Method to handle dropdown option changes
    void HandleInputData(int val)
    {
        if (val == 0)
        {
            // Placeholder selected, do nothing or reset the scene
            Debug.Log("Placeholder selected. No site will be loaded.");
            return;
        }
        int siteIndex = val - 1;
        // You can check the value and decide which site to load based on levelNumber
        if (levelNumber == 2)
        {
            // Handle Level 2 dropdown options
            LoadSite(siteIndex); // val corresponds to dropdown option selected
        }
        else if (levelNumber == 6)
        {
            // Handle Level 6 dropdown options
            LoadSite(siteIndex + 32); // Assuming Level 6 sites start from 30
        } 
    }

    // Method to load a specific site
    public void LoadSite(int siteNumber)
    {
        // Hide all sites first
        for (int i = 0; i < objSites.Length; i++)
        {
            objSites[i].SetActive(false);
        }

        // Show the selected site
        if (siteNumber < objSites.Length)
        {
            objSites[siteNumber].SetActive(true);
        }
         
        dropDownMenu.gameObject.SetActive(true);

        dropDownMenu.value = 0;

    }
}
