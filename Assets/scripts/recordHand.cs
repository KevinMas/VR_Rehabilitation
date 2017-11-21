using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using Leap.Unity;

namespace Leap.Unity
{
    /**
     * This script records hand position, especially palm and index tip position.
     * Associate this script with concerned hands and notify if it the left hand or not
     * Name file and save folder path must be informed
     * The final file is a .csv with the character ',' for separating values.
     * */
    public class recordHand : MonoBehaviour
    {
        public bool isLeft;

        public bool record;
        public string FileName;
        public string folder;

		public bool thumb;
		public bool index;
		public bool middle;
		public bool ring;
		public bool pinky;


        private StringBuilder stringBuilder;

        private LeapServiceProvider provider;


        // Use this for initialization
        void Start()
        {
            provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;

            stringBuilder = new StringBuilder();

			string titles = "TIME,PALM_X,PALM_Y,PALM_Z";
			if (index)
				titles += ",INDEX_X,INDEX_Y,INDEX_Z";
			if (thumb)
				titles += ",THUMB_X,THUMB_Y,THUMB_Z";
			if (middle)
				titles += ",MIDDLE_X,MIDDLE_Y,MIDDLE_Z";
			if (ring)
				titles += ",RING_X,RING_Y,RING_Z";
			if (pinky)
				titles += ",PINKY_X,PINKY_Y,PINKY_Z";

			stringBuilder.AppendLine(titles);
        }

        // Update is called once per frame
        void Update()
        {
            if (record)
            {
                Frame frame = provider.CurrentFrame;
                foreach (Hand hand in frame.Hands)
                {
                    if(hand.IsLeft == isLeft) {
                        //Here palm position is caught
						string value = ""+Time.time + "," + hand.PalmPosition.x + "," + hand.PalmPosition.y + "," + hand.PalmPosition.z;

                        foreach (Finger finger in hand.Fingers)
                        {
							if (finger.Type == Finger.FingerType.TYPE_INDEX && index)
                            {
                                //Here index finger position is caught
                                value += "," + finger.TipPosition.x + "," + finger.TipPosition.y + "," + finger.TipPosition.z;
                            }
							if (finger.Type == Finger.FingerType.TYPE_THUMB && thumb)
							{
								//Here index finger position is caught
								value += "," + finger.TipPosition.x + "," + finger.TipPosition.y + "," + finger.TipPosition.z;
							}
							if (finger.Type == Finger.FingerType.TYPE_MIDDLE && middle)
							{
								//Here index finger position is caught
								value += "," + finger.TipPosition.x + "," + finger.TipPosition.y + "," + finger.TipPosition.z;
							}
							if (finger.Type == Finger.FingerType.TYPE_RING && ring)
							{
								//Here index finger position is caught
								value += "," + finger.TipPosition.x + "," + finger.TipPosition.y + "," + finger.TipPosition.z;
							}
							if (finger.Type == Finger.FingerType.TYPE_PINKY && pinky)
							{
								//Here index finger position is caught
								value += "," + finger.TipPosition.x + "," + finger.TipPosition.y + "," + finger.TipPosition.z;
							}
                        }
                        stringBuilder.AppendLine(value);
                        Savecsv(stringBuilder);
                    }
                }
            }
        }

        void Savecsv(StringBuilder sb)
        {
            StreamWriter writer = new StreamWriter(folder +"\\" + FileName + ".csv");
            writer.WriteLine(sb.ToString());
            writer.Close();
        }
    }
}
