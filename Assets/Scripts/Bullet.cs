using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

//TODO: the properties need to be modified during movement, if its not, move it out
/// <summary>
/// The basic properties of a bullet.
/// It has to be the struct because it only copy its value while the class will transfer its address.
/// So, if u want to use a constant temporary argument, it has to be a struct. 
/// </summary>

[Serializable]
public struct BulletProperties {
    [Header("Basic Properties")]
    public float radius;
    public int type;
    public Color color;
    public Vector3 worldPosition;
    public float speed;
    public Vector3 direction;
    public float acceleration;
    public float rotation;
    
    [Header("Modify these values only when necessary.")]
    public Bullet bullet;
    public long spawnTime;
    public Bullet previous;
    public Bullet follow;

    /// <summary>
    /// Return the screen position of the bullet in vector2.
    /// if <c>Camera.main == null</c>, the default value of vector2 will be returned instead. 
    /// </summary>
    public Vector2 ScreenPosition
        => Camera.main != null ?
            Camera.main.WorldToScreenPoint(worldPosition) : default(Vector2);
}

enum BulletStates {
    Inactivated,
    Spawning,
    Activated,
    Destroying
};

public class Bullet : MonoBehaviour {
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private PlayerController player;
    [SerializeField] private BulletProperties prop;
    public BulletProperties Prop {
        get => prop;
        set {
            prop = value;
            render.color = value.color;
            transform.position = value.worldPosition;
        }
    }
    
    /// <summary>
    /// The state machine of the bullet.
    /// </summary>
    private BulletStates _states;

    /// <summary>
    /// Things being done per frame.
    /// </summary>
    public event Action<Bullet> StepEvent;
    
    /// <summary>
    /// Things being done when a bullet is destroyed including the basic effects of destruction.
    /// Will be executed only once after being triggered.
    /// </summary>
    public event Action<Bullet> DestroyEvent;

    private bool _grazed;
    public bool Grazed {
        set;
        get;
    }

    #region Abandoned

    //same as the Update() event. Doesnt worth the trouble.
    /*public void Step() {
        StepEvent?.Invoke(this);
    }*/
    //private void Destroy(){}

    #endregion

    /// <summary>
    /// To unregister the Step and Destroy event when a bullet is inactivated.
    /// Will be <b>immediately</b> called by the func BulletInactivate(),
    /// So you dont need to call this by yourself.
    /// </summary>
    public void InactivateEvent() {
        StepEvent = null;
        DestroyEvent = null;
    }

    /// <summary>
    /// check the current bullet is on screen or not.
    /// If it leaves the screen, then inactivate it;
    /// </summary>
    private void CheckOnField() {
        Vector3 position = prop.ScreenPosition;
        if (!(position.x <= -64f) && !(position.x >= (Screen.width + 64f)) && !(position.y <= -64f) &&
            !(position.y >= (Screen.height + 64f))) return;
        InactivateEvent();
        BulletManager.Manager.BulletInactivate(this);
    }

    private void CheckState() {
        switch (_states) {
            case BulletStates.Spawning:
                break;
            case BulletStates.Activated:
                break;
            case BulletStates.Destroying:
                break;
            case BulletStates.Inactivated:
                break;
        }
    }
    
    /// <summary>
    /// Check the distance between the bullet and player.
    /// Alert that the z position will be ignored.
    /// </summary>
    private void CheckDistance() {
        float distance = ((Vector2)player.transform.position - (Vector2)prop.worldPosition).magnitude;
        if (!_grazed && (prop.radius + player.GrazeRadius) >= distance) {
            _grazed = true;
            GameManager.Manager.NumGraze += 1;
        }
        if (prop.radius + player.HitRadius >= distance) {
            GameManager.Manager.NumHit += 1;
            BulletManager.Manager.BulletInactivate(this);
        }
    }


    public Vector3 Position {
        set {
            transform.position = new Vector3(value.x,value.y,10f);
        }
        get {
            return transform.position;
        }
    }
    //In unity, Update is called once a frame,
    //so its ok to use update here to get a stable refresh rate.
    //On the contrast FixedUpdate will be executed more or less than once
    //according to the length of the frame.
    private void Update() {
        StepEvent?.Invoke(this);
        //CheckOnField();
        //transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
        CheckDistance();
    }

    private void OnDrawGizmos() {
        Gizmos.color = _grazed ? Color.green : Color.magenta;
        Gizmos.DrawWireSphere(prop.worldPosition,prop.radius);
        
    }
}
