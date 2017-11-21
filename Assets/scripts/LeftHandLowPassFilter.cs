using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;
using UnityEngine.Collections;
using System.Text;
using System.IO;

namespace Leap.Unity
{
    public class LeftHandLowPassFilter : MonoBehaviour
    {

        public CapsuleHand handModel;

        public Controller controller;

		[Tooltip("More T is high, the more the hand will be filtered. To suspend the filter set T to 0.01.")]
        public float T;

        LeapServiceProvider provider;
        private Hand previousHand;
        
        // Use this for initialization
        void Start()
        {
            provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
            previousHand = new Hand();

			//This part is usefull to be certain that Leap motion will not apply any personal filter on hands.
			//So we set all concerned filter to 0. BUT I don't if this code create any changement or not.
            controller = provider.GetLeapController();

            if (controller.IsConnected)
            {
                controller.Config.Set<float>("skeleton_finger_filtering", 0.0f, delegate (bool success) {
                    if (!success)
                        Debug.Log("Warning: Could not set skeleton_finger_filtering configuration.");
                });
                controller.Config.Set<float>("skeleton_palm_filtering", 0.0f, delegate (bool success) {
                    if (!success)
                        Debug.Log("Warning: Could not set skeleton_palm_filtering configuration.");
                });
                controller.Config.Set<float>("skeleton_velocity_filtering", 0.0f, delegate (bool success) {
                    if (!success)
                        Debug.Log("Warning: Could not set skeleton_velocity_filtering configuration.");
                });

            }
            else
            {
                Debug.Log("The controller is not connected");
            }
        }

				
		// Update is called once per frame
        void Update ()
        {
			//For the current frame, each hand will be checked, if there is a left hand, we will apply the Low Pass Filter.
            Frame frame = provider.CurrentFrame;
            foreach (Hand hand in frame.Hands)
            {
                if (hand.IsLeft)
                {
					if (T == 0.0f) return;

					//calculation of the Low Pass filter value
                    float alpha = 1 - (0.01f / T);
                    float beta = (0.01f / T);
    
                    for (int i = 0; i < HandModel.NUM_FINGERS; i++)
                    {
                        //For each bone, the position will be update according to the Low Pass value
                        for (int j = 0; j < 4; j++)
                        {
							Bone previousBone = getPreviousFinger(hand.Fingers[i].Type).Bone(hand.Fingers[i].bones[j].Type);

                            Vector newPosition = alpha * previousBone.PrevJoint + beta * hand.Fingers[i].bones[j].PrevJoint;
                            Vector newNextPosition = alpha * previousBone.NextJoint + beta * hand.Fingers[i].bones[j].NextJoint;

							//This function will properly update the bone position based on the joint called "previousJoint"
                            hand.Fingers[i].bones[j].SetTransform(newPosition.ToVector3(), hand.Fingers[i].bones[j].Rotation.ToQuaternion());
                            //to make a proper new hand, we update the previous bone's "NexJoint" which is related with the current bone's "PreviousJoint"
                            if (j>0)
                            	hand.Fingers[i].bones[j-1].NextJoint = hand.Fingers[i].bones[j].PrevJoint;
							//For the last bone, we update directly the NextJoint because there is no next bone to update it.
							if (j == 3)
								hand.Fingers[i].bones[j].NextJoint = newNextPosition;
                        }

                    }

					//It is the Palm and Wrist turn to be filtered
					Vector newNextPalmPosition = alpha * previousHand.PalmPosition + beta * hand.PalmPosition;
					Vector newNextWristPosition = alpha * previousHand.WristPosition + beta * hand.WristPosition;

					hand.PalmPosition = newNextPalmPosition;
                    hand.WristPosition = newNextWristPosition;

                    //Finally we set the new hand into our hand Model and ask for re-update the hand in order to refresh the rendering
                    handModel.SetLeapHand(hand);
					handModel.UpdateHand();

                    // Save this current hand for the next frame calculation
                    previousHand.CopyFrom(hand);
                }
            }
        }


        Finger getPreviousFinger(Finger.FingerType fingerType)
        {
            Finger finger = null;
            switch (fingerType)
            {
                case Finger.FingerType.TYPE_INDEX:
                    finger = previousHand.GetIndex(); break;
                case Finger.FingerType.TYPE_MIDDLE:
                    finger = previousHand.GetMiddle(); break;
                case Finger.FingerType.TYPE_PINKY:
                    finger = previousHand.GetPinky(); break;
                case Finger.FingerType.TYPE_RING:
                    finger = previousHand.GetRing(); break;
                case Finger.FingerType.TYPE_THUMB:
                    finger = previousHand.GetThumb(); break;
            }
            return finger;
        }
    }
}
