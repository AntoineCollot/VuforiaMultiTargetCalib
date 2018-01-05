using UnityEngine;
using System.Collections;



public class Anchor : MonoBehaviour
{

    public string ObjectAnchorStoreName;

    UnityEngine.XR.WSA.Persistence.WorldAnchorStore anchorStore;

    bool Placing = false;
    // Use this for initialization
    void Start()
    {
        Debug.Log("WorldAnchorStore.GetAsync()");
        UnityEngine.XR.WSA.Persistence.WorldAnchorStore.GetAsync(AnchorStoreReady);
        gameObject.SetActive(false);
    }

    void AnchorStoreReady(UnityEngine.XR.WSA.Persistence.WorldAnchorStore store)
    {
        anchorStore = store;

        //if (PlacementBypasser.Instance != null)
        //{
        //    CreateAnchor();
        //}
        CreateAnchor();

    }

    public void PlaceAt(Vector3 position)
    {
        StartPlacing();

        transform.position = position;

        StopPlacing();
    }


    public void PlaceAt(Vector3 position, Vector3 lookAt)
    {
        StartPlacing();

        transform.position = position;
        transform.LookAt(lookAt);

        StopPlacing();
    }

    public void StartPlacing()
    {
        if (anchorStore == null)
        {
            return;
        }

        UnityEngine.XR.WSA.WorldAnchor anchor = gameObject.GetComponent<UnityEngine.XR.WSA.WorldAnchor>();
        if (anchor != null)
        {
            DestroyImmediate(anchor);
        }

        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            Debug.Log(ids[index]);
            if (ids[index] == ObjectAnchorStoreName)
            {
                bool deleted = anchorStore.Delete(ids[index]);
                Debug.Log("deleted: " + deleted);
                break;
            }
        }
    }

    public void StopPlacing()
    {
        if (anchorStore == null)
        {
            return;
        }

        UnityEngine.XR.WSA.WorldAnchor attachingAnchor = gameObject.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
        if (attachingAnchor.isLocated)
        {
            Debug.Log("Saving persisted position immediately");
            bool saved = anchorStore.Save(ObjectAnchorStoreName, attachingAnchor);
            Debug.Log("saved: " + saved);
        }
        else
        {
            attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
        }
    }

    private void AttachingAnchor_OnTrackingChanged(UnityEngine.XR.WSA.WorldAnchor self, bool located)
    {
        if (located)
        {
            Debug.Log("Saving persisted position in callback");
            bool saved = anchorStore.Save(ObjectAnchorStoreName, self);
            Debug.Log("saved: " + saved);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }

    public void CreateAnchor()
    {
        Debug.Log("looking for " + ObjectAnchorStoreName);
        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            Debug.Log(ids[index]);
            if (ids[index] == ObjectAnchorStoreName)
            {
                UnityEngine.XR.WSA.WorldAnchor wa = anchorStore.Load(ids[index], gameObject);
                Placing = false;
                break;
            }
        }
    }
}
