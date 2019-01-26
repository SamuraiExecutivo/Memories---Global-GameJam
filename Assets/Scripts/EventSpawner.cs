﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour {

	public GameObject [] objectsEnemy;
	public float spawnTime;
	public Vector3 spawnPoint;
	public float spawnTimer = 0;

	// Use this for initialization
	void Start () {
		spawnTime = 12f;
	}
	
	void FixedUpdate () {
		spawnTimer += 1 * Time.deltaTime;

		if (spawnTimer > 1) {
			spawnTimer -= 0.15f * Time.deltaTime;
		}

		if (spawnTimer > spawnTime) {
			Spawn ();
			spawnTimer = 0;
		}  
	}

	void Spawn ()
     {
         spawnPoint.x = 10;
         spawnPoint.y = -1;
         Instantiate(objectsEnemy[Random.Range(0, objectsEnemy.Length - 1)], spawnPoint, Quaternion.identity);
     }
}