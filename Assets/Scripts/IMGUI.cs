﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IMGUI : MonoBehaviour {

	public Texture btTexture;
	public Texture btCreditos;
	private float tx, ty;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		tx = Screen.width;
		ty = Screen.height;
	}

	void OnGUI() {
		if (GUI.Button(new Rect(3*tx/4, 3*ty/4 + 20, tx/5, ty/5), btTexture)) SceneManager.LoadScene(1);
		if (GUI.Button(new Rect(3*tx/4, 4*ty/6, tx/5, ty/10), btCreditos)) SceneManager.LoadScene(3);
	}
}
