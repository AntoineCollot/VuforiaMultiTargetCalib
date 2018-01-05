using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeUserDetector : MonoBehaviour {

    [Header("Thresholds")]

    [SerializeField]
    float horizontalRotationTreshold = 50;

    [SerializeField]
    float verticalRotationTreshold = 80;

    [Space(20)]

    public UnityEvent onChangeUser = new UnityEvent();

    Transform camT;

    void Start () {
        camT = Camera.main.transform;
    }

	void Update () {
        if (Time.time < 3)
            return;

        //Get the rotation on the local z axis (Left/Right)
        float horizontalRotation = Mathf.Abs(Mathf.Asin(camT.right.y) * Mathf.Rad2Deg);

        //Get the rotation on the local x axis (forward/backward)
        float verticalRotation = Mathf.Abs(Mathf.Asin(camT.forward.y) * Mathf.Rad2Deg);

        //Check if any of them is greater than the threshold
        if (horizontalRotation>horizontalRotationTreshold || verticalRotation>verticalRotationTreshold)
        {
            //If it's the case, it means that the user changed
            onChangeUser.Invoke();
        }
	}
}
