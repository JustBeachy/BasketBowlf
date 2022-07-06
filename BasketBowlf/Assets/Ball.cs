using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    public GameObject cam, hoop, hole;
    Vector2 mouseStartPos, mouseEndPos;
    public bool start = false;
    float pos;
    int dir = 1;
    bool fixedLaunch;
    public bool pastPins, pastHoop;
    int scoreBowl, scoreBB, scoreGolf;
    int bounceCount;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

   /* private void FixedUpdate()
    {
        if (fixedLaunch)
        {
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0, 0, 100), ForceMode.Impulse);
            fixedLaunch = false;
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector3(0, 0, 300), ForceMode.Impulse);
            }

            if (Input.GetMouseButtonDown(0))
            {
                mouseStartPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseEndPos = Input.mousePosition;

                if (!pastHoop)
                rb.AddForce(new Vector3((mouseEndPos.x - mouseStartPos.x) * 2, 0, (mouseEndPos.y - mouseStartPos.y) * 4) * 2);
                if(bounceCount<3)
                rb.AddTorque(new Vector3(/*Mathf.Abs*/(mouseEndPos.y - mouseStartPos.y) * 4, 0, (mouseStartPos.x - mouseEndPos.x) * 4));
            }
        }
        else
        {
            pos += Time.deltaTime * 6 * dir;
            if (pos > 3 || pos < -3)
                dir = (int)Mathf.Sign(pos)*-1;

            rb.position = new Vector3(pos, rb.position.y, rb.position.z);

            if(Input.GetMouseButtonDown(0))
            {
                start = true;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector3(-30, 0, 100), ForceMode.Impulse);
                fixedLaunch = true;
            }
        }
        
        if(!pastPins && transform.position.y<-15)
        {
            pastPins = true;

            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Pin"))
            {
                
                if (p.GetComponentInChildren<Transform>().position.y<1.2)
                    scoreBowl++;
            }
            
        }

        if (!pastHoop && transform.position.y < hoop.transform.position.y)
        {
            pastHoop = true;
            
        }
        if(pastHoop && rb.velocity==Vector3.zero&&rb.angularVelocity==Vector3.zero)
        {
            float dis = Vector3.Distance(transform.position, hole.transform.position);
            if(scoreGolf<10)
            {
                if (dis >= 50)
                    scoreGolf = 1;
                else
                    scoreGolf = (10 - ((int)dis/5)) + 1;
            }

        }


    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag=="CamTrigger")
        {
            cam.transform.rotation = Quaternion.Euler(new Vector3(75, 0, 0));
            cam.GetComponent<CameraObj>().distanceAboveBall = 9; 
        }

        if (collision.gameObject.tag == "Net")
        {
            scoreBB = 10;
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Green")
        {
            if (bounceCount < 3)
            {
                rb.AddForce(new Vector3(-rb.angularVelocity.z, 0, rb.angularVelocity.x) * 2000);
                bounceCount++;
            }
            else
                rb.angularDrag = 2f;
        }

        if (collision.gameObject.tag == "Rim")
        {
            if (scoreBB != 10)
            {
                if (scoreBB < 4)
                    scoreBB = 4;
                else
                    scoreBB = 7;
            }
        }

        if (collision.gameObject.tag == "Backboard")
        {
            if (scoreBB != 10)
            {
                if (scoreBB < 4)
                    scoreBB = 3;
                else
                    scoreBB = 7;
            }
        }
    }
}
