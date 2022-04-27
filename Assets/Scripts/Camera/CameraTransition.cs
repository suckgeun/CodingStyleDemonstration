using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField]
    private Transform _vcTrans;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _roomCamLocalXAngleInBody;
    [SerializeField]
    private float _bodyCamLocalXAngleInBody;

    private Vector3 _startWorldPos;
    private Vector3 _endWorldPos;
    private float _startLocalXAngle;
    private float _endLocalXAngle;

    private bool _isMoving = false;
    private float _startTime;
    private float _t;

    private Action _transEndCallBack;

    public void TransitFromRoomToBody(Vector3 roomCamWorldPos, Vector3 bodyCamWorldPos, Transform parentBody, Action callBack)
    {
        // we rotate camera in the room we enter. 
        // so, make the transition camera parent to room Enter
        _vcTrans.SetParent(parentBody);

        // save the rotation info 
        _startWorldPos = roomCamWorldPos;
        _endWorldPos = bodyCamWorldPos;
        _startLocalXAngle = _roomCamLocalXAngleInBody;
        _endLocalXAngle = _bodyCamLocalXAngleInBody;
        _transEndCallBack = callBack;

        // move transition camera to the start position 
        _vcTrans.position = _startWorldPos;
        _vcTrans.localRotation = Quaternion.Euler(_startLocalXAngle, 0, 0);

        // reset the timer settings 
        _startTime = Time.time;
        _t = 0f;

        // start moving 
        _vcTrans.gameObject.SetActive(true);
        _isMoving = true;
    }

    public void TransitFromBodyToRoom(Vector3 bodyCamWorldPos, Vector3 roomCamWorldPos, Transform parentBody, Action callBack)
    {
        // we rotate camera in the room we enter. 
        // so, make the transition camera parent to room Enter
        _vcTrans.SetParent(parentBody);

        // save the rotation info 
        _startWorldPos = bodyCamWorldPos;
        _endWorldPos = roomCamWorldPos;
        _startLocalXAngle = _bodyCamLocalXAngleInBody;
        _endLocalXAngle = _roomCamLocalXAngleInBody;
        _transEndCallBack = callBack;

        // move transition camera to the start position 
        _vcTrans.position = _startWorldPos;
        _vcTrans.localRotation = Quaternion.Euler(_startLocalXAngle, 0, 0);

        // reset the timer settings 
        _startTime = Time.time;
        _t = 0f;

        // start moving 
        _vcTrans.gameObject.SetActive(true);
        _isMoving = true;
    }

    private void Update()
    {
        if (!_isMoving) { return; }

        // calculate the x delta angle 
        _t = (Time.time - _startTime) / _duration;
        float dx = Mathf.Lerp(_startLocalXAngle, _endLocalXAngle, _t);

        // rotation 
        _vcTrans.transform.localRotation = Quaternion.Euler(dx, 0, 0);

        // change position 
        _vcTrans.transform.position = Vector3.Lerp(_startWorldPos, _endWorldPos, _t);

        if (_t > 1)
        {
            _isMoving = false;
            EndTransition();
        }
    }

    private void EndTransition()
    {
        _isMoving = false;
        _transEndCallBack();
        _vcTrans.SetParent(null);
        _vcTrans.gameObject.SetActive(false);
    }
}
