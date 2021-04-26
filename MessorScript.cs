using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Written by Charlie Cook
public class MessorScript : MonoBehaviour
{
	// Assign these to the non-active cards and the Player's CharacterController
	public GameObject[] cards = new GameObject[5];
	public VirScript player;
	public AudioSource narrator;

	public AudioClip welcomeAudio;
	public AudioClip[] introAudio = new AudioClip[4];
	public AudioClip[] taskAudio = new AudioClip[3];
	public AudioClip[] progressAudio = new AudioClip[3];
	public AudioClip[] revealAudio = new AudioClip[3];
	public AudioClip[] dillyAudio = new AudioClip[3];
	public AudioClip[] dallyAudio = new AudioClip[3];

	// Every message to be printed in the HUD
	public static string welcomeMessage = "So, you're finally awake pilgrim. Splendid!";
	// POSTREMO EXCITARE PEREGRINATOR. SPLENDIDUS!
	public static string[] introMessages = {
		"Be not alarmed from not seeing anyone. I am Messor, he who reaps.",
		// NON TREPIDARE VIDERE NEMO UNUS. EGO MESSOR, DEUS MESSORIS.
		"Not souls, pilgrim, grain; And there is plenty here for you and your kind.",
		// NON ANIMUS, PEREGRINATOR. FRUMENTUM. ET IBI ABUNDATIA HIC AD TE ET AMICI.
		"But it will do you no good lying about in the fields. They await you, pilgrim.",
		// IS NON PRODESSE TE IN AGRI. EA EXSPECTANT TE, PEREGRINATOR.
		"When you are ready, proceed to the shed to the south and find my instrument..."
		// QUANDO TU PARATUS, AD MERIDIANA TUGURIUM ET INVENIRE MEUS INSTRUMENTUM.
	};
	public static string[] taskMessages = {
		"Very good, pilgrim. You are ready for your task.",
		// PERBONUM PEREGRINATOR. TE PARATUS AD OPUS.
		"Reap for me CENTVM ET VIGINTI stalks of wheat,",
		// MESSIS AD MEUM --- CULMOS TRITICUM,
		"Then I shall reveal to you a minor wonder..."
		// TUM EGO APERIRE AD TE PARVUS MIRACULUM.
	};
	public static string[] progressMessages = {
		"Good work, pilgrim. Keep to your pace.",
		// BONUM LABOR, PEREGRINATOR. TENERE TUUS VELOCITAS.
		"Still more reaping remains, pilgrim.",
		// ADHUC PLUS MESSIS RELIQUIAE, PEREGRINATOR.
		"Your tally grows, pilgrim. Soon it will be satisfactory."
		// TUUS RATIO AUGERIS, PEREGRINATOR. MOX ERIT SATIS.
	};
	public static string[] revealMessages = {
		"Enough! You have labored enough, pilgrim.",
		// SATIS! TU LABORAVERAS SATIS, PEREGRINATOR.
		"Return now to the main hall, and gaze upon the log table;",
		// REFERRE NUNC AD PRIMUM ATRIUM ET INTUERI SUPER STIPES MENSA.
		"My wonder awaits you there. Farewell, pilgrim."
		// MEUM MIRACULUM EXPECTAT IBI TIBI. SALVETE, PEREGRINATOR.
	};
	public static string[] dillyDallyIntro = {
		"Linger not longer, pilgrim. The grain awaits. Find my instrument.",
		// NO MORARI LONGIUS, PEREGRINATOR. FRUMENTA EXSPECTANT. INVENIAM MEUM INSTRUMENTUM.
		"The grain continues to wait, pilgrim. Hurry to the shed!",
		// FRUMENTA CONTINUES EXSPECTARE, PEREGRINATOR. ACCELERARE AD TUGURIO!
		"You'll find little else to do with your time here, pilgrim. Come now, find my instrument."
		// INVENIAM MODICUM ALIUM FACERE CUM TUUM TEMPUS, PEREGRINATOR.
		// VENIRE NUNC, INVENIAM MEUM INSTRUMENTUM.
	};
	public static string[] dillyDallyWorking = {
		"Linger not longer, pilgrim. The grain awaits harvesting.",
		// NO MORARI LONGIUS, PEREGRINATOR. FRUMENTA EXSPECTANT METI.
		"The grain continues to wait, pilgrim. Swing swiftly my instrument!",
		// FRUMENTA CONTINUES EXSPECTARE, PEREGRINATOR. AGITARE CITO MEUM INSTRUMENTUM!
		"You'll find little else to do with your time here, pilgrim. Come now, reap the harvest."
		// INVENIAM MODICUM ALIUM FACERE CUM TUUM TEMPUS, PEREGRINATOR.
		// VENIRE NUNC, MESSIS SEGES.
	};
	// Control and State variables
	private bool introduced;
	private bool firstSicklePickup;
	private bool revealed;
	private IEnumerator majorCoRoVar;
	private IEnumerator dillyDallyCoRoVar;
	private IEnumerator progressCoRoVar;
	private static string key = "Messor";
	// Audio Playing Wrapper
	private void playLine(AudioClip line) {
		narrator.Stop();
		narrator.clip = line;
		narrator.Play();
	}
	// Initial routine (invoked from Start())
	private IEnumerator welcome() {
		yield return new WaitForSeconds(3.0f);

		player.lockPrinting(key); // Locking prevents prints from other events ruining the narration
		playLine(welcomeAudio);
		player.printToHUD(welcomeMessage, welcomeAudio.length, key);
		yield return new WaitForSeconds(welcomeAudio.length + 0.5f);
		for (int i = 0; i < 4; i++) {
			playLine(introAudio[i]);
			player.printToHUD(introMessages[i], introAudio[i].length, key);
			yield return new WaitForSeconds(introAudio[i].length + 0.5f);
		}

		yield return new WaitForSeconds(3.0f);
		introduced = true;
		player.unlockPrinting();
		resetDillyDally();
	}
	// Task description routine (invoked when the sickle is first picked up, see SickleScript)
	private IEnumerator task() {
		player.lockPrinting(key);

		for (int i = 0; i < 3; i++) {
			playLine(taskAudio[i]);
			player.printToHUD(taskMessages[i], taskAudio[i].length, key);
			yield return new WaitForSeconds(taskAudio[i].length + 0.5f);
		}

		yield return new WaitForSeconds(1.5f);
		player.unlockPrinting();
		resetDillyDally();
	}
	// Wheat tally routine (just some flavor text)
	private IEnumerator progress() {
		if (revealed) yield break;
		int i = Random.Range(0, 3);
		yield return new WaitForSeconds(progressAudio[i].length / 2);
		playLine(progressAudio[i]);
		player.printToHUD(progressMessages[i], progressAudio[i].length);
	}
	// Reveal routine (invoked when more than 120 wheat has been deposited to the shed/silo)
	private IEnumerator reveal() {
		player.lockPrinting(key);
		yield return new WaitForSeconds(3.0f);

		for (int i = 0; i < 5; i++) { // Activates the cards that lead to other scenes
			if (cards[i] != null) cards[i].SetActive(true);
		}

		for (int i = 0; i < 3; i++) {
			playLine(revealAudio[i]);
			player.printToHUD(revealMessages[i], revealAudio[i].length, key);
			yield return new WaitForSeconds(revealAudio[i].length + 0.5f);
		}

		yield return new WaitForSeconds(3.0f);
		player.unlockPrinting();
	}
	// Reminder to the player to play the game
	// (happens every 30 seconds if no events occur here or in other scripts)
	private IEnumerator dillyDally() {
		if (revealed) yield break;
		int i = Random.Range(0, 3);
		yield return new WaitForSeconds(30.0f);

		if (firstSicklePickup) {
			playLine(dillyAudio[i]);
			player.printToHUD(dillyDallyIntro[i], dillyAudio[i].length);
		} else if (!revealed) {
			playLine(dallyAudio[i]);
			player.printToHUD(dillyDallyWorking[i], dallyAudio[i].length);
		}

		if (!revealed) resetDillyDally(); //"Tail Recursive" call to queue up the next run
	}
	// Resets the reminder coroutine above
	public void resetDillyDally() {
		if (dillyDallyCoRoVar != null) StopCoroutine(dillyDallyCoRoVar);
		dillyDallyCoRoVar = dillyDally();
		StartCoroutine(dillyDallyCoRoVar);
	}
	// Returns the state of the flag tracking if the sickle has been picked up at all
	public bool getFirstSicklePickup() { return firstSicklePickup; }
	// Invokes the task description coroutine, changes state appropriately
	public void delegateTask() {
		firstSicklePickup = false;
		StopCoroutine(majorCoRoVar);
		player.unlockPrinting();
		majorCoRoVar = task();
		StartCoroutine(majorCoRoVar);
	}
	// Invokes the progress coroutine
	public void commentateProgress() {
		if (progressCoRoVar != null) StopCoroutine(progressCoRoVar);
		progressCoRoVar = progress();
		StartCoroutine(progressCoRoVar);
	}
	// Returns the state of the flag tracking if enough wheat has been harvested
	public bool getRevealed() { return revealed; }
	// Invokes the reveal coroutine, changes state to terminal configuration
	public void doReveal() {
		revealed = true;
		StopCoroutine(majorCoRoVar);
		player.unlockPrinting();
		majorCoRoVar = reveal();
		StartCoroutine(majorCoRoVar);
	}

    // Start is called before the first frame update
    void Start() {
		// Set initial state flags
		introduced = false;
		firstSicklePickup = true;
		revealed = false;
		// Invoke first coroutine
		majorCoRoVar = welcome();
		StartCoroutine(majorCoRoVar);
	}

    // Update is called once per frame
    void Update() {}
}
