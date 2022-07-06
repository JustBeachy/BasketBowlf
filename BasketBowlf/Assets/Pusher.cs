using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.GetComponent<Ball>().start)
        {
            transform.position += new Vector3(0, 0, 5) * Time.deltaTime;
            if (ball.GetComponent<Ball>().pastPins)
                Destroy(gameObject);
        }
    }
}
