using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShedScript : MonoBehaviour
{
	public VirScript player;

	// Disencumbers the player
	private void OnTriggerEnter(Collider col) {
		if (col.name == "Sickle") {
			SickleScript.siloCount += SickleScript.wheatCount;
			SickleScript.wheatCount = 0;
			player.printToHUD("You have harvested " + SickleScript.siloCount + " total wheat.");
		}
	}

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}
}
