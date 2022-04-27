using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquito : MonoBehaviour
{
    public Vector3 AreaCenter => _areaCenter;
    public Room[] Rooms => _rooms;

    private IMosqAbility[] _abilities;
    private Room[] _rooms;
    private BoxCollider _area;
    private Vector3 _areaCenter;

    private void Start()
    {
        _abilities = GetComponents<IMosqAbility>();
        _areaCenter = _area.transform.TransformPoint(_area.center);
    }

    private void Update()
    {
        // first, process all the early abilities
        foreach (IMosqAbility ability in _abilities)
        {
            ability.ProcessEarlyAbility();
        }

        // then, process all the normal abilities 
        foreach (IMosqAbility ability in _abilities)
        {
            ability.ProcessAbility();
        }
    }

    public void SetRoomInfo(Room[] rooms)
    {
        _rooms = rooms;
    }

    public void SetAreaBound(BoxCollider area)
    {
        _area = area;
    }

    public bool IsInArea()
    {
        return _area.bounds.Contains(transform.position);
    }
}
