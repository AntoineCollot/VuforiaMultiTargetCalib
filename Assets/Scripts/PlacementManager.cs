using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour {

    [SerializeField]
    Anchor target;

    [SerializeField]
    PointOffset[] points;

    int currentPointId = -1;

    public bool hologramIsPlaced = false;

    public static PlacementManager Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
    public void hidePoints()
    {
        foreach (PointOffset p in points)
        {
            p.gameObject.SetActive(false);
        }
    }

    public void SetPoint(Vector3 position, Quaternion rotation)
    {
        currentPointId++;

        if(currentPointId>points.Length)
        {
            return;
        }

        points[currentPointId].transform.SetPositionAndRotation(position, rotation);
        points[currentPointId].gameObject.SetActive(true);

        if(currentPointId == 2)
        {
            UpdateTargetPosition();
        }
    }

    public void UpdateTargetPosition()
    {
        Vector3 directionPoint = (points[1].Position + points[2].Position) * 0.5f;
        Vector3 direction = directionPoint - points[0].Position;

        target.PlaceAt(points[0].Position, points[0].Position + direction);
        target.gameObject.SetActive(true);

        hologramIsPlaced = true;
        hidePoints();
    }
}
