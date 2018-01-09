using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour {

    [SerializeField]
    Anchor target;

    [SerializeField]
    PointOffset[] points;

    [SerializeField]
    float minPointDistance = 0.2f;

    int currentPointId = -1;

    bool pointsAreVisible;

    [HideInInspector]
    public bool hologramIsPlaced = false;

    public static PlacementManager Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
    public void ShowPoints(bool value)
    {
        foreach (PointOffset p in points)
        {
            p.gameObject.SetActive(value);
        }

        pointsAreVisible = value;
    }

    public void ShowPoints()
    {
        ShowPoints(!pointsAreVisible);
    }

    public void SetPoint(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i <= currentPointId; i++)
        {
            if (Vector3.Distance(points[i].transform.position, position) < minPointDistance)
            {
                print("Points too close");
                return;
            }


        }

        currentPointId++;

        if(currentPointId>points.Length)
        {
            return;
        }

        points[currentPointId].transform.SetPositionAndRotation(position, rotation);
        points[currentPointId].gameObject.SetActive(true);

        if(currentPointId == 2)
        {
            SetTargetPosition();
        }
    }

    public void SetTargetPosition()
    {
        hologramIsPlaced = true;
        ShowPoints(false);
        GestureManager.Instance.StartCapturingAirTap();
        target.gameObject.SetActive(true);

        UpdateTargetPosition();
    }

    public void UpdateTargetPosition()
    {
        Vector3 directionPoint = (points[1].Position + points[2].Position) * 0.5f;
        Vector3 direction = directionPoint - points[0].Position;

        target.PlaceAt(points[0].Position, points[0].Position + direction);
    }
}
