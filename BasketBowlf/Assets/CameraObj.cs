using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObj : MonoBehaviour
{
    public GameObject ballToFollow;
    public float distanceAboveBall=1.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ballToFollow.transform.position + new Vector3(0, distanceAboveBall, -3);
    }
}
