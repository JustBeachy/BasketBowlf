using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    float timer;
    public float delay;
    public bool isVertical;
    public bool isUpDown;
    public float reverseTime=2;
    public float speed = 1;
    public int dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (delay > 0)
        {
            if (timer > delay)
            {
                delay = 0;
                timer = 0;
            }
        }
        else if (timer > reverseTime)
        {
            dir *= -1;
            timer = 0;
        }

        if (delay == 0)
        {
            if (!isUpDown)
            {
                if (!isVertical)
                    transform.position += new Vector3(speed * dir, 0, 0) * Time.deltaTime;
                else
                    transform.position += new Vector3(0, 0, speed * dir) * Time.deltaTime;
            }
            else
                transform.position += new Vector3(0, speed * dir, 0) * Time.deltaTime;
        }
    }
}
