using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private GameObject[] instantiateList;
    private int _numGraze;
    public int NumGraze {
        set;
        get;
    }
    
    private int _numHit;
    public int NumHit {
        set;
        get;
    }
    private void Awake() {
        Manager = this;
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        foreach (var inst in instantiateList) {
            if(inst != null) Instantiate(inst);
        }
        BulletManager.Manager.Player = PlayerController.Controller;
    }

    #region Debug
    private void Update() {
        textMesh.text
            = "Graze: " + NumGraze + "\n" + "Hit: " + NumHit;

    }
    #endregion
}
