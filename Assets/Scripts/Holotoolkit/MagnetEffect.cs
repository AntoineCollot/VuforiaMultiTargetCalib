using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEffect : Gazable
{
    [SerializeField]
    protected float scaleRatio;

    [SerializeField]
    protected Transform center;

    new Collider collider;
    Vector3 originalSize;

    // Use this for initialization
    protected virtual void Start()
    {
        collider = GetComponent<Collider>();

        System.Type type = collider.GetType();

        if (type == typeof(CapsuleCollider))
        {
            CapsuleCollider caspule = (CapsuleCollider)collider;
            originalSize.x = caspule.radius;
            originalSize.y = caspule.height;
        }
        else if(type == typeof(BoxCollider))
        {
            originalSize = ((BoxCollider)collider).size;
        }
        else if (type == typeof(SphereCollider))
        {
            originalSize.x = ((SphereCollider)collider).radius;
        }

    }

    public override void OnHover()
    {
        System.Type type = collider.GetType();

        if (type == typeof(CapsuleCollider))
        {
            CapsuleCollider caspule = (CapsuleCollider)collider;
            caspule.radius = originalSize.x* scaleRatio;
            caspule.height = originalSize.y* scaleRatio;
        }
        else if (type == typeof(BoxCollider))
        {
            ((BoxCollider)collider).size = originalSize* scaleRatio;
        }
        else if (type == typeof(SphereCollider))
        {
            ((SphereCollider)collider).radius = originalSize.x * scaleRatio;
        }
    }

    public override void OffHover()
    {
        System.Type type = collider.GetType();

        if (type == typeof(CapsuleCollider))
        {
            CapsuleCollider caspule = (CapsuleCollider)collider;
            caspule.radius = originalSize.x;
            caspule.height = originalSize.y;
        }
        else if (type == typeof(BoxCollider))
        {
            ((BoxCollider)collider).size = originalSize;
        }
        else if (type == typeof(SphereCollider))
        {
            ((SphereCollider)collider).radius =originalSize.x;
        }
    }

    public virtual Vector3 GetCenter()
    {
        return center.position;
    }
}
