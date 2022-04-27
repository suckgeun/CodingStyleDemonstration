using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public BoxCollider RoomModel => _roomModel;
    public float XMin => _xMin;
    public float XMax => _xMax;
    public float YMin => _yMin;
    public float YMax => _yMax;
    public float CameraXMin => _vcXMin;
    public float CameraXMax => _vcXMax;
    public float CameraYMin => _vcYMin;
    public float CameraYMax => _vcYMax;
    public float PlayerXMin => _pXMin;
    public float PlayerXMax => _pXMax;
    public float PlayerYMin => _pYMin;
    public float PlayerYMax => _pYMax;
    public float EdgeXMin => _eXMin;
    public float EdgeXMax => _eXMax;
    public float EdgeYMin => _eYMin;
    public float EdgeYMax => _eYMax;
    public float StartXMin => _sXMin;
    public float StartXMax => _sXMax;
    public float StartYMin => _sYMin;
    public float StartYMax => _sYMax;
    public Vector3 FixedCamLocalPos => _fixedCamLocalPos;
    public Vector3 FixedCamWorldPos => transform.TransformPoint(_fixedCamLocalPos);
    public Quaternion FixedCamRota => _fixedCamRota;
    public bool IsBody => _isBody;
    public Vector3 NormalWorldDir => _normal;
    public Room RoomRight => _roomRight;
    public Room RoomLeft => _roomLeft;

    [SerializeField]
    private BoxCollider _roomModel;

    [Header("Player Movement Limit Settings")]
    // player can move where pMin = min + pOffset, pMax = max - pOffset
    [SerializeField]
    private float _playerXOffset;
    [SerializeField]
    private float _playerYOffset;

    [Header("offset for enter other room")]
    // player move to other room where eMin = min + eOffset, eMax = max - eOffset
    [SerializeField]
    private float _edgeXOffset;
    [SerializeField]
    private float _edgeYOffset;

    [Header("Player Start position When moving other room")]
    [SerializeField]
    private float _startXOffset;
    [SerializeField]
    private float _startYOffset;

    [Header("Camera Movement Settings")]
    // camera can move where vcMin = min + vcOffset, vcMax = max - vcOffset
    [SerializeField]
    private float _vcXOffset;
    [SerializeField]
    private float _vcYOffset;

    [Header("Fixed Camera Settings")]
    [SerializeField]
    private Transform _vcFixed;

    [Header("Rooms Near")]
    [SerializeField]
    private Room _roomLeft;
    [SerializeField]
    private Room _roomRight;

    [Header("Check if room is body")]
    [SerializeField]
    private bool _isBody;

    private float _xMin;
    private float _xMax;
    private float _yMin;
    private float _yMax;

    private float _vcXMin;
    private float _vcXMax;
    private float _vcYMin;
    private float _vcYMax;

    private float _pXMin;
    private float _pXMax;
    private float _pYMin;
    private float _pYMax;

    private float _eXMin;
    private float _eXMax;
    private float _eYMin;
    private float _eYMax;

    private float _sXMin;
    private float _sXMax;
    private float _sYMin;
    private float _sYMax;

    private Vector3 _normal;
    private Vector3 _fixedCamLocalPos;
    private Quaternion _fixedCamRota;

    private void Awake()
    {
        // check if offset is corret 
        if (!IsOffsetCorrect())
        {
            // throw new ArgumentException("Player offset must be greater than Edge offset");
        }

        Vector3 scale = _roomModel.transform.localScale;
        Vector2 p1 = new Vector2(scale.x / 2f, scale.y / 2f);
        Vector2 p2 = new Vector2(-scale.x / 2f, -scale.y / 2f);

        _xMin = Mathf.Min(p1.x, p2.x);
        _xMax = Mathf.Max(p1.x, p2.x);
        _yMin = Mathf.Min(p1.y, p2.y);
        _yMax = Mathf.Max(p1.y, p2.y);

        _vcXMin = _xMin + _vcXOffset;
        _vcXMax = _xMax - _vcXOffset;
        _vcYMin = _yMin + _vcYOffset;
        _vcYMax = _yMax - _vcYOffset;

        _pXMin = _xMin + _playerXOffset;
        _pXMax = _xMax - _playerXOffset;
        _pYMin = _yMin + _playerYOffset;
        _pYMax = _yMax - _playerYOffset;

        _eXMin = _xMin + _edgeXOffset;
        _eXMax = _xMax - _edgeXOffset;
        _eYMin = _yMin + _edgeYOffset;
        _eYMax = _yMax - _edgeYOffset;

        _sXMin = _xMin + _startXOffset;
        _sXMax = _xMax - _startXOffset;
        _sYMin = _yMin + _startYOffset;
        _sYMax = _yMax - _startYOffset;

        _normal = transform.TransformDirection(Vector3.forward);

        if (_vcFixed != null)
        {
            _fixedCamLocalPos = _vcFixed.localPosition;
            _fixedCamRota = _vcFixed.rotation;
        }
    }

    private bool IsOffsetCorrect()
    {
        if (_playerXOffset >= _edgeXOffset || 
            _edgeXOffset >= _startXOffset || 
            _startXOffset >= _vcXOffset ||
            _playerYOffset >= _edgeYOffset ||
            _edgeYOffset >= _startYOffset ||
            _startYOffset >= _vcYOffset)
        {
            return false;
        }

        return true;
    }
}
