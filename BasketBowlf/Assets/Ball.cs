using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    public GameObject cam;
    Vector2 mouseStartPos, mouseEndPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 0, 300), ForceMode.Impulse);
        }

        if(Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseEndPos = Input.mousePosition;
            rb.AddForce(new Vector3((mouseEndPos.x - mouseStartPos.x)*2, 0, (mouseEndPos.y - mouseStartPos.y)*4) *2);
            rb.AddTorque(new Vector3(/*Mathf.Abs*/(mouseEndPos.y - mouseStartPos.y)*100, 0, (mouseStartPos.x - mouseEndPos.x) * 100) * 10000);
        }


    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag=="Pin")
        {
            cam.transform.rotation = Quaternion.Euler(new Vector3(75, 0, 0));
            cam.GetComponent<CameraObj>().distanceAboveBall = 9;
        }
    }
}
