using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _mosq;
    [SerializeField]
    private int _numGen;
    [SerializeField]
    private BoxCollider _areaBound;

    private Room[] _rooms;

    private void Awake()
    {
        _rooms = FindObjectsOfType<Room>();
    }

    private void Start()
    {
        // generate mosquitos 
        for (int i=0; i < _numGen; i++)
        {
            GameObject obj = Instantiate(_mosq);
            Mosquito mosq = obj.GetComponent<Mosquito>();
            mosq.SetRoomInfo(_rooms);
            mosq.SetAreaBound(_areaBound);
            mosq.name = "mosq" + i;

            // distribute the mosquito 
            mosq.transform.position = GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = _areaBound.center.x + UnityEngine.Random.Range(_areaBound.bounds.min.x, _areaBound.bounds.max.x);
        float y = _areaBound.center.y + UnityEngine.Random.Range(_areaBound.bounds.min.y, _areaBound.bounds.max.y);
        float z = _areaBound.center.z + UnityEngine.Random.Range(_areaBound.bounds.min.z, _areaBound.bounds.max.z);

        return new Vector3(x, y, z);
    }
}
