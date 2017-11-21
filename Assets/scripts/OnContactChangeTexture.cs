using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script must be associated with the target gameObject.
 * Each time the target will be touch by hands, this script will change its texture and create a copy of this sameObject with a random postion.
 * */
public class OnContactChangeTexture : MonoBehaviour
{

	//Texture 1 is the default texture, texture2 is when one of the two hands reach the object
    public Texture texture1;
    public Texture texture2;

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject congratulationText;

    //How many time the script will create a copy
    public int numberMaxStep = 2;
    private int nbStep;

    //because we create a copy, this scrypt will be duplicate too.
    //In order to keep synchronisation, objects which are already been reached must not interfere with the active script.
    //This variable inform if the script has already create his copy or not.
    private bool instantiated = false;

    private string currentHandTracked;

    // Use this for initialization
    void Start()
    {
        congratulationText = GameObject.FindGameObjectWithTag("text");
        congratulationText.GetComponent<TextMesh>().text = "";

    }

    void OnTriggerEnter(Collider other)
    {
        //If it's the first object, we will check the hand which activated the first step. Only this hand will be able to continue
        if ((nbStep == 0 && !instantiated) ||
            (other.name.Equals(currentHandTracked) && !instantiated && nbStep <= numberMaxStep))
        {
            if (nbStep == 0)
            {
                if (other.gameObject.name.Equals("Contact LeftFingerbone"))
                    currentHandTracked = "Contact LeftFingerbone";
                else
                    currentHandTracked = "Contact RightFingerbone";
            }
            nbStep++;

            if (nbStep < numberMaxStep) DrawNextStep();
            GetComponent<MeshRenderer>().material.mainTexture = texture2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( nbStep == numberMaxStep)
            congratulationText.GetComponent<TextMesh>().text = "Congratulation !!!";
    }

    void DrawNextStep()
    {
        //Position is decided randomly in the user hands range.
        float x = Random.Range(-0.3f, 0.3f);
        float y = Random.Range(0.7f, 1.3f);
		float z = Random.Range(0.2f, 0.35f);
        Vector3 position = new Vector3(x, y, z);

        OnContactChangeTexture obj = Instantiate(this, position, transform.rotation);
        obj.name = "step" + nbStep;
        obj.nbStep = nbStep;
        obj.currentHandTracked = currentHandTracked;
        obj.congratulationText = congratulationText;

        instantiated = true;
    }

}
