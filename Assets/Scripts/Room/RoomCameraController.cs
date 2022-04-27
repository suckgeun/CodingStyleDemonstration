using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    [SerializeField]
    private RoomChannel _roomChannel;
    [SerializeField]
    private FarViewChannel _farViewChannel;

    [SerializeField]
    private CameraPlayerFollow _vcPlayerFollow;
    [SerializeField]
    private Transform _vcFarView;
    [SerializeField]
    private CameraTransition _cameraTransition;

    private Room _roomSize;
    private bool _isFarView;
    private bool _isPlayerIn;
    private Transform _player;

    private void Awake()
    {
        _roomSize = GetComponent<Room>();
    }

    private void OnEnable()
    {
        _roomChannel.OnEnterRoom += OnEnterRoom;
        _roomChannel.OnExitRoom += OnExitRoom;

        _farViewChannel.OnFarViewEnter += OnFarViewEnter;
        _farViewChannel.OnFarViewExit += OnFarViewExit;
    }

    private void OnDisable()
    {
        _roomChannel.OnEnterRoom -= OnEnterRoom;
        _roomChannel.OnExitRoom -= OnExitRoom;

        _farViewChannel.OnFarViewEnter -= OnFarViewEnter;
        _farViewChannel.OnFarViewExit -= OnFarViewExit;
    }

    private void OnEnterRoom(Room roomEnter, Room roomFrom, Vector3 enterPos, Transform player)
    {
        if (_roomSize != roomEnter) { return; }

        _isPlayerIn = true;
        _player = player;

        if (roomFrom == null || !roomFrom.IsBody)
        {
            EnableCamera();
        }
        else
        {
            _cameraTransition.TransitFromBodyToRoom(
                roomFrom.FixedCamWorldPos, _vcPlayerFollow.transform.position,
                roomFrom.transform, EnableCamera);
        }
    }

    private void EnableCamera()
    {
        if (_isFarView)
        {
            _vcFarView.gameObject.SetActive(true);
            _vcPlayerFollow.DisableCamera();
        }
        else
        {
            _vcPlayerFollow.EnableCamera(_player, _roomSize);
            _vcFarView.gameObject.SetActive(false);
        }
    }

    private void OnExitRoom(Room roomExit)
    {
        if (_roomSize != roomExit) { return; }

        _isPlayerIn = false;
        DisableCamera();
    }

    private void DisableCamera()
    {
        _vcFarView.gameObject.SetActive(false);
        _vcPlayerFollow.DisableCamera();
    }

    private void OnFarViewEnter()
    {
        _isFarView = true;
        if (_isPlayerIn)
        {
            EnableCamera();
        }
    }

    private void OnFarViewExit()
    {
        _isFarView = false;
        if (_isPlayerIn)
        {
            EnableCamera();
        }
    }
}
