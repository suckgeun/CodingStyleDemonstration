using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Channels/BodyChannel")]
public class BodyChannel : ScriptableObject
{
    /// <summary>
    /// roomsize = roomFrom, Transform = player, Vector3 = playerRoomLocalPos
    /// </summary>
    public UnityAction<Room, Transform, Vector3> OnEnterBody;
    /// <summary>
    /// roomsize = bodyFrom
    /// </summary>
    public UnityAction<Room> OnChangeBody;
    /// <summary>
    /// roomSize = body exiting
    /// </summary>
    public UnityAction<Room> OnExitBody;

    /// <summary>
    /// called when player is moving from room to body. 
    /// </summary>
    /// <param name="roomFrom"></param>
    /// <param name="player"></param>
    /// <param name="playerRoomLocalPos"></param>
    public void RaiseEnterBodyEvent(Room roomFrom, Transform player, Vector3 playerRoomLocalPos)
    {
        if (OnEnterBody != null)
        {
            OnEnterBody.Invoke(roomFrom, player, playerRoomLocalPos);
        }
        else
        {
            Debug.LogWarning("Body Enter event was raised but no one was listening");
        }
    }

    /// <summary>
    /// called when moving between bodies. 
    /// </summary>
    /// <param name="bodyFrom"> body player is from </param>
    public void RaiseChangeBodyEvent(Room bodyFrom)
    {
        if (OnChangeBody != null)
        {
            OnChangeBody.Invoke(bodyFrom);
        }
        else
        {
            Debug.LogWarning("Body change event was raised but no one was listening");
        }
    }
    
    public void RaiseExitBodyEvent(Room bodyExit)
    {
        if (OnExitBody != null)
        {
            OnExitBody.Invoke(bodyExit);
        }
        else
        {
            Debug.LogWarning("Body Exit event was raised but no one was listening");
        }
    }
}
