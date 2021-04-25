using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessorScript : MonoBehaviour
{
	public GameObject[] cards = new GameObject[5];
	public VirScript player;

	public static string welcomeMessage = "So, you're finally awake pilgrim. Splendid!";
	public static string[] introMessages = {
		"Be not alarmed from not seeing anyone. I am Messor, he who reaps.",
		"Not souls, pilgrim, grain; And there is plenty here for you and your kind.",
		"But it will do you no good lying about in the fields. They await you, pilgrim.",
		"When you are ready, proceed to the shed to the south and find my instrument..."
	};
	public static string[] dillyDallyIntro = {
		"Linger not longer, pilgrim. The grain awaits. Find my instrument.",
		"The grain continues to wait, pilgrim. Hurry to the shed!",
		"You'll find little else to do with your time here, pilgrim. Come now, find my instrument."
	};
	public static string[] dillyDallyWorking = {
		"Linger not longer, pilgrim. The grain awaits harvesting.",
		"The grain continues to wait, pilgrim. Swing swiftly my instrument!",
		"You'll find little else to do with your time here, pilgrim. Come now, reap the harvest."
	};

	private bool introduced;
	private bool firstSiloCheck;
	private bool revealed;
	private IEnumerator dillyDallyCoRoVar;
	private static string key = "Messor";

	private IEnumerator welcome() {
		yield return new WaitForSeconds(2.0f);

		player.lockPrinting(key);
		player.printToHUD(welcomeMessage, 6.0f, key);
		for (int i = 0; i < 4; i++) {
			yield return new WaitForSeconds(6.0f);
			player.printToHUD(introMessages[i], 6.0f, key);
		}

		yield return new WaitForSeconds(4.0f);
		introduced = true;
		player.unlockPrinting();
		resetDillyDally();
	}

	private IEnumerator dillyDally() {
		yield return new WaitForSeconds(30.0f);

		if (firstSiloCheck) {
			player.printToHUD(dillyDallyIntro[Random.Range(0, 3)]);
		} else if (!revealed) {
			player.printToHUD(dillyDallyWorking[Random.Range(0, 3)]);
		}

		if (!revealed) resetDillyDally();
	}

	public void resetDillyDally() {
		if (dillyDallyCoRoVar != null) StopCoroutine(dillyDallyCoRoVar);
		dillyDallyCoRoVar = dillyDally();
		StartCoroutine(dillyDallyCoRoVar);
	}

    // Start is called before the first frame update
    void Start() {
		introduced = false;
		firstSiloCheck = true;
		revealed = false;
		StartCoroutine(welcome());
	}

    // Update is called once per frame
    void Update() {}
}
