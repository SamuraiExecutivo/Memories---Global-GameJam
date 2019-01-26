﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuvemSpawner : MonoBehaviour {


	public GameObject [] objectsNuvem;
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
         spawnPoint.x = 11;
         spawnPoint.y = -1;
         Instantiate(objectsNuvem[Random.Range(0, objectsNuvem.Length - 1)], spawnPoint, Quaternion.identity);
     }
}