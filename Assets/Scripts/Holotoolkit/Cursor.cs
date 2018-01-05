using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour {

    [SerializeField]
    float maxDistance;

    [SerializeField]
    float smooth;

    [SerializeField]
    LayerMask collisionLayer;

    Transform camTransform;
    Vector3 refVelocity;

    public static Cursor Instance;

	// Use this for initialization
	void Awake () {
        camTransform = Camera.main.transform;
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPos;

        RaycastHit hit;
#if UNITY_EDITOR
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
        Ray camRay = new Ray(camTransform.position, camTransform.forward );
#endif
        if (Physics.Raycast(camRay, out hit, maxDistance, collisionLayer))
        {
            if (hit.transform.tag == "Magnet")
            {
                MagnetEffect magnet = hit.transform.GetComponent<MagnetEffect>();
                targetPos = magnet.GetCenter();
            }
            else
            {
                targetPos = hit.point;
            }
        }
        else
        {
#if UNITY_EDITOR
            targetPos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(maxDistance);
#else
         targetPos = camTransform.position + (camTransform.forward).normalized * maxDistance;
#endif
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refVelocity, smooth);
    }
}
