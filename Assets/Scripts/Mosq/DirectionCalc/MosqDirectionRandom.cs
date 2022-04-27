using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqDirectionRandom : MonoBehaviour, IMosqDirection
{
    [SerializeField]
    private float _updateAngle;
    [SerializeField]
    [Tooltip("When mosquito meet this, it will reset direction")]
    private ELayers[] _layersToReset;

    private Vector3 _dir;
    private HashSet<ELayers> _layers;

    private void Awake()
    {
        _layers = new HashSet<ELayers>(_layersToReset);
    }

    private void Start()
    {
        // initialize dir
        _dir = InitializeRandomDirection();
    }

    public Vector3 CalcDirection(int numHits, Collider[] hits)
    {
        if (ShouldResetDirection(numHits, hits))
        {
            _dir = InitializeRandomDirection();
        }
        else
        {
            UpdateRandomDirection();
        }

        // Debug.Log("Random dir: " + _dir);
        return _dir;
    }

    private bool ShouldResetDirection(int numHits, Collider[] hits)
    {
        // check if it should reset direction 
        for (int i = 0; i < numHits; i++)
        {
            ELayers layer = (ELayers)hits[i].gameObject.layer;

            // if hit is not in layers, ignore it 
            if (!_layers.Contains(layer)) { continue; }

            // if hit is room and mosq is in room, we ignore it 
            if (layer == ELayers.Room && transform.parent != null) { continue; }

            // if hit is itself, ignore 
            if (hits[i].transform.parent == this.transform) { continue; }

            return true;
        }

        return false;
    }

    private Vector3 InitializeRandomDirection()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);
        float z = 0f;
        // if mosq is in area, z should have random value too 
        if (transform.parent == null)
        {
            z = UnityEngine.Random.Range(-1f, 1f);
        }

        return new Vector3(x, y, z);
    }

    private void UpdateRandomDirection()
    {
        float x = UnityEngine.Random.Range(-_updateAngle, _updateAngle);
        float y = UnityEngine.Random.Range(-_updateAngle, _updateAngle);
        float z = 0f;
        // if mosq is in area, z should have random value too 
        if (transform.parent == null)
        {
            z = UnityEngine.Random.Range(-_updateAngle, _updateAngle);
        }

        _dir = Quaternion.Euler(x, y, z) * _dir;
    }
}
