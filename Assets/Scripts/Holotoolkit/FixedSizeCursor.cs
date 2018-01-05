using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedSizeCursor : MonoBehaviour
{

    [Range(-1, 1)]
    float offsetVerticalDir;

    [SerializeField]
    float smooth;

    Transform camTransform;

    public float maxDistance;

    public float offsetZ;

    Camera cam;

    Vector3 refVelocity;

    float angleOfCamera;

    float startLocalScale;

    public static FixedSizeCursor Instance;

    void Start()
    {
        Instance = this;
        cam = Camera.main;
        camTransform = cam.transform;
        angleOfCamera = cam.fieldOfView;
        startLocalScale = transform.localScale.y;
    }

    void Update()
    {

        Vector3 targetPosition = GetTargetPosition();

        SetConstantScale(targetPosition);

        SmoothPosition(targetPosition);
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetPosition;

#if UNITY_EDITOR
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
#else
        Vector3 dir =  camTransform.forward + camTransform.up * offsetVerticalDir;
        dir.Normalize();
        Ray camRay = new Ray(camTransform.position, dir);
#endif
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, maxDistance))
        {
            targetPosition = hit.point;
        }
        else
        {
#if UNITY_EDITOR
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = offsetZ;
            targetPosition = cam.ScreenToWorldPoint(mousePos);
#else
            targetPosition = camTransform.position + camTransform.forward * maxDistance;
#endif
        }

        return targetPosition;
    }

    void SetConstantScale(Vector3 _targetPosition)
    {
        float Ld = Mathf.Tan(angleOfCamera / 2) * offsetZ;

        Vector3 relativeTargetPosition = camTransform.InverseTransformPoint(_targetPosition);

        float distanceRelativeTargetPosition = relativeTargetPosition.z;

        float Lx = Mathf.Tan(angleOfCamera / 2) * distanceRelativeTargetPosition;

        float newScale = (Lx * startLocalScale) / Ld;

        //Debug.Log("newScale:" + newScale);

        //Debug.Log("ratio1: " + Ld / Lx);
        //Debug.Log("ratio2: " + startLocalScale / newScale);

        transform.localScale = Vector3.one * newScale;
    }

    public void SmoothPosition(Vector3 _targetPosition)
    {
        //Passer en ref cam
        Vector3 camRefTargetPosition = camTransform.InverseTransformPoint(_targetPosition);
        Vector3 camRefPosition = camTransform.InverseTransformPoint(transform.position);

        //smoother x
        camRefPosition.x = Mathf.SmoothDamp(camRefPosition.x, camRefTargetPosition.x, ref refVelocity.x, smooth);

        //Smoother y
        camRefPosition.y = Mathf.SmoothDamp(camRefPosition.y, camRefTargetPosition.y, ref refVelocity.y, smooth);

        //Smoother z
        //camRefPosition.z = Mathf.SmoothDamp(camRefPosition.z, camRefTargetPosition.z, ref refVelocity.z, smooth);
        camRefPosition.z = camRefTargetPosition.z;

        //passer en world et appliquer la position
        transform.position = camTransform.TransformPoint(camRefPosition);
    }
}
