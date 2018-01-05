using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

/// <summary>
/// Get the gestures of the user
/// </summary>
public class GestureManager : MonoBehaviour
{
    public UnityEngine.XR.WSA.Input.GestureRecognizer recognizer { get; private set; }

    public bool IsManipulating { get; private set; }
    public bool IsNavigating { get; private set; }

    [HideInInspector]
    public Vector3 ManipulationPosition;
    [HideInInspector]
    public Vector3 NavigationPosition;

    public UnityEvent onAirTap = new UnityEvent();

    public UnityEvent onDragStarted = new UnityEvent();

    public UnityEvent onDragEnded = new UnityEvent();

    public static GestureManager Instance;

    public LayerMask airTapLayer;

    /// <summary>
    /// Initialization of recognizers
    /// </summary>
    void Awake()
    {
        Instance = this;

        //___________Action________________
        recognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        StartCapturingAirTap();
        recognizer.TappedEvent += TappedEvent;

        //Manipulation
        recognizer.ManipulationStartedEvent += ManipulationStartedEvent;
        recognizer.ManipulationUpdatedEvent += ManipulationUpdatedEvent;
        recognizer.ManipulationCompletedEvent += ManipulationEndedEvent;
        recognizer.ManipulationCanceledEvent += ManipulationEndedEvent;

        //Navigation
        recognizer.NavigationStartedEvent += NavigationStartedEvent;
        recognizer.NavigationUpdatedEvent += NavigationUpdatedEvent;
        recognizer.NavigationCompletedEvent += NavigationEndedEvent;
        recognizer.NavigationCanceledEvent += NavigationEndedEvent;

        recognizer.StartCapturingGestures();
    }

    void OnDestroy()
    {
        recognizer.TappedEvent -= TappedEvent;

        recognizer.ManipulationStartedEvent -= ManipulationStartedEvent;
        recognizer.ManipulationUpdatedEvent -= ManipulationUpdatedEvent;
        recognizer.ManipulationCompletedEvent -= ManipulationEndedEvent;
        recognizer.ManipulationCanceledEvent -= ManipulationEndedEvent;
        recognizer.NavigationUpdatedEvent -= NavigationUpdatedEvent;

        recognizer.NavigationStartedEvent -= NavigationStartedEvent;
        recognizer.NavigationUpdatedEvent -= NavigationUpdatedEvent;
        recognizer.NavigationCompletedEvent -= NavigationEndedEvent;
        recognizer.NavigationCanceledEvent -= NavigationEndedEvent;
    }

    public void StartCapturingManipulation()
    {
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);

#if UNITY_EDITOR
        recognisedGesture = GestureSettings.Tap | GestureSettings.ManipulationTranslate;
#endif
    }

    public void StartCapturingNavigation()
    {
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationX);

#if UNITY_EDITOR
        recognisedGesture = GestureSettings.Tap | GestureSettings.NavigationX;
#endif
    }

    public void StartCapturingAirTap()
    {
        recognizer.SetRecognizableGestures(GestureSettings.Tap);

#if UNITY_EDITOR
        recognisedGesture = GestureSettings.Tap;
#endif
    }

    #region Gesture Callbacks
    private void TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,7,airTapLayer))
        {
            hit.transform.SendMessage("OnAirTap");
        }
        else
            onAirTap.Invoke();
    }

    private void ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = true;

        ManipulationPosition = position;

        onDragStarted.Invoke();
    }

    private void ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = true;

        ManipulationPosition = position;
    }

    private void ManipulationEndedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;

        onDragEnded.Invoke();
    }

    private void NavigationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsNavigating = true;

        NavigationPosition = position;

        onDragStarted.Invoke();
    }

    private void NavigationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        NavigationPosition = position;
    }

    private void NavigationEndedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsNavigating = false;

        onDragEnded.Invoke();
    }
    #endregion

#if UNITY_EDITOR
    void Update()
    {
        KeyboardSimulation();
    }

    Vector3 mouseStartPos;
    GestureSettings recognisedGesture;

    void KeyboardSimulation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TappedEvent(InteractionSourceKind.Other, 1, Camera.main.ScreenPointToRay(Input.mousePosition));
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseStartPos = GetMouseWorldPos();

            switch(recognisedGesture)
            {
                case GestureSettings.Tap | GestureSettings.ManipulationTranslate:
                    ManipulationStartedEvent(InteractionSourceKind.Other, Vector3.zero, Camera.main.ScreenPointToRay(Input.mousePosition));
                    break;
                case GestureSettings.Tap | GestureSettings.NavigationX:
                    NavigationStartedEvent(InteractionSourceKind.Other, Vector3.zero, Camera.main.ScreenPointToRay(Input.mousePosition));
                    break;
                default:
                    break;
            }

        }
        else if (Input.GetMouseButtonUp(1))
        {
            Vector3 mouseDragVector = GetMouseWorldPos() - mouseStartPos;

            switch (recognisedGesture)
            {
                case GestureSettings.Tap | GestureSettings.ManipulationTranslate:
                    ManipulationEndedEvent(InteractionSourceKind.Other, mouseDragVector, Camera.main.ScreenPointToRay(Input.mousePosition));
                    break;
                case GestureSettings.Tap | GestureSettings.NavigationX:
                    Vector3 navPosition = Vector3.zero;
                    navPosition.x = (Camera.main.transform.InverseTransformPoint(GetMouseWorldPos()) - Camera.main.transform.InverseTransformPoint(mouseStartPos)).x;
                    navPosition.Normalize();
                    NavigationEndedEvent(InteractionSourceKind.Other, navPosition, Camera.main.ScreenPointToRay(Input.mousePosition));
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 mouseDragVector = GetMouseWorldPos() - mouseStartPos;

            switch (recognisedGesture)
            {
                case GestureSettings.Tap | GestureSettings.ManipulationTranslate:
                    ManipulationUpdatedEvent(InteractionSourceKind.Other, mouseDragVector, Camera.main.ScreenPointToRay(Input.mousePosition));
                    break;
                case GestureSettings.Tap | GestureSettings.NavigationX:
                    Vector3 navPosition = Vector3.zero;
                    navPosition.x = (Camera.main.transform.InverseTransformPoint(GetMouseWorldPos()) - Camera.main.transform.InverseTransformPoint(mouseStartPos)).x;
                    navPosition.Normalize();
                    NavigationUpdatedEvent(InteractionSourceKind.Other, navPosition, Camera.main.ScreenPointToRay(Input.mousePosition));
                    break;
                default:
                    break;
            }
        }
     
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0.4f;
        return Camera.main.ScreenToWorldPoint(pos);
    }
#endif
}