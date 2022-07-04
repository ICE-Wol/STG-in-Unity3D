using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;

public class LaserHead : MonoBehaviour {
    private MeshFilter _meshFilter;
    private MeshRenderer _renderer;
    private Mesh _mesh;
    private Bullet _nextBullet;
    private int length;

    private float _radius;
    private Vector3[] _vertices;
    private Vector2[] _uv;
    private int[] _triangles;
    private int _timer;
    private BulletProperties _tempProp;
    
    /// <summary>
    /// Get the number of fragments which the laser contains.
    /// </summary>
    /// <param name="current">Should always starts with <c>_nextBullet</c></param>
    /// <returns></returns>
    private int GetLength(Bullet current) {
        if (current.Next == null) return 1;
        else return GetLength(current.Next) + 1;
    }

    /// <summary>
    /// Add a bullet to the tail of the laser.
    /// </summary>
    /// <param name="current">Should always start with <c>_nextBullet</c>.</param>
    /// <param name="tail">The bullet you want to add.</param>
    private void AddTail(Bullet current,Bullet tail) {
        if (current == null) {
            _nextBullet = tail;
            return;
        }
        if (current.Next == null) {
            current.Next = tail;
            tail.Prev = current;
        }
        else {
            AddTail(current.Next,tail);
        }
    }
    
    /// <summary>
    /// Initialize a laser.
    /// </summary>
    /// <param name="length">Number of fragments which the laser contains.</param>
    private void Initialize() {
        _renderer.sortingLayerName = "Bullet";
        for (int i = 0; i < length; i++) {
            Bullet bullet = BulletManager.Manager.BulletActivate();
            _tempProp = bullet.Prop;
            bullet.Head = this;
            bullet.Order = i;
            _tempProp.radius = 0.15f;
            /*if (i == 0 || i == length - 1) _tempProp.radius /= 1.9f;
            if (i == 1 || i == length - 1) _tempProp.radius /= 1.8f;
            if (i == 2 || i == length - 2) _tempProp.radius /= 1.8f;
            if (i == 3 || i == length - 3) _tempProp.radius /= 1.75f;
            if (i == 4 || i == length - 4) _tempProp.radius /= 1.7f;
            if (i == 5 || i == length - 5) _tempProp.radius /= 1.6f;
            if (i == 6 || i == length - 6) _tempProp.radius /= 1.5f;
            if (i == 7 || i == length - 7) _tempProp.radius /= 1.4f;*/
            BulletManager.Manager.BulletRefresh(bullet,_tempProp);
            bullet.StepEvent += LaserStep0_0;
            AddTail(_nextBullet, bullet);
        }

        _vertices = new Vector3[length * 2];
        _uv = new Vector2[length * 2];
        _triangles = new int[(length - 1) * 2 * 3];
    }

    private void Refresh() {
        Bullet current = _nextBullet;
        for (int i = 0; i < length; i += 1) {
            Vector3 position = current.transform.position;
            _vertices[i * 2] = position + current.Prop.radius * current.CurHorDir;
            _vertices[i * 2 + 1] = position - current.Prop.radius * current.CurHorDir;
            //Debug.Log(_vertices[i * 2] + " " + _vertices[i * 2 + 1]);
            _uv[i * 2] = new Vector2(i / (float)(length - 1), 1f);
            _uv[i * 2 + 1] = new Vector2(i / (float)(length - 1), 0f);
            if (current.Next != null) {
                current = current.Next;
                _triangles[i * 6] = i * 2 + 1;
                _triangles[i * 6 + 1] = i * 2;
                _triangles[i * 6 + 2] = (i + 1) * 2;
                _triangles[i * 6 + 3] = i * 2 + 1;
                _triangles[i * 6 + 4] = (i + 1) * 2;
                _triangles[i * 6 + 5] = (i + 1) * 2 + 1;
            }
        }
        
        _mesh.vertices = _vertices;
        _mesh.uv = _uv;
        _mesh.triangles = _triangles;
        _meshFilter.mesh = _mesh;
    }

    private void LaserStep0_0(Bullet bullet) {
        _tempProp = bullet.Prop;
        var deg = Mathf.Deg2Rad * (_timer * 2f + bullet.Order * 2f);
        var pos = new Vector3(Mathf.Cos(deg), 1.5f*Mathf.Sin(deg), .0f);
        //var deg2 = bullet
        _tempProp.worldPosition = this.transform.position + 2f * pos;
        BulletManager.Manager.BulletRefresh(bullet,_tempProp);
    }

    private void Awake() {
        _meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _mesh = new Mesh();
        length = 120;
    }

    private void Start() {
        Initialize();
    }
    private void Update() {
        _timer += 1;
        Refresh();
    }
}
