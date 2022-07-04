using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Particle : MonoBehaviour {
    private int timer = 0;
    public SpriteRenderer render;

    private void Start() {
        timer = (int)(Random.value * 1440f);
    }

    private void Update() {
        timer++;
        var scale = 0.08f*(1.6f + 0.25f * Mathf.Sin(timer / 30f * Mathf.Deg2Rad));
        transform.localScale = new Vector3(scale, scale, 0);
        render.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(timer / 10f * Mathf.Deg2Rad))*0.75f);
    }

    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }
}
