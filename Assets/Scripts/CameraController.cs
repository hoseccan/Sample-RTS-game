using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _camera;
    float hight;
    float width;
    public float persent;
    public float speed;

    private void Start()
    {
        hight = Screen.height;
        width = Screen.width;
    }
    void Update()
    {
        if (Input.mousePosition.x > width * (1 - persent * 0.01) || Input.GetAxis("Horizontal") > 0)
        {
            transform.Translate(Vector3.right * speed);
        }
        if (Input.mousePosition.x < width * (persent * 0.01) || Input.GetAxis("Horizontal") < 0)
        {
            transform.Translate(Vector3.left * speed);
        }
        if (Input.mousePosition.y > hight * (1 - persent * 0.01) || Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(Vector3.forward * speed);
        }
        if (Input.mousePosition.y < hight * (persent * 0.01) || Input.GetAxis("Vertical") < 0)
        {
            transform.Translate(Vector3.back * speed);
        }
        if ((Input.GetKey(KeyCode.Q) || Input.mouseScrollDelta.y > 0) && _camera.transform.position.y > 8f)
        {
            _camera.Translate(Vector3.forward * speed);
        }
        if ((Input.GetKey(KeyCode.E) || Input.mouseScrollDelta.y < 0) && _camera.transform.position.y < 75f)
        {
            _camera.Translate(Vector3.back * speed);
        }
    }
}
