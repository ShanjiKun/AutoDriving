using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class Car : MonoBehaviour {

	/* Description
	 * 1unit point = 5m in fact
	 * width of car is 0.4(x) => car's width is 2m.
	*/

	public enum TurnType
	{
		LEFT=0,
		RIGHT=1,
		FORWARD=2
	}

	public enum Direction
	{
		NORTH=0,
		EAST=270,
		SOUTH=180,
		WEST=90
	}

	public enum TurnToLane
	{
		EASY,
		MEDIUM,
		HARD
	}

	//	Public variable
	public bool runable;
	public float Speed_KMH;
	public float speed;
	public TurnType planTurnType;
	public Direction direction;
	public Street street;
	public GameObject motor;


	public float testTime = 0;
	private float fps = 0;
	public float fpsTest = 0;

	//	MARK: Private variable
	// For descrement speed
	private bool isDesSpeed;

	// For inscrement speed
	private bool isIncSpeed;

	// For Street
	private Street newStreet;

	// For turn
	public float TRPF; // Turn Rotate Per Frame. Good test: 1.7 for 10km/h
	private bool isTurn;
	private int prevDirection;
	private float turnRotate;

	//	For turn to other lane
	private bool isTurnToLane;
	private float turnToLaneRotate;

	//	For inertia stop
	private bool isInertiaStop;

	// For turn back lane
	public Lane currentLane;
	private bool isTurnBackLane;
	private float turnBackLaneRotate;

	//	For TrafficLights
	public TrafficLights trafficlights;

	//	For AI
	public bool isResolving;
	public string dec1;
	public string dec2;
	public string dec3;

	//	Constant
	private float numYperFrame = (float)1/540;	//	speed 1km/h move 1/540 Y in a frame

	// Call first
	void Awake()
	{
		//	Setup FPS
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 30;

		//	Setup Car run
		runable = false;

		//	Handle first state
		int carPos =  PlayerPrefs.GetInt("CarPos");
		Speed_KMH = PlayerPrefs.GetInt("Speed");

		runable = true;

		//	Handle training for system
//		bool isTrainingSuccess = AIManager.TrainSystem("", "", "");
//		if (isTrainingSuccess) {
//			print ("Training data ready!");
//			runable = true;
//		}

		//	Test train data
//		List<string> trainData = AIManager.testTrainSystem();
//		foreach (string line in trainData) {
//			print (line);
//		}
	}

	// Use this for initialization
	void Start ()
	{
		//	Initial
		isTurnBackLane = true;

		//	Setup current speed
		speed = Speed_KMH;

		//	Random Turn
//		randomTurnType();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Inertia Stop
		CARInertiaStop();

		// Turn easy to other lane
		CARTurnToLane();

		//	Check Turn
		CARTurn();

		//	Turn Back Lane
		HELPTurnBackLane();

		//	Check SlowDown
		CARSlowDown();

		//	Check FastUp
		CARFastUp();

		//	Check Run TrafficLights
		CARCheckRunWithTrafficLight();

		//	Handle Movement
		handleMovement();

		//	Testing
		testTime += Time.deltaTime;
		fps += 1;
		if (testTime >= 1) {
			fpsTest = fps;
			testTime = 0;
			fps = 0;
		}
	}

	//	Helpers
	void randomTurnType()
	{
		planTurnType = (TurnType)Random.Range(0, 3);
		print (planTurnType);
	}

	void setupTurnToLane(TurnToLane type)
	{
		if (isTurnToLane)
			return;

		//	Get turn rotate
		float rotate = street.turnRotate (currentLane, type);

		//	Calculate turn easy rotate
		switch(direction){
		case Direction.SOUTH:
		case Direction.WEST:
			{
				rotate = -rotate;
			}
			break;
		default:
			break;
		}

		//	Turn the car
		print ("Turn " + type + " " + rotate);
		transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z + rotate);

		//	Turn on
		isTurnToLane = true;
	}

	void CARCheckRunWithTrafficLight ()
	{
		if (trafficlights != null && (!runable || speed == 0f) && trafficlights.lightType == TrafficLights.LightType.GREEN) {
			runable = true;
			isIncSpeed = true;
			isResolving = false;
			isInertiaStop = false;	
		}
	}

	//
	//	Car controls
	//
	void handleMovement()
	{
		if (!runable || speed <= 0.0001f)
			return;

		// Get local position of MOTOR
		Vector3 position = motor.transform.localPosition;

		// Translate motor direct y
		motor.transform.localPosition = new Vector3 (position.x, position.y + speed*numYperFrame, position.z);

		// Fix car position = Motor position
		this.transform.position = new Vector3 (motor.transform.position.x, motor.transform.position.y, 0);

		// Back localPosition to origin pos
		motor.transform.localPosition = position;
	}

	void CARFastUp()
	{
		
		if (!isIncSpeed)
			return;
		
		float _1per60Speed = Speed_KMH / 60f;
		speed += _1per60Speed;

		if (speed >= Speed_KMH) {
			speed = Speed_KMH;
			isIncSpeed = false;
		}
	}

	void CARSlowDown()
	{
		
		if (!isDesSpeed)
			return;
		
		float _1per30Speed = Speed_KMH / 30f;
		speed -= _1per30Speed;

		float _2per3Speed = 2 * Speed_KMH / 3f;
		if (speed <= _2per3Speed) {
			isDesSpeed = false;
		}
	}

	void CARTurn()
	{
		if (!isTurn)
			return;

		// Rotate a turnRotate per frame
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + turnRotate);

		// If car rotate > 90, turn finish
		if (Mathf.Abs (transform.eulerAngles.z - (float)prevDirection) <= 90f)
			return;

		// Turn finished
		transform.eulerAngles = new Vector3(0, 0, (float)direction);
		street = newStreet;
		isTurn = false;

		// Back to mid lane
		 isTurnBackLane = true;
	}

	void CARInertiaStop()
	{
		if (!isInertiaStop)
			return;

		float _1per30Speed = Speed_KMH / 30f;
		speed -= _1per30Speed;
		if (speed <= 0f) {
			speed = 0f;
			runable = false;
			isInertiaStop = false;
		}
	}

	void CARTurnToLane()
	{
		if (!isTurnToLane)
			return;

		//	Go to midpoint of other lane
		Vector2 carPos = transform.position;

		switch(street.direction){
		case Street.Direction.VERTICAL:
			{
				if (currentLane == street.laneLeft) {
					Vector2 midpoint = street.laneRight.mid.transform.position;
					if (carPos.x > midpoint.x) {
						carPos.x = midpoint.x;
						currentLane = street.laneRight;
						CARFinishTurnToLane (carPos);
					}
				} else {
					Vector2 midpoint = street.laneLeft.mid.transform.position;
					if (carPos.x < midpoint.x) {
						carPos.x = midpoint.x;
						currentLane = street.laneLeft;
						CARFinishTurnToLane (carPos);
					}
				}
			}
			break;
		default:
			{
				if (currentLane == street.laneLeft) {
					Vector2 midpoint = street.laneRight.mid.transform.position;
					if (carPos.y < midpoint.y) {
						carPos.y = midpoint.y;
						currentLane = street.laneRight;
						CARFinishTurnToLane (carPos);
					}
				} else {
					Vector2 midpoint = street.laneLeft.mid.transform.position;
					if (carPos.y > midpoint.y) {
						carPos.y = midpoint.y;
						currentLane = street.laneLeft;
						CARFinishTurnToLane (carPos);
					}
				}
			}
			break;
		}

	}

	void CARFinishTurnToLane(Vector2 carPos)
	{
		isTurnToLane = false;
		transform.position = carPos;
		transform.eulerAngles = new Vector3 (0, 0, (float)direction);

		// Handle Decision 3
		if (dec3 == NaiveBayes.QD3_DUNG) {
			isInertiaStop = true;	
		} else {
			isResolving = false;
		}

		// Reset decisions
		dec1 = dec2 = dec3 = "";
	}

	void HELPTurnBackLane()
	{
		if (!isTurnBackLane)
			return;

		// Check street direction
		switch(street.direction){
		case Street.Direction.VERTICAL:
			{
				// Back to X
				Vector3 curPos = this.transform.position;
				Vector3 midPoint = currentLane.mid.transform.position;
				this.transform.position = new Vector3 (midPoint.x, curPos.y);
				break;
			}
		case Street.Direction.HORIZONTAL:
			{
				// Back to Y
				Vector3 curPos = this.transform.position;
				Vector3 midPoint = currentLane.mid.transform.position;
				this.transform.position = new Vector3 (curPos.x, midPoint.y);
				break;
			}
		default:
			break;
		}

		// Turn off Back to lane
		isTurnBackLane = false;
	}

	//
	//	MARK: Trigger Enemy Handle
	//
	void onTriggerEnemy(GameObject enemy)
	{
		if (isResolving || isInertiaStop)
			return;
		print (isInertiaStop);
		print (isResolving);
		isResolving = true;

		float dis = distanceToObject (enemy);
		dis = dis * 5f;

		int leftArea, rightArea, aheadArea, behindArea;
		if (currentLane == street.laneLeft) {
			leftArea = 5;
			rightArea = 0;
		} else {
			leftArea = 0;
			rightArea = 5;
		}
		aheadArea = 0;
		behindArea = 0;

		string strDistance = ((int)dis).ToString ();
		string strSpeed = ((int)speed).ToString ();
		string strLeft = leftArea.ToString ();
		string strRight = rightArea.ToString ();
		string strAhead = aheadArea.ToString ();
		string strBehind = behindArea.ToString ();

		print (strDistance + " " + strSpeed + " " + strLeft + " " + strRight + " " + strAhead + " " + strBehind);
//		List<string> results = NaiveBayes.AI( strDistance, strSpeed, strLeft, strRight, strAhead, strBehind);
		List<string> results = AIManager.getDecisions( strDistance, strSpeed, strLeft, strRight, strAhead, strBehind);
		if (results == null || results.Count <= 0) {
			print ("Unknown decision!!");
			return;
		}

		printResult (results);
		print ("_____________________________________");

		//	Reset prev decision
		dec1 = dec2 = dec3 = "";

		//	Get decision
		dec1 = results [0];
		if (dec1 == AIManager.QD1_GIAMREPHAI || dec1 == AIManager.QD1_GIAMRETRAI) {
			
			dec2 = results [1];
			dec3 = results [2];

			if (dec2 == AIManager.QD2_REGAP) {
				setupTurnToLane (TurnToLane.HARD);
				isInertiaStop = true;
			} else if (dec2 == AIManager.QD2_REVUA) {
				setupTurnToLane (TurnToLane.MEDIUM);
			} else {
				setupTurnToLane (TurnToLane.EASY);
			}

		} else if (dec1 == AIManager.QD1_DUNG) {
			isInertiaStop = true;
		} else if (dec1 == AIManager.QD1_TANGTOC) {
			runable = true;
			isIncSpeed = true;
			isResolving = false;
			isInertiaStop = false;
		} else if (dec1 == AIManager.QD1_TIEPTUC) {
			runable = true;
			isIncSpeed = true;
			isResolving = false;
			isInertiaStop = false;
		} else {
			print ("Can't make a decision");
			isInertiaStop = true;
		}
	}

	//
	//	MARK: Trigger Enemy Handle
	//
	void onTriggerTrafficLights(GameObject light)
	{
		if (isResolving || isInertiaStop)
			return;
		
		isResolving = true;

		float dis = distanceToObject (light);

		if (dis <= 0.5f) {
			speed = 0f;
			runable = false;
		} else {
			isResolving = false;
		}
	}

	//
	//	MARK: Detection handle
	//
	// AHEAD
	void AHEADHandleEnterCrossroads()
	{
			isDesSpeed = true;
			isIncSpeed = false;
	}

	void AHEADHandleEnterEnemy(GameObject enemy)
	{
		onTriggerEnemy (enemy);
	}

	void AHEADHandleStayEnemy(GameObject enemy)
	{
		onTriggerEnemy (enemy);
	}

	void AHEADHandleExitEnemy(GameObject enemy)
	{
		onTriggerEnemy (enemy);
	}

	void AHEADHandleExitCrossroads()
	{
		isIncSpeed = true;
		isDesSpeed = false;

		// Random new turn
//		randomTurnType ();
	}

	void AHEADHandleEnterTrafficLight (GameObject light) {

		trafficlights = light.GetComponentInParent<TrafficLights>();

		switch (trafficlights.lightType) {
		case TrafficLights.LightType.RED:
			{
				onTriggerTrafficLights (light);
			}
			break;
		default:
			{
				runable = true;
				isIncSpeed = true;
				isResolving = false;
				isInertiaStop = false;
			}
			break;
		}
	}

	void AHEADHandleStayTrafficLight (GameObject light) {

		trafficlights = light.GetComponentInParent<TrafficLights>();

		switch (trafficlights.lightType) {
		case TrafficLights.LightType.RED:
			{
				onTriggerTrafficLights (light);
			}
			break;
		default:
			{
				runable = true;
				isIncSpeed = true;
				isResolving = false;
				isInertiaStop = false;
			}
			break;
		}
	}

	// BODY
	void BODYHandleEnterLane(Collider2D other)
	{
		if (isTurn || other.gameObject == street.laneLeft.gameObject || other.gameObject == street.laneRight.gameObject)
			return;

		// Set Current Lane
		currentLane = other.GetComponent<Lane>();

		// Calculate Plan Turn Type
		switch(planTurnType){
		case TurnType.LEFT:
			{
				// Calculate rotate
				turnRotate = speed*TRPF / 10f;

				// Calculate new direction
				int newDirection = (int)direction + 90;

				// Back up direction
				prevDirection = (int)direction;

				// Check direction
				direction = newDirection == 360 ? (Direction)0 : (Direction)newDirection;

				// Start turn to direction
				isTurn = true;
				isTurnBackLane = false;
				isTurnToLane = false;
			}
			break;
		case TurnType.RIGHT:
			{	
				// Calculate rotate
				turnRotate = -speed*TRPF / 10f;
				
				// Calculate new direction
				int newDirection = (int)direction - 90;

				// Back up direction
				prevDirection = (int)direction == 0 ? 360 : (int)direction;

				// Check direction
				direction = newDirection == -90 ? (Direction)270 : (Direction)newDirection;

				// Start turn to direction
				isTurn = true;
				isTurnBackLane = false;
				isTurnToLane = false;
			}
			break;
		default:
			break;
		}
	}

	void BODYHandleEnterStreet(Collider2D other)
	{
		newStreet = other.GetComponent<Street> ();
	}
	// Gameover
	void BODYHandleEnterAnythingElse()
	{
		runable = false;
//		Scene scene = SceneManager.GetActiveScene();
//		SceneManager.LoadScene(scene.name);	
	}

	//
	//	Detection
	//
	//	DetectionEnter
	public void onDetectionEnter(Detection.Type type, Collider2D other){
		switch (type) {
		case Detection.Type.AHEAD:
			{
				switch (other.tag) {
				case "Crossroads":
					{
						AHEADHandleEnterCrossroads ();
					}
					break;
				case "Enemy":
					{
						AHEADHandleEnterEnemy (other.gameObject);
					}
					break;
				case "TrafficLights":
					{
						TrafficLights trafficlights = other.gameObject.GetComponentInParent<TrafficLights>();
						if (trafficlights.carDirection == direction) {
							AHEADHandleEnterTrafficLight (other.gameObject);
						}
					}
					break;
				default:
					break;
				}
				break;
			}
		case Detection.Type.FRONT:
			{
				break;
			}
		case Detection.Type.RIGHT:
			{
				break;
			}
		case Detection.Type.BOTTOM:
			{
				break;
			}
		case Detection.Type.LEFT:
			{
				break;
			}
		case Detection.Type.BODY:
			{
				switch (other.tag) {
				case "Crossroads":
					{
						break;
					}
				case "TrafficLights":
					{
						break;
					}
				case "Lane":
					{
						BODYHandleEnterLane (other);
						break;
					}
				case "Street":
					{
						BODYHandleEnterStreet (other);
						break;
					}
				default:
					{
						BODYHandleEnterAnythingElse ();
						break;
					}
				}
				break;	
			}
		}
	}

	//	DetectionStay
	public void onDetectionStay(Detection.Type type, Collider2D other){
		switch (type) {
		case Detection.Type.AHEAD:
			{
				switch (other.tag) {
				case "Enemy":
					{
						AHEADHandleStayEnemy (other.gameObject);
					}
					break;
				case "TrafficLights":
					{
						TrafficLights trafficlights = other.gameObject.GetComponentInParent<TrafficLights>();
						if (trafficlights.carDirection == direction) {
							AHEADHandleStayTrafficLight (other.gameObject);
						}
					}
					break;
				default:
					break;
				}
				break;
			}
		case Detection.Type.FRONT:
			{
				break;
			}
		case Detection.Type.RIGHT:
			{
				break;
			}
		case Detection.Type.BOTTOM:
			{
				break;
			}
		case Detection.Type.LEFT:
			{
				break;
			}
		}
	}

	//	DetectionExit
	public void onDetectionExit(Detection.Type type, Collider2D other){
		switch (type) {
		case Detection.Type.AHEAD:
			{
				switch (other.tag) {
				case "Crossroads":
					{
						AHEADHandleExitCrossroads ();
						break;
					}
				case "Enemy":
					{
						AHEADHandleExitEnemy (other.gameObject);
						break;
					}
				default:
					break;
				}
				break;
			}
		case Detection.Type.FRONT:
			{
				break;
			}
		case Detection.Type.RIGHT:
			{
				break;
			}
		case Detection.Type.BOTTOM:
			{
				break;
			}
		case Detection.Type.LEFT:
			{
				break;
			}
		}
	}

	// Utils
	float distanceToObject(GameObject objc)
	{
		Vector2 carPos = transform.position;
		Vector2 enePos = objc.transform.position;
		return Mathf.Sqrt (Mathf.Pow((carPos.x - enePos.x), 2) + Mathf.Pow((carPos.y - enePos.y), 2));
	}

	void printResult(List<string>results){
		if (results [0] == NaiveBayes.QD1_GIAMREPHAI || results [0] == NaiveBayes.QD1_GIAMRETRAI) {
			print ("Dec1: " + results [0]);	
			print ("Dec2: " + results [1]);	
			print ("Dec3: " + results [2]);	
		} else {
			print ("Dec1: " + results [0]);	
		}
	}
}
