using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    bool hit=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < 5)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit && (collision.gameObject.tag == "Pin" || collision.gameObject.tag == "Ball"))
        {
            GetComponent<AudioSource>().volume = .2f + (GetComponent<Rigidbody>().velocity.magnitude/2);
            GetComponent<AudioSource>().Play();
            hit = true;
        }
    }
}
