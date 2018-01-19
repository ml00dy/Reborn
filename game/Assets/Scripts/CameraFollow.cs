using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform Target;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		// Kamera porusza się za graczem
		transform.position = new Vector3(Target.position.x, Target.position.y, -10f);
		
	}
}
