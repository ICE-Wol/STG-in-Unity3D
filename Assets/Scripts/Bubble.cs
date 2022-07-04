using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Bubble : MonoBehaviour {
    public int timer = 0;
    public float x;

    private void Start() {
        x = transform.position.x;
    }

    void Update() {
        timer++;
        Vector3 offset = new Vector3(0.2f * Mathf.Sin(Mathf.Deg2Rad * timer / 30f), - timer / 3000f, 0);
        transform.position -= offset;
        if(transform.position.x <= 0f)
            Destroy(this.gameObject);
    }
}
