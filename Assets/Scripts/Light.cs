using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor.Tilemaps;
using UnityEngine;
using Random = UnityEngine.Random;

public class Light : MonoBehaviour {
    public GameObject moon;
    public SpriteRenderer render;
    public GameObject star;
    public Bubble bubble;
    public int timer = 0;
    // Update is called once per frame

    private void Start() {
        for (int i = 0; i <= 60; i++) {
            float x = Random.value * Screen.width;
            float y = Random.value * Screen.height;
            Vector2 pos2 = new Vector2(x, y);
            Vector3 pos = Camera.main.ScreenToWorldPoint(pos2);
            pos.z = 0f;
            GameObject temp = Instantiate(star);
            temp.transform.position = pos;
        }
            
    }

    void Update() {
        timer++;
        transform.position = moon.transform.position;
        var scale = (1.6f + 0.25f * Mathf.Sin(timer / 30f * Mathf.Deg2Rad));
        transform.localScale = new Vector3(scale, scale, 0);
        render.color = new Color(1, 1, 1, 0.9f + 0.25f * Mathf.Sin(timer / 30f * Mathf.Deg2Rad));


        /*if (timer % 40 == 0) {
            for (var i = 0; i <= 3; i++) {
                float x = Random.value * Screen.width;
                float y = 0;
                Vector2 pos2 = new Vector2(x, y);
                Vector3 pos = Camera.main.ScreenToWorldPoint(pos2);
                pos.z = 0f;
                GameObject temp = Instantiate(bubble.gameObject);
                temp.transform.position = pos;
                
            }
        }*/
    }
}
