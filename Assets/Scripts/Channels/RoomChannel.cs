using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "ScriptableObjects/Channels/RoomChannel")]
public class RoomChannel : ScriptableObject
{
    public UnityAction<Room, Room, Vector3, Transform> OnEnterRoom;
    public UnityAction<Room> OnExitRoom;

    /// <summary>
    /// raise event when entering room from all sources 
    /// </summary>
    /// <param name="roomEnter"> room entering </param>
    /// <param name="roomFrom"> room from </param>
    /// <param name="enterLocalPos"> if enterPos is not zero vector, player will be set there </param>
    /// <param name="player"></param>
    public void RaiseEnterRoomEvent(Room roomEnter, Room roomFrom, Vector3 enterLocalPos, Transform player)
    {
        if (OnEnterRoom != null)
        {
            OnEnterRoom.Invoke(roomEnter, roomFrom, enterLocalPos, player);
            // _playerState.PlayerLocation = IPlayerLocation.Room;
        }
        else
        {
            Debug.LogWarning("Room Enter event raised, but no one was listening");
        }
    }

    public void RaiseExitRoomEvent(Room roomExit)
    {
        if (OnExitRoom != null)
        {
            OnExitRoom.Invoke(roomExit);
            // _playerState.PlayerLocation = IPlayerLocation.Room;
        }
        else
        {
            Debug.LogWarning("Room Exit event raised, but no one was listening");
        }
    }
}
