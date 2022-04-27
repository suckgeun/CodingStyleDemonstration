using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Channels/FarViewChannel")]
public class FarViewChannel: ScriptableObject
{
    public UnityAction OnFarViewEnter;
    public UnityAction OnFarViewExit;

    public void RaiseFarViewEnterEvent()
    {
        if (OnFarViewEnter != null)
        {
            OnFarViewEnter.Invoke();
        }
        else
        {
            Debug.LogWarning("FarViewModeEnter event was raised but no one was listening");
        }
    }

    public void RaiseFarViewExitEvent()
    {
        if (OnFarViewExit != null)
        {
            OnFarViewExit.Invoke();
        }
        else
        {
            Debug.LogWarning("FarViewModeExit event was raised but no one was listening");
        }
    }
}
