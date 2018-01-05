using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonneTrackableEventHandler : DefaultTrackableEventHandler
{
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        BroadcastMessage("OnVuferiaTrackingFound", SendMessageOptions.DontRequireReceiver);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        BroadcastMessage("OnVuferiaTrackingLost", SendMessageOptions.DontRequireReceiver);
    }
}
