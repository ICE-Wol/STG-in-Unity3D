using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Background : MonoBehaviour
{
    private int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        timer++;
        transform.localScale = new Vector3(transform.localScale.x,2f *(1f + 0.2f * Mathf.Sin(Mathf.Deg2Rad * timer / 20f)),0f);
    }
}
