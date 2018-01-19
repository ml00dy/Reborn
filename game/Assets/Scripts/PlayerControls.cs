using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public float maxSpeed;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void FixedUpdate()
	{
		float moveVertical = Input.GetAxis("Vertical");
		float moveHorizontal = Input.GetAxis("Horizontal");

		//Kierunek w którym patrzy gracz
		var MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion rot = Quaternion.LookRotation(transform.position - MousePosition, Vector3.forward);
		transform.rotation = rot;
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		GetComponent<Rigidbody2D>().angularVelocity = 0;

		
		
		//Ruch gracza
			//Poruszanie sie w kierunku kursora
		maxSpeed = 15f;
		if(moveVertical != 0 )
		{
			if(moveVertical >0)
			{
				GetComponent<Rigidbody2D>().AddForce (gameObject.transform.up * maxSpeed * moveVertical);
			}
			if(moveVertical <0)
			{
				GetComponent<Rigidbody2D>().AddForce (gameObject.transform.up * (maxSpeed/3) * moveVertical);
			}
		}

			//Poruszanie się na boki (STRAFE)

		if(moveHorizontal != 0)
		{
			GetComponent<Rigidbody2D>().AddForce (gameObject.transform.right * maxSpeed * moveHorizontal);
		}






		

	}
}
