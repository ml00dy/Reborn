using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public float Height;



	void Start () {
		Height = Camera.main.orthographicSize;
	}
	

	void Update () 
	{
		if(Input.GetKey(KeyCode.F10) == true && Camera.main.orthographicSize >= 2f)
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize - 0.25f;
		}
		
		if(Input.GetKey(KeyCode.F11) == true && Camera.main.orthographicSize <= 5.5f)
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize + 0.25f;
		}

	}
}