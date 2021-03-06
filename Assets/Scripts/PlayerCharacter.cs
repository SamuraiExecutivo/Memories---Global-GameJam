﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour {

	private int age, contadorFasesDavida;
	private float jumpMultiplyer, speedMultiplyer, details, initialSpeed,
				  initialJump, speed, jump, moveLeft, infarto, stress,
				  canJump = 0f;

	private float timerScore;
	private int scoreTime;

	float vert, hori;
	private Rigidbody2D rb;
	private bool jumping 		= true;
	private bool eventWrapper 	= false;
	private bool esposa = false, diploma = false, guitarra = false, mae = false;
	private GameObject gameObj;
	public List<GameObject> Sound;
	public bool superPlayer = false;

	private Animator pAnimator;

	void Start () {
		inicializador();
	}
	
	void Update () {
        vert = Input.GetAxisRaw("Vertical");
        hori = Input.GetAxis("Horizontal");

		PlayerActions.ContadorEvento();
		transform.Translate (-moveLeft * Time.deltaTime, 0, 0);

		calculaScore();
	
		if (superPlayer) {
			if (Input.GetKeyDown(KeyCode.V)) age = 1;
			if (Input.GetKeyDown(KeyCode.B)) age = 2;
			if (Input.GetKeyDown(KeyCode.N)) age = 3;
			if (Input.GetKeyDown(KeyCode.M)) age = 4;
		}
		
		pAnimator.SetInteger 	("playerAge", age);
    }

	void FixedUpdate() {
		AgeChanger (age);
		if (!mae) {
			andaPlayer();
			verificaParadaNoPombo();

			if (PlayerActions.gameOverCond) moveLeft = 0;
			
			verificaInfarto();
		} else {
			rb.velocity = velocidade(0, 0);
			setAudioPlayer(false, false, false, false, true);
			ChangeAnimator(false, false, true, age);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "ground") jumping = false;
    }

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "ground") jumping 			= true;
		if (collision.gameObject.tag == "Event Trap") eventWrapper 	= false;

	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Event Trap") {
			ChangeAnimator (true, false, false, age);
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Event Trap") {
			gameObj = collision.gameObject;
			setPombosConditions(true, true, 0);
			ChangeAnimator (false, true, false, age);
		}
		
		if (collision.gameObject.tag == "Score") {
			ScoreActions.contador += Random.Range(5,10);
			Destroy(collision.gameObject);
		}
		
		if (collision.gameObject.tag == "Fases da Vida") {
			setFaseDaVida();
			collision.gameObject.tag = "Untagged";
		}
	}

	private void ChangeAnimator (bool walk, bool eventAct, bool dying, int changeAge) {
		pAnimator.SetBool 		("andar", 		walk);
		pAnimator.SetBool 		("eventAction", eventAct);
		pAnimator.SetBool 		("morrer", 		dying);
		pAnimator.SetInteger 	("playerAge", 	changeAge);

	}

	void setPombosConditions(bool evWrp, bool boolPombos, int resetCounter) {
		eventWrapper 				= evWrp;
		PlayerActions.boolTrap 	= boolPombos;
		PlayerActions.contador	 	= resetCounter;
	}

	bool verificaFasesDaVida() {
		if (age == 1) {
			return !esposa && !diploma && !guitarra && !mae;
		}else if (age == 2) {
			return esposa && !diploma && !guitarra && !mae;
		}else if (age == 3) {
			return esposa && diploma && !guitarra && !mae;
		}else if (age == 4) {
			return esposa && diploma && guitarra && !mae;
		}
		return mae;
	}

	int valorPombo() {
		return (PlayerActions.maxEventCount/((age/10)+1));
	}

	private Vector2 velocidade(float x, float y) {
		return new Vector2(x, y);
	}

	private void verificaInfarto() {
		if (!superPlayer) {
			if (stress <= infarto) {
				Debug.Log("Infarto Fulminante");
				ChangeAnimator (false, false, true, age);
				stress = 0;
				infarto = 0;
				PlayerActions.gameOverCond = true;
				setAudioPlayer(false, false, false, false, true);
				Invoke ("GameOverCondition", 4);
			} else {
				stress = Random.Range (0f, 1000000f);
				if (infarto < 10) infarto += 0.05f * Time.deltaTime;
			}
		}
	}

	private void andaPlayer() {
        if(!jumping && !eventWrapper && Time.time > canJump) rb.velocity = velocidade(hori * speed, vert * jump);
		else if (jumping) canJump = Time.time + 0.3f;
		else rb.velocity = velocidade(hori * speed, 0);
	}

	private void verificaParadaNoPombo() {
		if (eventWrapper) {
			rb.velocity = velocidade(0, 0);
			if (PlayerActions.contador == valorPombo()) {
				Destroy(gameObj);
				setPombosConditions(false, false, 0);
			}
		}
	}

	private void calculaScore() {
		if (eventWrapper && timerScore >= 0) {
			timerScore -= 1 * Time.deltaTime;
			scoreTime = (int) timerScore;
		} else {
			ScoreActions.contador += scoreTime;
			scoreTime = 0;
			timerScore = 20;
		}
	}

	private void setAudioPlayer(bool pri, bool duo, bool tri, bool tet, bool pen) {
		Sound[0].SetActive(pri);
		Sound[1].SetActive(duo);
		Sound[2].SetActive(tri);
		Sound[3].SetActive(tet);
		Sound[4].SetActive(pen);
	}

	private void setFaseDaVida() {
		if (contadorFasesDavida == 0 && verificaFasesDaVida()) {
			setAudioPlayer(false, true, false, false, false);
			setIdadeDaVida(2);
		} else if (contadorFasesDavida == 1 && verificaFasesDaVida()) {
			setAudioPlayer(false, false, true, false, false);
			setIdadeDaVida(3);
		} else if (contadorFasesDavida == 2 && verificaFasesDaVida()) {
			setAudioPlayer(false, false, false, true, false);
			setIdadeDaVida(4);
		} else if (contadorFasesDavida == 3 && verificaFasesDaVida()) {
			mae = true;
		}
			contadorFasesDavida++;
	}
	private void setIdadeDaVida(int idade) {
		age = idade;
		if (idade == 2) esposa = true;
		else if (idade == 3) diploma = true;
		else if (idade == 4) guitarra = true;
		else mae = true;
	}

	private void inicializador() {
		rb = GetComponent<Rigidbody2D>();
		age = 1;
		infarto = 0.5f;
		stress = 10;
		moveLeft = 0.5f;
		speed = initialSpeed = 1;
		jump = initialJump = 5;
		AgeChanger (1);
		ScoreActions.ResetaContador();
		contadorFasesDavida = 0;
		setAudioPlayer(true, false, false, false, false);
		timerScore = 20;
		pAnimator = GetComponent<Animator>();
	}

	void GameOverCondition () {
		SceneManager.LoadScene(2);
	}

	void AgeChanger (float playerAge) {
		Time.timeScale = playerAge * 1.2f;
		if (age == 1) SetPlayerCharacter(2f, 1f, 1);
		if (age == 2) SetPlayerCharacter(3f, 3f, 0.75f);
		if (age == 3) SetPlayerCharacter(4f, 3.5f, 0.5f);
		if (age == 4) SetPlayerCharacter(6f, 3.5f, 0.25f);
		if (PlayerActions.gameOverCond) SetPlayerCharacter(0f, 0f, 0f);
	}

    void SetPlayerCharacter(float speedAux, float jumpAux, float detailsAux){
        speedMultiplyer = speedAux;
        jumpMultiplyer 	= jumpAux;
        details 		= detailsAux;
		
		speed			= initialSpeed * speedMultiplyer;
		jump 			= initialJump * jumpMultiplyer;
    }
}