using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;
    [SerializeField] private TextMeshProUGUI textMesh; 
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


    #region Debug
    public GameObject test;
    public PlayerController testplayer;
    private Bullet textbullet;
    private void Update() {
        textbullet = test.GetComponent<Bullet>();
        BulletProperties bp = new BulletProperties();
        bp.bullet = textbullet;
        bp.color = Color.red;
        bp.worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        textbullet.Prop = bp;
        textMesh.text
            = "Graze: " + NumGraze + "\n" + "Hit: " + NumHit +
              "Dis3 " + ((Vector2)testplayer.transform.position - (Vector2)textbullet.transform.position).magnitude +
              "\n Dis2 " + (testplayer.ScreenPosition - textbullet.Prop.ScreenPosition).magnitude;

    }
    #endregion
}
