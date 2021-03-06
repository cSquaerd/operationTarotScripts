using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;
using Valve.VR.InteractionSystem;
// Written by Charlie Cook
public class VirScript : MonoBehaviour
{
	public SteamVR_Action_Vector2 walkAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("default", "Walk");
	// Assign the below Public Variables to Components from within the Unity Inspector
	public TextMeshProUGUI HUDText;
	public CharacterController playerCollider;
	public Transform playerCoordinates;
	public Transform cameraCoordinates; // read only
	// How long printed text is shown before it is cleared
	public float printToHUDClearDelay = 5.0f;
	// How long printed text must wait before being printed after printToHUD() is called
	public float printToHUDSetDelay = 0.125f;
	// How fast (meters per second?) the player can move
	public float moveSpeed = 6.0f;
	// Computational Variables
	private bool pendingClear = false;
	private bool printingLocked = false;
	private string printingKey;
	private IEnumerator setCoRoVar, clearCoRoVar;
	private Vector2 stick;
	private Vector3 movement;
	private Vector3 colliderSync;
	private Quaternion cameraOrientation;
	private float cameraAngle = 0f;
	private float sinCA = 0f;
	private float cosCA = 0f;
	private float sinStick = 0f;
	private float cosStick = 0f;
	private float vY = 0f;
	private const float g = -9.81f;
	// Delayed HUD Clearing Function
	private IEnumerator clearHUD(float delay) {
		yield return new WaitForSeconds(delay + printToHUDSetDelay);
		HUDText.SetText("");
		pendingClear = false;
	}

	private IEnumerator setHUDText(string text) {
		yield return new WaitForSeconds(printToHUDSetDelay);
		HUDText.SetText(text);
	}

	// Printing Locking/Unlocking functions
	public void lockPrinting(string key) {
		printingLocked = true;
		printingKey = key;
	}
	public void unlockPrinting() {
		printingLocked = false;
	}
	// Print Function for Heads-Up Display Text
	public void printToHUD(string stuffToPrint, float delayOverride = 5.0f, string key = "") {
		if (printingLocked && key != printingKey) return;
		if (HUDText != null) {
			if ( // If no override is passed but the constant doesn't match the variable who does its job
				delayOverride == 5.0f
				&& delayOverride != printToHUDClearDelay
			) delayOverride = printToHUDClearDelay; // then sync up the override to the variable
			// Good God do I love semaphores/mutexes
			if (!pendingClear) pendingClear = true;
			else {
				StopCoroutine(setCoRoVar);
				StopCoroutine(clearCoRoVar);
				HUDText.SetText("");
			}

			setCoRoVar = setHUDText(stuffToPrint);
			clearCoRoVar = clearHUD(delayOverride);
			StartCoroutine(setCoRoVar);
			StartCoroutine(clearCoRoVar);

		} else print("[From printToHUD()]: " + stuffToPrint);
	}

	// Start is called before the first frame update
	void Start() {
		//HUDText = (TextMeshProUGUI) GameObject.Find("HUDText").GetComponent(typeof(TextMeshProUGUI));
		printToHUD("");
	}

	// Update is called once per frame
	void Update() {
		// Set the height of the CharCon to the height of the Head Mounted Display
		playerCollider.height = cameraCoordinates.localPosition.y;
		// Get the XZ coords of the HMD relative to CharCon (which is its parent)
		colliderSync = new Vector3(
			cameraCoordinates.localPosition.x,
			playerCollider.center.y,
			cameraCoordinates.localPosition.z
		);
		// Center the CharCon to the head
		playerCollider.center = colliderSync;

		// Gravity Update
		if (playerCollider.isGrounded) vY = 0f;
		else vY += g * Time.deltaTime;

		// Get the Y-Axis Rotation of the HMD
		cameraAngle = cameraCoordinates.eulerAngles.y;
		// Either make a Quaternion object with the cameraAngle (sane choice)
		cameraOrientation = Quaternion.Euler(new Vector3(0, cameraAngle, 0));

		// Or do some trigonometry (insane choice)
		// (The angle is negated because it increases clockwise in the world,
		// but trigonometric angles increase counter-clockwise (we consider
		// the stick's effective angle in trigonometric terms) )
/*		sinCA = Mathf.Sin(-(cameraAngle) * Mathf.Deg2Rad);
		cosCA = Mathf.Cos(-(cameraAngle) * Mathf.Deg2Rad);
*/
		// Get the control stick Vector2
		stick = walkAction.GetAxis(SteamVR_Input_Sources.Any);
		// Continuing the insane way, store the normalized (vector-magnitude = 1)
		// stick components as their trigonometric definitions
/*		sinStick = stick.normalized.y;
		cosStick = stick.normalized.x;
*/
		// Use the stick Vector2 to make a movement vector
		// and multiply it to the cameraOrientation (which is more or less a rotation matrix),
		// gravity inclusive (sane way)
		movement = cameraOrientation * new Vector3(
			stick.x * moveSpeed, // Side-to-side
			vY, // Gravity
			stick.y * moveSpeed // Forward & Back
		);

		// Or use the angle sum trig identities to manually calculate
		// the movement vector, gravity inclusive (insane way)
/*		movement = new Vector3(
			( (cosStick * cosCA) - (sinStick * sinCA) ) * moveSpeed, // cos(stickAngle + cameraAngle)
			vY,
			( (sinStick * cosCA) + (cosStick * sinCA) ) * moveSpeed // sin(stickAngle + cameraAngle)
		);
*/
		// Apply the movement vector with respect to time
		playerCollider.Move(movement * Time.deltaTime);
	}
}
