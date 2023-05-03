using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{

    public GameObject m_Prefabs;
    [SerializeField]
    GameObject visualObject;

    public GameObject placedPrefabs
    {
        get { return m_Prefabs; }
        set { m_Prefabs = value; }
    }
    UnityEvent placementUpdate;

    public GameObject spawnObject { get; private set; }
    ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null)
            placementUpdate = new UnityEvent();

        placementUpdate.AddListener(DisableVisual);
    }

    bool TryGetPos(out Vector2 touchPos)
    {
        if (Input.touchCount > 0 && GameControl.Instance.planeClick)
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }

        touchPos = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetPos(out Vector2 touchPos))
            return;

        if (m_RaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            Debug.Log("xay ra van de nay");

            if (spawnObject == null)
            {
                spawnObject = Instantiate(m_Prefabs, hitPose.position, hitPose.rotation);
                GameControl.Instance.planeClick = false;
                GameControl.Instance.onPlane = true;

            }
            else
            {
                spawnObject.transform.position = hitPose.position;
                GameControl.Instance.planeClick = false;
                GameControl.Instance.onPlane = true;

            }
            placementUpdate.Invoke();
        }
    
    
        
    }

    public void DisableVisual()
    {
        visualObject.SetActive(false);
    }
}
