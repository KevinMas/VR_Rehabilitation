using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

namespace Leap.Unity
{
	public class ShowLinesBetweenCameraAndObject : MonoBehaviour {

		//Texture 1 is the default texture, texture2 is when one of the two hands reach the line
		public Texture texture1;
		public Texture texture2;

		//the goal of the exercice
		public GameObject obj;

		public int NumberOfLines = 3;

		private int previousNumberOfLines;

		public float lineWith = 0.001f;

		private float previousLineWith;

		private GameObject origine;

		private ArrayList Lines;

		private LeapServiceProvider provider;

		// Use this for initialization
		void Start () {
			provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;

			origine = GameObject.FindGameObjectWithTag ("HDM");
			Lines = new ArrayList ();
			previousNumberOfLines = NumberOfLines;
			previousLineWith = lineWith;
			//We draw a first time lines
			drawLines ();
		}
		
		// Update is called once per frame
		void Update () {
			//When user is changing line width or distance between lines, we will instantly update the scene
			if ((NumberOfLines != previousNumberOfLines) || (previousLineWith != lineWith)) {
				//if there a modification, we erase all lines which are saved in the arrayList and clear the list
				foreach (GameObject line in Lines) {
					GameObject.DestroyImmediate(line);
				}
				Lines.Clear ();
				//We redraw new lines and update previous parameter variables
				drawLines();
				previousNumberOfLines = NumberOfLines;
				previousLineWith = lineWith;
			}
	
			//We check if there is any hands on the screen which are reach any line and if so, we will change the line color with texture2.
			Frame frame = provider.CurrentFrame;
			foreach (Hand hand in frame.Hands) {
			
				foreach (GameObject line in Lines) {
					
					LineRenderer lr = line.GetComponent<LineRenderer> ();
					if (hand.PalmPosition.z >= line.transform.position.z) {
						lr.material.mainTexture = texture2;
					} else {
						lr.material.mainTexture = texture1;
					}
				}
			}

		}

		//Function which draws all lines between the VR camera and the concerned obj according to distance Between line and line Witdh
		void drawLines(){

			float origineHeigh = origine.transform.position.y - ( origine.transform.position.y *0.15f);
			float origineForward = origine.transform.position.z - ( origine.transform.position.z *0.3f);

			//calculate the distance between lines depending on how much lines the user desires.
			float distanceBetweenLines = (float) ((obj.transform.position.z - origineForward) / ( NumberOfLines + 1) ) ;
			float HeighBetweenLines = (float) ((obj.transform.position.y - origineHeigh) / ( NumberOfLines + 1) ) ;
 
			//start and end means the two point to draw the line, here is for the first line near the camera
			Vector3 start = origine.transform.position + new Vector3( -0.2f , 0 , distanceBetweenLines );
			Vector3 end = origine.transform.position + new Vector3( 0.3f , 0 , distanceBetweenLines );
			start.y = origineHeigh;
			end.y = origineHeigh;

			//Now for each lines, we will create it and give a texture and draw it in the scene
			for (int i = 0; i < NumberOfLines; i++) {

				GameObject line = new GameObject();
				line.name = "line" + i;
				line.transform.position = start;
				line.AddComponent<LineRenderer> ();

				LineRenderer lr = line.GetComponent<LineRenderer> ();
				lr.material = new Material (Shader.Find ("Diffuse"));
				lr.material.mainTexture=  texture1;
				lr.startColor = Color.cyan;
				lr.endColor = Color.cyan;
				lr.startWidth = lineWith;
				lr.endWidth = lineWith;
				lr.SetPosition (0, start);
				lr.SetPosition (1, end);

				//We add the created line to the arrayList in order to be able to delete it later
				Lines.Add ( line );

				//we update the new position for the next line
				start.z += distanceBetweenLines;
				end.z += distanceBetweenLines;
				start.y += HeighBetweenLines;
				end.y += HeighBetweenLines;
			}
		}
			
	}
}
