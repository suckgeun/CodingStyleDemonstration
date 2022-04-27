using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqRoomPositioner : MonoBehaviour, IMosqAbility, IRoomEventListener
{
    private Mosquito _mosq;
    private Room _currentRoom;

    private void Awake()
    {
        _mosq = GetComponent<Mosquito>();
    }

    public void OnRoomEnter()
    {
        if (transform.parent != null) { return; }

        Room roomEnter = GetCloseRoom();
        EnterRoom(roomEnter);
    }

    private Room GetCloseRoom()
    {
        Room roomClosest = null;
        float distClosest = 100f;

        for (int i = 0; i < _mosq.Rooms.Length; i++)
        {
            // we only consider non body rooms 
            if (_mosq.Rooms[i].IsBody) { continue; }

            if (roomClosest == null)
            {
                roomClosest = _mosq.Rooms[i];
                distClosest = GetDistance(roomClosest);
                continue;
            }

            float dist = GetDistance(_mosq.Rooms[i]);
            if (dist < distClosest)
            {
                roomClosest = _mosq.Rooms[i];
                distClosest = dist;
            }
        }

        return roomClosest;
    }

    private float GetDistance(Room roomClose)
    {
        Vector3 closestPos = roomClose.RoomModel.ClosestPoint(transform.position);
        float dist = Vector3.Distance(closestPos, transform.position);
        return dist;
    }

    private void EnterRoom(Room roomEnter)
    {
        // change parent 
        if (roomEnter == null)
        {
            OnAreaEnter();
            return;
        }

        transform.parent = roomEnter.transform;
        transform.localRotation = Quaternion.identity;
        _currentRoom = roomEnter;
    }

    public void OnAreaEnter()
    {
        transform.parent = null;
        transform.localRotation = Quaternion.identity;
        _currentRoom = null;
    }

    public void ProcessEarlyAbility()
    {
        // only check when it is in room 
        if (transform.parent == null) { return; }

        if (_currentRoom.IsBody)
        {
            // TODO 
            // body shift 
        }
        else
        {
            // check if it is near edge of room
            if (transform.localPosition.x > _currentRoom.EdgeXMax)
            {
                EnterRoom(_currentRoom.RoomRight);
            }

            if (transform.localPosition.x < _currentRoom.EdgeXMin)
            {
                EnterRoom(_currentRoom.RoomLeft);
            }
        }
    }

    public void ProcessAbility() { }

}
