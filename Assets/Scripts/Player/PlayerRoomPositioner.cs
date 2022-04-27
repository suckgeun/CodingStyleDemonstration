using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomPositioner : MonoBehaviour
{
    [Header("Required Channels")]
    [SerializeField]
    private RoomChannel _roomChannel;
    [SerializeField]
    private BodyChannel _bodyChannel;

    [Header("Player Body Rooms")]
    [SerializeField]
    private Room _bodyLower;
    [SerializeField]
    private Room _bodyUpper;

    [Header("For Debugging")]
    [SerializeField]
    private Room _currentRoom;

    private Vector3 _FALSE_VALUE = new Vector3(-999, -999, -999);

    // save info when going to body 
    private Room _prevRoom;
    private Vector3 _prevRoomPos;


    private void Start()
    {
        // for test. 
        // when start, enter room 1
        if (_currentRoom == null)
        {
            throw new ArgumentNullException("initiating _currentRoom for test is needed");
        }
        EnterRoom(_currentRoom);
        _roomChannel.RaiseEnterRoomEvent(_currentRoom, null, _FALSE_VALUE, this.transform);
    }


    private void OnEnable()
    {
        // channel for room events
        _roomChannel.OnEnterRoom += OnRoomEnter;
        _roomChannel.OnExitRoom += OnRoomExit;

        // chanel for body events
        _bodyChannel.OnEnterBody += OnBodyLowerEnter;
        _bodyChannel.OnChangeBody += OnBodyChange;
        _bodyChannel.OnExitBody += OnBodyExit;
    }

    private void OnDisable()
    {
        // channel for room events
        _roomChannel.OnEnterRoom -= OnRoomEnter;
        _roomChannel.OnExitRoom -= OnRoomExit;

        // chanel for body events
        _bodyChannel.OnEnterBody -= OnBodyLowerEnter;
        _bodyChannel.OnChangeBody -= OnBodyChange;
        _bodyChannel.OnExitBody -= OnBodyExit;
    }

    private void OnRoomEnter(Room roomEnter, Room roomFrom, Vector3 enterLocalPos, Transform player)
    {
        EnterRoom(roomEnter);
        PositionInRoom(roomEnter, roomFrom, enterLocalPos);
    }

    private void EnterRoom(Room roomEnter)
    {
        this.transform.SetParent(roomEnter.transform);
        this.transform.localRotation = Quaternion.identity;
        _currentRoom = roomEnter;
    }

    private void PositionInRoom(Room roomEnter, Room roomFrom, Vector3 enterLocalPos)
    {
        if (enterLocalPos == _FALSE_VALUE)
        {
            SetPlayerStartingPos(GetStartingLocalPos(roomEnter, roomFrom));
        }
        else
        {
            SetPlayerStartingPos(enterLocalPos);
        }
    }

    private void SetPlayerStartingPos(Vector3 pos)
    {
        float x = Mathf.Clamp(pos.x, _currentRoom.StartXMin, _currentRoom.StartXMax);
        float y = Mathf.Clamp(pos.y, _currentRoom.StartYMin, _currentRoom.StartYMax);
        Vector3 startPos = new Vector3(x, y, 0);

        this.transform.localPosition = startPos;
    }

    private Vector3 GetStartingLocalPos(Room roomEnter, Room roomFrom)
    {
        if (roomEnter.RoomLeft == roomFrom)
        {
            return new Vector3(roomEnter.StartXMin, transform.localPosition.y, 0);
        }
        else if (roomEnter.RoomRight == roomFrom)
        {
            Vector3 pos = new Vector3(roomEnter.StartXMax, transform.localPosition.y, 0);
            return pos;
        }
        else
        {
            Debug.LogWarning("roomEnter does not match room left nor room right. starting from zero vector");
            return Vector3.zero;
        }
    }

    private void OnRoomExit(Room roomExit)
    {
        if (!roomExit.IsBody)
        {
            _prevRoom = roomExit;
            _prevRoomPos = transform.localPosition;
        }
    }

    private void OnBodyLowerEnter(Room roomFrom, Transform player, Vector3 playerRoomLocalPos)
    {
        EnterRoom(_bodyLower);
        // position in body lower
        transform.localPosition = new Vector3(0, _bodyLower.StartYMax, 0);
    }

    private void OnBodyChange(Room bodyFrom)
    {
        Room bodyEnter = GetEnteringBody(bodyFrom);
        EnterRoom(bodyEnter);
        PositionInBody(bodyEnter);
    }

    private Room GetEnteringBody(Room bodyFrom)
    {
        if (bodyFrom == _bodyLower)
        {
            return _bodyUpper;
        }
        else
        {
            return _bodyLower;
        }
    }

    private void PositionInBody(Room bodyEnter)
    {
        if (bodyEnter == _bodyLower)
        {
            PositionInBodyLower();
        }
        else if (bodyEnter == _bodyUpper)
        {
            PositionInBodyUpper();
        }
    }

    private void PositionInBodyLower()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _bodyLower.StartYMin, 0);
    }

    private void PositionInBodyUpper()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _bodyUpper.StartYMax, 0);
    }

    private void OnBodyExit(Room bodyExit)
    {

    }

    private void Update()
    {
        if (_currentRoom.IsBody)
        {
            ProcessBodyPos();
        }
        else
        {
            ProcessRoomPos();
        }
    }

    private void ProcessRoomPos()
    {
        if (transform.localPosition.x < _currentRoom.EdgeXMin)
        {
            _roomChannel.RaiseExitRoomEvent(_currentRoom);
            _roomChannel.RaiseEnterRoomEvent(_currentRoom.RoomLeft, _currentRoom, _FALSE_VALUE, this.transform);
        }
        else if (transform.localPosition.x > _currentRoom.EdgeXMax)
        {
            _roomChannel.RaiseExitRoomEvent(_currentRoom);
            _roomChannel.RaiseEnterRoomEvent(_currentRoom.RoomRight, _currentRoom, _FALSE_VALUE, this.transform);
        }
        else if (transform.localPosition.y < _currentRoom.EdgeYMin)
        {
            _roomChannel.RaiseExitRoomEvent(_currentRoom);
            _bodyChannel.RaiseEnterBodyEvent(_currentRoom, this.transform, transform.localPosition);
        }
    }

    private void ProcessBodyPos()
    {
        if (_currentRoom == _bodyLower)
        {
            // when player is at bottom of the screen 
            if (transform.localPosition.y > _currentRoom.EdgeYMax)
            {
                // go back to room from 
                _roomChannel.RaiseExitRoomEvent(_currentRoom);
                _roomChannel.RaiseEnterRoomEvent(_prevRoom, _currentRoom, _prevRoomPos, transform);
            }
            else if (transform.localPosition.y < _currentRoom.EdgeYMin)
            {
                // go to upper body 
                _bodyChannel.RaiseExitBodyEvent(_currentRoom);
                _bodyChannel.RaiseChangeBodyEvent(_currentRoom);
            }
        }
        else if (_currentRoom == _bodyUpper)
        {
            // when player is at bottom of the screen (we use yMax since camera is upside down)
            if (transform.localPosition.y > _currentRoom.EdgeYMax)
            {
                // go to lower body
                _bodyChannel.RaiseExitBodyEvent(_currentRoom);
                _bodyChannel.RaiseChangeBodyEvent(_currentRoom);
            }
            else if (transform.localPosition.y < _currentRoom.EdgeYMin)
            {
                // do nothing 
                //_roomChannel.OnEnterRoom(_roomFrom, _roomSize, Vector3.zero, _player);
                //DiscardPlayer();
            }
            else if (transform.localPosition.x > _currentRoom.EdgeXMax)
            {
                _bodyChannel.RaiseExitBodyEvent(_currentRoom);
                _roomChannel.RaiseEnterRoomEvent(_prevRoom, _currentRoom, _prevRoomPos, transform);
            }
            else if (transform.localPosition.x < _currentRoom.EdgeXMin)
            {
                _bodyChannel.RaiseExitBodyEvent(_currentRoom);
                _roomChannel.RaiseEnterRoomEvent(_prevRoom, _currentRoom, _prevRoomPos, transform);
            }
        }
    }
}
