using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GazeManagerSimple : MonoBehaviour
{

    Transform camTransform;

    [SerializeField]
    LayerMask collisionLayer;

    GameObject hoveredObject;
    Gazable[] gazables;

    public static GazeManagerSimple Instance;

    [HideInInspector]
    public bool lockObject = false;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockObject)
            return;

#if UNITY_EDITOR
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
        Ray camRay = new Ray(camTransform.position, camTransform.forward);
#endif
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, 10, collisionLayer))
        {
            if (hit.transform.gameObject != hoveredObject)
            {
                //Unselect the old object
                if (hoveredObject != null)
                {
                    foreach (Gazable g in gazables)
                    {
                        g.OffHover();
                    }
                }

                //Select the new object
                hoveredObject = hit.transform.gameObject;
                gazables = hoveredObject.GetComponents<Gazable>();

                foreach (Gazable g in gazables)
                {
                    g.OnHover();
                }
            }
        }

        else
        {
            if (hoveredObject != null)
            {
                foreach (Gazable g in gazables)
                {
                    g.OffHover();
                }
            }

            hoveredObject = null;
        }
    }

    private void OnDisable()
    {
        if (hoveredObject != null)
        {
            foreach (Gazable g in gazables)
            {
                g.OffHover();
            }
        }

        hoveredObject = null;
    }
}

public abstract class Gazable : MonoBehaviour
{
    [HideInInspector]
    public bool hovered;

    public virtual void OnHover()
    {
        hovered = true;
    }
    public virtual void OffHover()
    {
        hovered = false;
    }
}
