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
    /*public GameObject test;
    public PlayerController testplayer;
    private Bullet textbullet;
    private void Update() {
        textbullet = test.GetComponent<Bullet>();
        BulletProperties bp = new BulletProperties();
        bp.radius = 0.2f;
        bp.bullet = textbullet;
        bp.color = Color.white;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bp.worldPosition = new Vector3(pos.x, pos.y, 0f);
        textbullet.Prop = bp;
        textMesh.text
            = "Graze: " + NumGraze + "\n" + "Hit: " + NumHit +
              "Dis3 " + ((Vector2)testplayer.transform.position - (Vector2)textbullet.transform.position).magnitude +
              "\n Dis2 " + (testplayer.ScreenPosition - textbullet.Prop.ScreenPosition).magnitude;

    }*/
    #endregion
}
