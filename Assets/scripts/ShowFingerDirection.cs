/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/
// Original script: "MagneticPinch.cs" modified by unitycoder.com to just get the finger position & directions

using UnityEngine;
using System.Collections;
using Leap;
using System.Text;
using System.IO;

namespace Leap.Unity {

	/**
	 * This script was for debuging, it show with lines hand finger bones on Scene screen. 
	 * It draws finger tip direction too.
	 **/
    public class ShowFingerDirection : MonoBehaviour
    {
        HandModel hand_model;
        Hand leap_hand;

        private StringBuilder stringBuilder;

        void Start()
        {
            hand_model = GetComponent<HandModel>();
            leap_hand = hand_model.GetLeapHand();
            if (leap_hand == null) Debug.LogError("No leap_hand founded");

        }

        void Update()
        {
            for (int i = 0; i < HandModel.NUM_FINGERS; i++)
            {
                FingerModel finger = hand_model.fingers[i];
                // draw ray from finger tips (enable Gizmos in Game window to see)
                Debug.DrawRay(finger.GetTipPosition(), finger.GetRay().direction, Color.red);

                for( int j = 0; j < 4; j++)
                    Debug.DrawLine(finger.GetJointPosition(j), finger.GetJointPosition(j+1), Color.blue);

            }
        }

    }
}