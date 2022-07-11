using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public Material matBBall, matGball;
    Rigidbody rb;
    public GameObject cam, hoop, hole, popUp;
    Vector2 mouseStartPos, mouseEndPos;
    public bool start = false;
    bool firstClick = false;
    float pos;
    int dir = 1;
    bool fixedLaunch;
    public bool pastPins, pastHoop, pastGolf;
    int scoreBowl, scoreBB, scoreGolf;
    int bounceCount;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        scoreBB = 0;
        scoreBowl = 0;
        scoreGolf = 0;
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

            if (Input.GetMouseButtonDown(0))
            {
                mouseStartPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (firstClick)
                {
                    mouseEndPos = Input.mousePosition;

                    if (!pastHoop)
                        rb.AddForce(new Vector3((mouseEndPos.x - mouseStartPos.x) * 2, 0, (mouseEndPos.y - mouseStartPos.y) * 4) * 2);
                    if (bounceCount < 3)
                        rb.AddTorque(new Vector3(/*Mathf.Abs*/(mouseEndPos.y - mouseStartPos.y) * 4, 0, (mouseStartPos.x - mouseEndPos.x) * 4));
                }
                else
                    firstClick = true;
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
                rb.AddForce(new Vector3(0, 0, 150), ForceMode.Impulse);
                fixedLaunch = true;
            }
        }
        
        if(!pastPins && transform.position.y<-15)
        {
            pastPins = true;
            GetComponent<MeshRenderer>().material = matBBall;

            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Pin"))
            {
                
                if (p.GetComponentInChildren<Transform>().position.y<1.2)
                    scoreBowl++;
            }

            var popup = Instantiate(popUp, GameObject.FindGameObjectWithTag("Canvas").transform);

            popup.GetComponent<PointsPopUp>().desc.text = scoreBowl +" Pins knocked down!";
            popup.GetComponent<PointsPopUp>().points.text = "+" + scoreBowl.ToString();

        }

        if (!pastHoop && transform.position.y < hoop.transform.position.y)
        {
            pastHoop = true;
            GetComponent<MeshRenderer>().material = matGball;

            var popup = Instantiate(popUp, GameObject.FindGameObjectWithTag("Canvas").transform);

            string dtext="";
            if (scoreBB == 0)
                dtext = "Hoop missed";
            if (scoreBB == 3)
                dtext = "Backboard hit!";
            if (scoreBB == 4)
                dtext = "Rim hit!";
            if (scoreBB == 7)
                dtext = "Backboard and rim hit!";
            if (scoreBB == 10)
                dtext = "Basket!";

            popup.GetComponent<PointsPopUp>().desc.text = dtext;
            popup.GetComponent<PointsPopUp>().points.text = "+"+ scoreBB.ToString();


        }
        if(!pastGolf && pastHoop && rb.velocity==Vector3.zero&&rb.angularVelocity==Vector3.zero)
        {
            pastGolf = true;
            float dis = Vector3.Distance(transform.position, hole.transform.position);
            if (scoreGolf != 10)
            {
                
                if (scoreGolf < 10)
                {
                    if (dis >= 50)
                        scoreGolf = 1;
                    else
                        scoreGolf = (9 - ((int)dis / 5));
                    if (scoreGolf < 0)
                        scoreGolf = 0;
                }

            }
            var popup = Instantiate(popUp, GameObject.FindGameObjectWithTag("Canvas").transform);
            popup.GetComponent<PointsPopUp>().points.text = "+" + scoreGolf.ToString();
            if(scoreGolf!=10)
            popup.GetComponent<PointsPopUp>().desc.text = dis.ToString("0.0") + " Meters from the hole.";
            else
            popup.GetComponent<PointsPopUp>().desc.text = "In the hole!";

            int framescore = scoreBB + scoreBowl + scoreGolf;
            ScoreStatic.frameScore[SceneManager.GetActiveScene().buildIndex] = framescore;
            if (SceneManager.GetActiveScene().buildIndex == 0)
                ScoreStatic.totalScore[0] = framescore;
            else
                ScoreStatic.totalScore[SceneManager.GetActiveScene().buildIndex] = ScoreStatic.totalScore[SceneManager.GetActiveScene().buildIndex - 1] + framescore;

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

        }

        if (collision.gameObject.tag == "Hole")
        {
            scoreGolf = 10;
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
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                rb.AddForce(new Vector3(-rb.angularVelocity.z, 0, rb.angularVelocity.x) * 1000);
                bounceCount++;
            }
            else
            {
                rb.angularDrag = 2f;
                rb.drag = 1f;
            }
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
