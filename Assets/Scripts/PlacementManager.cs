using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour {

    [SerializeField]
    Anchor target;

    [SerializeField]
    Anchor[] points;

    [SerializeField]
    float minPointDistance = 0.2f;

    int currentPointId = -1;

    bool pointsAreVisible;

    [HideInInspector]
    public bool hologramIsPlaced = false;

    [Header("Audio")]

    new AudioSource audio;

    public static PlacementManager Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
        audio=GetComponent<AudioSource>();
    }
	
    public void ShowPoints(bool value)
    {
        foreach (Anchor p in points)
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

        points[currentPointId].PlaceAt(position, rotation);
        points[currentPointId].gameObject.SetActive(true);

        if(currentPointId == 1)
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
        audio.Play();

        UpdateTargetPosition();
    }

    public void UpdateTargetPosition()
    {
        Vector3 direction = points[1].transform.position - points[0].transform.position;

        target.PlaceAt(points[0].transform.position, points[0].transform.position + direction);
    }
}
