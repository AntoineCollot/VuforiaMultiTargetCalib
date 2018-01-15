using UnityEngine;
using System.Collections;
using Vuforia;

public class VuforiaDetection : MonoBehaviour
{
    [SerializeField]
    float stableDeltaDistance = 0.1f;

    [SerializeField]
    float maxDeltaAngle = 10;

    Vector3 originalDirection;
    bool activated = false;
    bool completed = false;

    public static bool disable;

    private void Start()
    {
        originalDirection = transform.up;
        disable = false;
    }

    public void OnVuferiaTrackingFound()
    {
        if (activated || completed || disable)
            return;

        activated = true;
        StartCoroutine(FindStablePosition(1.5f));

        //CalibScreen.Instance.TrackingFound();
    }

    public void OnVuferiaTrackingLost()
    {
        if (disable)
            return;

        completed = false;
        activated = false;
        StopAllCoroutines();
    }

    [ContextMenu("Apply pos")]
    public void ApplyPosition()
    {
        ApplyPosition(transform.position);
    }

    void ApplyPosition(Vector3 pos)
    {
        PlacementManager.Instance.SetPoint(transform.position, transform.rotation);
        

		completed = true;
		
        //transform.parent.gameObject.SetActive(false);
    }

    IEnumerator FindStablePosition(float time)
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("Try " + i.ToString());

            Vector3 refPosition = transform.position;
            float t = 0;

            //CalibScreen.Instance.SetStabilizationProgression(i / 100f);

            while (true)
            {
                t += Time.deltaTime / time;

                yield return null;

                if (Vector3.Distance(refPosition, transform.position) > stableDeltaDistance)
                {
                    Debug.Log("Break distance : " + Vector3.Distance(refPosition, transform.position));
                    break;
                }
                if (t > 1)
                {

                    //if (IsRotated())
                    //{
                    //    Debug.Log("Break Angle");
                    //    break;
                    //}

                    Debug.Log("Apply pos");

                    //CalibScreen.Instance.StabilizationSuccesful();

                    ApplyPosition(transform.position);
                    yield break;
                }
            }
        }
    }

    bool IsRotated()
    {

        float diffAngle = Vector3.Angle(originalDirection, transform.up);
        if (diffAngle > 180)
        {
            diffAngle -= 360;
            diffAngle = Mathf.Abs(diffAngle);
        }


        if (diffAngle > maxDeltaAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
