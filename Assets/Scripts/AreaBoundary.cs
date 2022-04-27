using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBoundary : MonoBehaviour
{
    private BoxCollider _area;

    private void Awake()
    {
        _area = GetComponent<BoxCollider>();
    }

    public bool IsInArea(Transform obj)
    {
        return _area.bounds.Contains(obj.position);
    }
}
