using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoloButton : Gazable {

    public UnityEvent onClick = new UnityEvent();

    [SerializeField]
    float lerpTime;

    [SerializeField]
    Color onHoverColor;

    [SerializeField]
    Color offHoverColor;

    Material mat;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = offHoverColor;
    }

    public void OnAirTap()
    {
        onClick.Invoke();
    }

    public override void OnHover()
    {
        base.OnHover();

        StopAllCoroutines();
        StartCoroutine(LerpColor(onHoverColor));
    }

    public override void OffHover()
    {
        base.OffHover();

        StopAllCoroutines();
        StartCoroutine(LerpColor(offHoverColor));
    }

    IEnumerator LerpColor(Color targetColor)
    {
        float t = 0;
        Color startColor = mat.color;

        while (t < 1)
        {
            t += Time.deltaTime / lerpTime;

            mat.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }
    }
}

