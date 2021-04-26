using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Written by Charlie Cook
public class ShedScript : MonoBehaviour
{
	public VirScript player;
	public GameObject wheatHarvestedProgenitor;
	public MessorScript messor;

	// Disencumbers the player
	private void OnTriggerEnter(Collider col) {
		if (col.name == "Sickle") {
			for (int i = 0; i < SickleScript.wheatCount; i++) {
				GameObject newWheat = Instantiate(wheatHarvestedProgenitor);
				newWheat.GetComponent<Transform>().Translate(
					new Vector3(Random.Range(-3.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))
				);
				newWheat.SetActive(true);
			}
			SickleScript.siloCount += SickleScript.wheatCount;
			SickleScript.wheatCount = 0;
			player.printToHUD("[You have harvested " + SickleScript.siloCount + " total wheat.]");

			if (SickleScript.siloCount >= 120 && !messor.getRevealed()) {
				messor.doReveal();
			} else if (!messor.getRevealed()) {
				messor.commentateProgress();
				messor.resetDillyDally();
			}
		}
	}

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}
}
