using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 300.0f;
    private float zoomSpeed = 600.0f;
    private float zoomAmount = 0;

    //tour manager
    private TourManager tourManager;

    void Start()
    {
        tourManager = FindObjectOfType<TourManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (tourManager.isCameraMove)
        {
            if (Input.GetMouseButton(0))
            {
                //rotate camera according to the mouse
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed, transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed, 0);
            }

            if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                zoomAmount = Mathf.Clamp(zoomAmount + Input.GetAxis("Mouse Y") * Time.deltaTime * zoomSpeed, -5.0f, 5.0f);
                Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
            }
        }
    }

    public void ResetCamera()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        zoomAmount = 0f;
        Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
    }
}
