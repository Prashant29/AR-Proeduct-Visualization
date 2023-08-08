using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneScanner : MonoBehaviour
{
    
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] private GameObject placementIndicator;

    ARPlaneManager arPlaneManager;

    private bool isObjectPlaced = false;

    public GameObject FindingPopup;
    public GameObject FoundPopup;

    private bool placementIsPoseValid;
    private Pose placementPose;

    public GameObject placedObject;

    public ObjectManager objectManager;
    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementIsPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isObjectPlaced)
        {
            PlaceItem();
        }

       

    }


   

    

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        if (arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            placementIsPoseValid = true;
            placementPose = hits[0].pose;
        }
        else
        {
            placementIsPoseValid = false;
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (isObjectPlaced)
        {
            return;
        }
        if (placementIsPoseValid)
        {
            placementIndicator.SetActive(true);
            FindingPopup.SetActive(false);
            FoundPopup.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
            FindingPopup.SetActive(true);
            FoundPopup.SetActive(false);
        }
    }
    [SerializeField] private ARAnchorManager arReferencePointManager;
    private void PlaceItem()
    {
        Debug.Log("Item Placed");
        if (!isObjectPlaced)
        {
            ARAnchor ahchorPoint = arReferencePointManager.AddAnchor(placementPose);
            if (ahchorPoint == null)
            {
                string errorEntry = "There was an error creating a reference point\n";
                Debug.LogError(errorEntry);
            }
            else
            {
                isObjectPlaced = true;
                objectManager.ObjectPlace = true;

                placedObject.transform.position = placementIndicator.transform.position;

                placementIndicator.SetActive(false);
                placedObject.SetActive(true);

                FindingPopup.SetActive(false);
                FoundPopup.SetActive(false);
                TrackingToggle();
            }

        }

    }

    

    bool planesAreVisible = true;

    public void TrackingToggle()
    {
        planesAreVisible = !planesAreVisible;
        arPlaneManager.SetTrackablesActive(planesAreVisible);
        arPlaneManager.planePrefab.SetActive(planesAreVisible);
    }

}
