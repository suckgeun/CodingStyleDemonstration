using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMover : MonoBehaviour
{
    [SerializeField]
    private RoomChannel _roomChannel;
    [SerializeField]
    private BodyChannel _bodyChannel;
    [SerializeField]
    private Transform _upperBody;
    [SerializeField]
    private Transform _lowerBody;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private float _zOffset = -5f;

    private bool _isInRoom = false;


    private void OnEnable()
    {
        _roomChannel.OnEnterRoom += OnRoomEnter;
        _bodyChannel.OnEnterBody += OnBodyEnter;
    }

    private void OnDisable()
    {
        _roomChannel.OnEnterRoom -= OnRoomEnter;
        _bodyChannel.OnEnterBody -= OnBodyEnter;
    }

    private void OnRoomEnter(Room roomEnter, Room roomFrom, Vector3 enterPos, Transform player)
    {
        if (roomEnter.IsBody)
        {
            _isInRoom = false;
        }
        else
        {
            _isInRoom = true;
            LookRotation(roomEnter.transform.forward);
        }
    }

    private void LookRotation(Vector3 forward)
    {
        transform.rotation = Quaternion.LookRotation(forward);
    }

    private void OnBodyEnter(Room roomFrom, Transform player, Vector3 playerPos)
    {
        _isInRoom = false;
    }

    private void Update()
    {
        if (_isInRoom)
        {
            UpdateLocation();
        }
    }

    private void UpdateLocation()
    {
        Vector3 localPos = new Vector3(_player.localPosition.x, 0, _zOffset);
        Vector3 globalPos = _player.parent.TransformPoint(localPos);
        transform.position = globalPos;
    }
}
