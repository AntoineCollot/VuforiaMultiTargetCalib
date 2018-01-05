using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOffset : MonoBehaviour {

    [SerializeField]
    string pointName;

    [HideInInspector]
    public Vector3 offset;

    [SerializeField]
    GameObject buttonHolder;

    public Vector3 Position
    {
        get
        {
            return transform.position + offset;
        }
    }

	// Use this for initialization
	void Start () {
        offset = Vector3.zero;
        offset.x = PlayerPrefs.GetFloat(pointName + "_x", 0);
        offset.y = PlayerPrefs.GetFloat(pointName + "_y", 0);
        offset.z = PlayerPrefs.GetFloat(pointName + "_z", 0);
    }

    private void OnEnable()
    {
        buttonHolder.SetActive(PlacementManager.Instance.hologramIsPlaced);
    }

    public void MoveRight(float amount)
    {
        Move(Vector3.right * amount);
    }

    public void MoveUp(float amount)
    {
        Move(Vector3.up * amount);
    }

    public void MoveForward(float amount)
    {
        Move(Vector3.forward * amount);
    }

    public void Move(Vector3 displacement)
    {
        offset += displacement;
        PlayerPrefs.SetFloat(pointName + "_x", offset.x);
        PlayerPrefs.SetFloat(pointName + "_y", offset.y);
        PlayerPrefs.SetFloat(pointName + "_z", offset.z);

        PlacementManager.Instance.UpdateTargetPosition();
    }
}
