using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script make move the associated gameObject circularly and  indefinitely
 * and change its texture when there is a collision (use onTrigger, check the trigger case in Box collider part in the inspector)
 * */
public class circleMovement : MonoBehaviour {


    public float speed;
    public float width;
    public float heigh;

    //Texture 1 is the default texture, texture2 is when one of the two hands reach the object
    public Texture texture1;
    public Texture texture2;

    private float timecounter;

    //This the living time for the texture, when there is a collision, texture2 will be set and to make it more smoothy, 
    //when collision is ending, we wait few time to change again the texture. This time is defined by above variable.
    private float defaultLivingTime = 0.25f;
    private float livingTime;
    

    // Use this for initialization
    void Start () {
        livingTime = defaultLivingTime;

    }

    void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().material.mainTexture = texture2;
        livingTime = defaultLivingTime;
    }


    void OnTriggerExit(Collider other)
    {
        if (livingTime <= 0.0f)
        {
            GetComponent<MeshRenderer>().material.mainTexture = texture1;
            livingTime = defaultLivingTime;
        }
    }

    // Update is called once per frame
    void Update () {

        //We move the object in order to create a infinite movement up and down according to the speed variable.
        timecounter += Time.deltaTime * speed;
        float x = Mathf.Cos(timecounter) * width;
        float y = 1.0f + Mathf.Sin(timecounter) * heigh;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);

        livingTime -= Time.deltaTime;
        if (GetComponent<MeshRenderer>().material.mainTexture == texture2 && livingTime <= 0.0f)
        {
            GetComponent<MeshRenderer>().material.mainTexture = texture1;
            livingTime = defaultLivingTime;
        }

    }
}
