using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _vCFixed;
    [SerializeField]
    private CameraTransition _cameraTrans;

    [SerializeField]
    private BodyChannel _bodyChannel;

    // save the room camera info transitioning in 
    private Vector3 _roomCamWorldPos;
    private Quaternion _roomCamRota;
    private Room _room;

    [SerializeField]
    private bool _isEnterBodyAllowed = false;

    // TODO 
    // cam z pos should be saved in different SO 
    [SerializeField]
    private float _zPos = 0f;

    private bool _isPlayerIn = false;

    private void Awake()
    {
        _room = GetComponent<Room>();
    }

    private void OnEnable()
    {
        _bodyChannel.OnEnterBody += OnEnterBody;
        _bodyChannel.OnExitBody += OnExitBody;
        _bodyChannel.OnChangeBody += OnChangeBody;
    }

    private void OnDisable()
    {
        _bodyChannel.OnEnterBody -= OnEnterBody;
        _bodyChannel.OnExitBody -= OnExitBody;
        _bodyChannel.OnChangeBody -= OnChangeBody;
    }

    private void OnEnterBody(Room roomFrom, Transform player, Vector3 playerRoomLocalPos)
    {
        if (!_isEnterBodyAllowed) { return; }

        Debug.Log("body entering: " + this.name);

        // get the starting camera position from playerPos
        _roomCamWorldPos = GetRoomCamPos(roomFrom, playerRoomLocalPos);
        _roomCamRota = Quaternion.LookRotation(roomFrom.NormalWorldDir);

        _cameraTrans.TransitFromRoomToBody(
            _roomCamWorldPos, _vCFixed.position, transform, EnableCamera);

        _isPlayerIn = true;
    }

    private Vector3 GetRoomCamPos(Room roomFrom, Vector3 playerRoomLocalPos)
    {
        // clamp the x, y, z to find cam pos 
        float x = Mathf.Clamp(playerRoomLocalPos.x, roomFrom.CameraXMin, roomFrom.CameraXMax);
        float y = Mathf.Clamp(playerRoomLocalPos.y, roomFrom.CameraYMin, roomFrom.CameraYMax);
        Vector3 camLocalPos = new Vector3(x, y, _zPos);
        Vector3 camWorldPos = roomFrom.transform.TransformPoint(camLocalPos);

        return camWorldPos;
    }

    private void EnableCamera()
    {
        if (_isPlayerIn)
        {
            _vCFixed.gameObject.SetActive(true);
        }
    }

    private void OnExitBody(Room bodyExiting)
    {
        if (bodyExiting != _room) { return; }

        _isPlayerIn = false;
        DisableCamera();
    }

    private void OnChangeBody(Room bodyFrom)
    {
        if (bodyFrom != _room)
        {
            _isPlayerIn = true;
            EnableCamera();
        }
    }

    private void DisableCamera()
    {
        _vCFixed.gameObject.SetActive(false);
    }
}
