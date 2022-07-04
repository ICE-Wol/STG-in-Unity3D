using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonlight : MonoBehaviour
{
    private Mesh mesh;
    private int timer;
    private MeshFilter _meshFilter;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    
    void Start() {
        mesh = new Mesh();
        timer = 0;

        vertices = new Vector3[37 * 7];
        uv = new Vector2[37 * 7];
        triangles = new int[36 * 6 * 2 * 3];

        float radius1 = 1f;
        float radius2 = 7f;
        float degree = 0f;
        for (int i = 0; i <= 36; i += 1) {
            for (int j = 0; j <= 6; j += 1) {
                degree = Mathf.Deg2Rad * (i * 5);
                vertices[(i + j * 36) * 2] = new Vector3(radius1 * Mathf.Cos(degree), radius1 * Mathf.Sin(degree), 0);
                vertices[(i + j * 36) * 2 + 1] = new Vector3(radius2 * Mathf.Cos(degree), radius2 * Mathf.Sin(degree), 0);

                uv[i * 2] = new Vector2(i / 180f, 1);
                uv[i * 2 + 1] = new Vector2(i / 180f, 0);
                if (i == 180) continue;
                triangles[i * 6] = i * 2 + 1;
                triangles[i * 6 + 1] = i * 2;
                triangles[i * 6 + 2] = (i + 1) * 2;
                triangles[i * 6 + 3] = i * 2 + 1;
                triangles[i * 6 + 4] = (i + 1) * 2;
                triangles[i * 6 + 5] = (i + 1) * 2 + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = mesh;

    }
}
