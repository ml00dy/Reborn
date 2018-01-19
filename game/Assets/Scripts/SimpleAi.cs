using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]

public class SimpleAi : MonoBehaviour 
{
	public float maxSpeed = 7f;
	public float fov = 70f;
	public float rng = 5f;
	public Transform playerPosition;
	public bool isplayerVisible; 
	public bool seePlayer;
	public bool playerInRange;
	public float distanceToPlayer;
	public float angle;
	public float playerWasNotBeenSeenFor = 0;
	public Vector3 forward;
	public Vector3 pp;
	private Color distanceToplayerRay;
	public Vector2 lastPlayerPosition;
	public int Mask_wall;


// A*
	public float updateRate = 20f;
	private Seeker seeker;
	private Rigidbody2D rb;

	//Sciezka do gracza
	public Path path;

	//odległość między ai a punktem aby ai uznało że jest na miejscu
	public float nexWaypointDistane = 3f;


	[HideInInspector]
	public bool pathIsEnded = false;

	public ForceMode2D fMode;

	// Punkt do ktorego zmieza gracz
	private int currentWaypoint = 0;
	

	void Start () 
	{
		rotateInRandomDirection ();
		Mask_wall = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Walls"));
		Debug.Log (Mask_wall);


		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		
		if (playerPosition == null) {
		Debug.LogError ("No Player found? PANIC!");
		return;
		}
		
		// Start a new path to the target position, return the result to the OnPathComplete method
		seeker.StartPath (transform.position, playerPosition.position, OnPathComplete);
		//111111111111111111111111111111111111111111
		StartCoroutine (UpdatePath ());
	}

	IEnumerator UpdatePath()
	{
		
		if(playerPosition == null)
		{
			return false;
		}

		seeker.StartPath (transform.position, playerPosition.position, OnPathComplete);

		yield return new WaitForSeconds( 1f/updateRate );

		StartCoroutine(UpdatePath());

	}


	public void OnPathComplete(Path p)
	{
		//Debug.Log("Sciezka.  Czy wystapil blad:" + p.error);
		if(!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}

	void FixedUpdate () 
	{
		// Sprawdzenie czy gracz nie znajduje się za przeszkoda
		// float lenght = Mathf.Infinity;
		// RaycastHit2D canSeePlayer = Physics2D.Raycast(transform.position, transform.up, lenght, ~Mask_wall );
		//RaycastHit2D canSeePlayer = Physics2D.Linecast (transform.position, playerPosition.position, ~Mask_wall);
		RaycastHit2D seepalyer = Physics2D.Linecast (transform.position, playerPosition.position, Mask_wall);
		Debug.DrawLine(transform.position, playerPosition.position, Color.gray);
		Debug.DrawRay (transform.position, transform.up * 10000f, Color.yellow);
		angle = Vector2.Angle (transform.up, playerPosition.position - transform.position);


		if(seepalyer.collider != null && seepalyer.collider.gameObject.tag == "Player")
		{
						if (angle < fov * 0.5f) {
								enemySeePlayer ();
								isplayerVisible = true;
						}
			
		}
		else if(seepalyer.collider != null && seepalyer.collider.gameObject.tag == "Walls")
		{
			isplayerVisible = false;
			if (playerWasNotBeenSeenFor >=5 ) 
			{
				rotateInRandomDirection ();
			} 
			else
			{
				playerWasNotBeenSeenFor += Time.deltaTime;
			}
		}
	}
	
		


	
	void enemySeePlayer()
	{
		
		pp = playerPosition.position;
		forward = transform.position;
		playerInRange = false;
		seePlayer = false;
		
		// Obliczenie dystansu do gracza. Sprawdzenie czy znajduje się w zasięgu.
		distanceToPlayer = Vector3.Distance ((Vector2)transform.position, (Vector2)playerPosition.position);
		
		// Obliczenie konta przeciwnikiem a graczem. 
		//Debug.Log(angle);
		
		//Sprawdzenie czy gracz znajduje się w zasięgu wzroku przeciwnika oraz jego polu widzenia.	
		if (distanceToPlayer < rng) 
		{
			playerInRange = true;
			if(angle < fov*0.5f)
			{

				seePlayer = true;
				
				lastPlayerPosition = playerPosition.position;

				float z = Mathf.Atan2((playerPosition.position.y - transform.position.y), (playerPosition.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
				transform.eulerAngles = new Vector3 (0, 0, z);
				GetComponent<Rigidbody2D>().angularVelocity = 0;

				
				
				if(path == null)
				{
					return;
				}
				if(currentWaypoint >= path.vectorPath.Count)
				{
					if(pathIsEnded)
						return;


					//Debug.Log ("koniec sciezki");
					pathIsEnded = true;
					return;
				}
				pathIsEnded = false;

				/// kierunek do nastepnego punktu

				Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
				dir = dir * maxSpeed;

				rb.AddForce(dir, fMode);

				float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);


				if(dist < nexWaypointDistane)
				{
					currentWaypoint++;
					return;
				}





			
				
			}
		}
		
		
		
		// Debugi i pomocnicze//////////////////////////////////////////////////////////////
		
		 if (angle < fov*0.5f) 
		{
			distanceToplayerRay = Color.red;
		} 
		else if(distanceToPlayer <= rng) 
		{
			distanceToplayerRay = Color.yellow;
		} else
		{
			distanceToplayerRay = Color.white;		
		}
		Debug.DrawLine (transform.position, playerPosition.position, distanceToplayerRay);
		Debug.DrawRay (transform.position, transform.up ,Color.green);
		Debug.DrawRay (playerPosition.position, playerPosition.up, Color.blue);
		if (lastPlayerPosition != Vector2.zero) 
		{
			Debug.DrawLine(transform.position, lastPlayerPosition, Color.cyan);
		}
		///////////////////////////////////////////////////////////////////////////////////
		 /// 
	}
	
	void rotateInRandomDirection()
	{
		playerWasNotBeenSeenFor = 0; 
		float randomDirection = (float)Random.Range (0, 360);
		Debug.LogWarning (randomDirection);
		Debug.Log ("Rotation!!!!!!!!!");
		transform.eulerAngles = new Vector3 (0, 0, randomDirection);
	}
	
}
