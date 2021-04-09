using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickleScript : MonoBehaviour
{
	public static int wheatCount = 0;
	public static int siloCount = 0;
	public VirScript player;

	private void OnCollisionEnter(Collision col) {
		if (col.collider.name == "SiloPlaceholder") {
			siloCount += wheatCount;
			wheatCount = 0;
			player.printToHUD("You have harvested " + siloCount + " total wheat.");
		}
	}

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}
}
