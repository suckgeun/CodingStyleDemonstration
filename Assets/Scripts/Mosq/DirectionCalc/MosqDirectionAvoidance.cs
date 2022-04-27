using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqDirectionAvoidance : MonoBehaviour, IMosqDirection
{
    [SerializeField]
    private ELayers[] _layersToAvoid;
    [SerializeField]
    private float _maxAvoidPower;
    [SerializeField]
    private float _decreaseWeight = 0.5f;

    private HashSet<ELayers> _layers;
    private Vector3 _prevDir = Vector3.zero;

    private void Awake()
    {
        _layers = new HashSet<ELayers>(_layersToAvoid);
    }

    public Vector3 CalcDirection(int numHits, Collider[] hits)
    {
        Vector3 dir = Vector3.zero;

        for (int i=0; i < numHits; i++)
        {
            ELayers layer = (ELayers)hits[i].gameObject.layer;
            
            // if layer is not in avoidance, ignore
            if (!_layers.Contains(layer)) { continue; }

            // if hit is mosq itself, ignore 
            if (hits[i].transform.parent == this.transform) { continue; }

            // if hit is rooom or body, 
            if (layer == ELayers.Room || layer == ELayers.Body)
            {
                // if we are in room, ignore it. 
                if (transform.parent != null) { continue; }
            }

            if (IsWall(layer))
            {
                dir -= GetWallNormal(hits[i].transform);
            }
            else
            {
                // Debug.LogWarning("AwayFrom2 " + hits[i].transform.parent.name + ": " + GetAwayVectorFrom(hits[i].transform));
                dir += GetAwayVectorFrom(hits[i].transform);
                // dir = (dir / (Vector3.Distance(transform.position, hits[i].transform.position) + 0.1f));
            }
        }

        dir = (dir.normalized + _prevDir.normalized * _decreaseWeight) * _maxAvoidPower;
        _prevDir = dir ;

        if (transform.parent != null)
        {
            return new Vector3(dir.x, dir.y, 0);
        }

        return dir;
    }

    private bool IsWall(ELayers layer)
    {
        if (layer == ELayers.Boundary || layer == ELayers.Room || layer == ELayers.Body)
        {
            return true;
        }
        return false;
    }

    private Vector3 GetWallNormal(Transform wall)
    {
        return wall.TransformDirection(Vector3.forward);
    }

    private Vector3 GetAwayVectorFrom(Transform obj)
    {
        return transform.position - obj.position;
    }
}
