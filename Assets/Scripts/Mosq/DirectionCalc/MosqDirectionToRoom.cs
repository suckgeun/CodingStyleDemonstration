using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqDirectionToRoom : MonoBehaviour, IMosqDirection
{
    [SerializeField]
    private float _zLocal = 0f;
    [SerializeField]
    private float _th = 0.2f;

    public Vector3 CalcDirection(int numHits, Collider[] hits)
    {
        // if mosq is not in room, return zero vector;
        if (transform.parent == null) 
        { 
            return Vector3.zero; 
        }

        float zDiff = _zLocal - transform.localPosition.z;

        // if it is at its desired position, return 0 
        if (Mathf.Abs(zDiff) < _th)
        {
            return Vector3.zero;
        }

        // if mosq is in room, reduce its z value to match the desired local z  
        return new Vector3(0, 0, zDiff);
    }
}
