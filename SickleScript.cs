using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SickleScript : MonoBehaviour
{
	public SteamVR_Action_Boolean recoverAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "RecoverTool");
	public static int wheatCount = 0;
	public static int siloCount = 0;
	public Rigidbody sicklePhysics;
	public Interactable sickleInteractable;
	public VirScript player;

	private bool pressedOnce;
	private Vector3 inFront;
	private Vector3 sickleTeleportTo;
	private Quaternion cameraOrientation;
	private IEnumerator timeoutButtonCoRoVar;

	private void OnCollisionEnter(Collision col) {
		if (col.collider.name == "SiloPlaceholder") {
			siloCount += wheatCount;
			wheatCount = 0;
			player.printToHUD("You have harvested " + siloCount + " total wheat.");
		}
	}

	private IEnumerator timeoutButton() {
		yield return new WaitForSeconds(3.0f);
		pressedOnce = false;
	}

	private void recoverTool() {
		if (sickleInteractable.attachedToHand) {
			player.printToHUD("You're holding your sickle already!");
		} else if (pressedOnce) {
			StopCoroutine(timeoutButtonCoRoVar);
			cameraOrientation = Quaternion.Euler(
				new Vector3(0, player.cameraCoordinates.eulerAngles.y, 0)
			);
			inFront = new Vector3(0, player.playerCollider.height * 0.75f, 0.5f);
			print("in recoverTool: " + inFront + " : " + cameraOrientation * inFront);

			sickleTeleportTo = player.playerCoordinates.position + (cameraOrientation * inFront);
			sicklePhysics.position = sickleTeleportTo;
			//sicklePhysics.velocity.y = 5.0f;
			pressedOnce = false;
		} else {
			player.printToHUD(
				"Your sickle is located at " + sicklePhysics.position
				+ ".\nPress the recovery button again to teleport it in front of you."
			);
			pressedOnce = true;
			timeoutButtonCoRoVar = timeoutButton();
			StartCoroutine(timeoutButtonCoRoVar);
		}
	}

    // Start is called before the first frame update
    void Start() {
		pressedOnce = false;
	}

    // Update is called once per frame
    void Update() {
		if (recoverAction.GetStateDown(SteamVR_Input_Sources.Any)) recoverTool();
	}
}
