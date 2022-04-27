using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    private Room _roomSize;
    private Transform _target;
    private bool _isOn = false;

    public void EnableCamera(Transform player, Room roomSize)
    {
        this.gameObject.SetActive(true);
        _target = player;
        _roomSize = roomSize;
        _isOn = true;
    }

    public void DisableCamera()
    {
        _isOn = false;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isOn) { return; }

        // be aware that we are updating LOCAL position
        transform.localPosition = MoveTo(_target.localPosition);
    }

    private Vector3 MoveTo(Vector3 pos)
    {
        float x = Mathf.Clamp(pos.x, _roomSize.CameraXMin, _roomSize.CameraXMax);
        float y = Mathf.Clamp(pos.y, _roomSize.CameraYMin, _roomSize.CameraYMax);

        return new Vector3(x, y, pos.z);
    }
}
