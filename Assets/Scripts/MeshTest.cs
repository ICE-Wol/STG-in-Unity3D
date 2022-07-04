using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class MeshTest : MonoBehaviour {
    private Mesh mesh;
    private int timer;
    private MeshFilter _meshFilter;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    void Start() {
        mesh = new Mesh();
        timer = 0;

        vertices = new Vector3[181 * 2];
        uv = new Vector2[181 * 2];
        triangles = new int[180 * 2 * 3];

        float radius1 = 5f;
        float radius2 = 6f;
        float degree = 0f;
        for (int i = 0; i <= 180; i += 1) {
            degree = Mathf.Deg2Rad * (i * 2);
            vertices[i * 2] = new Vector3(radius1 * Mathf.Cos(degree), radius1 * Mathf.Sin(degree), 0);
            vertices[i * 2 + 1] = new Vector3(radius2 * Mathf.Cos(degree), radius2 * Mathf.Sin(degree), 0);

            uv[i * 2] = new Vector2(i / 180f, 1);
            uv[i * 2 + 1] = new Vector2(i / 180f, 0);
            if(i == 180) continue;
            triangles[i * 6] = i * 2 + 1;
            triangles[i * 6 + 1] = i * 2;
            triangles[i * 6 + 2] = (i + 1) * 2;
            triangles[i * 6 + 3] = i * 2 + 1;
            triangles[i * 6 + 4] = (i + 1) * 2;
            triangles[i * 6 + 5] = (i + 1) * 2 + 1;
        }
        /*//3 * 2 triangles

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 5, 0);
        vertices[2] = new Vector3(5, 5, 0);
        vertices[3] = new Vector3(5, 0, 0);
        //game space coordinate
        
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);
        //texture coordinate
        
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;
        //order of point to form triangles
        //make sure it is clockwise so that the front side of the sprite will be displayed
        //otherwise it will be the back side
        */
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = mesh;

    }

    private void Update() {
        //Debug.Log(Screen.width.ToString() + "  " + Screen.height.ToString());
        timer += 1;
        for (int i = 0; i <= 180; i += 1) {
            float u1, v1, u2, v2;
            u1 = (timer) / 180f ;
            if (u1 > 1) {
                u1 -= 1;
            }

            //v1 = 1 + timer / 180f;
            
            uv[i * 2] = new Vector2(u1, timer / 180f);
            uv[i * 2 + 1] = new Vector2(u1, (180 - timer) / 180f);
        }

        timer %= 180;

        _meshFilter.mesh.uv = uv;
    }
}
