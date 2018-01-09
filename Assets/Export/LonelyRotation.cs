using UnityEngine;
using System.Collections;

public class LonelyRotation : MonoBehaviour {

    public float speed;
    public bool rotate;
    public bool halfRotate;
    public bool specRotate;
    public Vector3 Axis;
    Quaternion temp;
    Quaternion init;

    void Start()
    {
        init = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
            transform.Rotate(Axis * speed * Time.deltaTime);

        if (halfRotate && !specRotate)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Mathf.Sin(Time.time*speed), transform.eulerAngles.z);
        }
        else if (halfRotate && specRotate)
        {
            StartCoroutine(SpecialRotate());
        }
    }

    IEnumerator SpecialRotate()
    {
        Quaternion targetRot1 = Quaternion.Euler(transform.localEulerAngles - Vector3.up * 90);
        Quaternion targetRot2 = Quaternion.Euler(transform.localEulerAngles + Vector3.up * 45);

        float t2 = 0.33f;

        while (t2 < 1)
        {
            t2 += Time.deltaTime / 8;
            transform.localRotation = Quaternion.Slerp(targetRot2, targetRot1, t2);
            yield return null;
        }

        while (halfRotate)
        {
            float t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime / 8;
                transform.localRotation = Quaternion.Slerp(targetRot1, targetRot2, t);
                yield return null;
            }
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / 8;
                transform.localRotation = Quaternion.Slerp(targetRot2, targetRot1, t);
                yield return null;
            }
            yield return null;
        }

        //Quaternion temprot = Quaternion.Euler(new Vector3(0, 90, 0) + Camera.main.transform.eulerAngles);
        //while (halfRotate)
        //{
        //    float t = 0;

        //    while (t < 1)
        //    {
        //        t += Time.deltaTime/8;
        //        transform.rotation = Quaternion.Lerp(temprot, Quaternion.Euler(transform.eulerAngles.x, temprot.y-100, transform.eulerAngles.z), t);
        //        yield return null;
        //    }
        //    t = 0;
        //    while (t<1)
        //    {
        //        t += Time.deltaTime/8;
        //        transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles.x, temprot.y - 100, transform.eulerAngles.z), temprot, t);
        //        yield return null;
        //    }
        //    yield return null;
        //}
    }



    public void BackUp()
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
            StartCoroutine(Backing());
    }

    IEnumerator Backing()
    {
        float t = 0;
        temp = transform.rotation;
        while (t < 1)
        {
            t += Time.deltaTime / 1f;
            transform.rotation = Quaternion.Lerp(temp, init, t);
            yield return null;
        }
    }
}
