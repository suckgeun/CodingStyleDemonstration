using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class is soley for testing purpose
public class PlayerMovementForTest : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private FarViewChannel _farViewChannel;


    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            float x = transform.localPosition.x - speed * Time.deltaTime;
            Vector3 pos = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = pos;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            float x = transform.localPosition.x + speed * Time.deltaTime;
            Vector3 pos = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = pos;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            float y = transform.localPosition.y + speed * Time.deltaTime;
            Vector3 pos = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            transform.localPosition = pos;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            float y = transform.localPosition.y - speed * Time.deltaTime;
            Vector3 pos = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            transform.localPosition = pos;
        }
        else if (Input.GetKey(KeyCode.Return))
        {
            _farViewChannel.RaiseFarViewEnterEvent();
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            _farViewChannel.RaiseFarViewExitEvent();
        }
    }
}
