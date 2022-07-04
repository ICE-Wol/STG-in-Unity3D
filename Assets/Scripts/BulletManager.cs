using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

//U should be informed that all classes which u define are implicitly derived from the class Object.
//And among all the types they are all value type except of the class Object and String.
//So if u use a struct here, u cant change '_parent.Section.Child = _child;'like this,
//because the property will only return a copy of the original struct instance,
//and the value in the Bullet object _parent is never changed.
//To avoid this u can simply just change the struct into a class,
//since it has quite a bit fields and functions, it wont do any harm to use a class instead.
//the structs are stored directly in the system stack, 
//as a result it will be faster for u to access it while it cost more memory to be stored.

//the class used for region division
#region Abandoned
//TEMP: currently unused
public class Link {
    public Link() {
        _parent = null;
        _child = null;
        _current = null;
    }
    private Bullet _parent;
    public Bullet Parent {
        set {
            _parent = value;
        }
        get {
            return _parent;
        }
    }

    private Bullet _child;
    public Bullet Child {
        set {
            _child = value;
        }
        get {
            return _child;
        }
    }

    private Bullet _current;
    public Bullet Current {
        set {
            _current = value;
        }
        get {
            return _current;
        }
    }

    //private Vector2Int _root;

    /// <summary>
    /// Update the root when the grid belonged is changed.
    /// </summary>
    public void RefreshRoot() {
        //_root = new Vector2Int((int) (_current.Prop.ScreenPosition.x / 256f),
        //        (int) (_current.Prop.ScreenPosition.y / 256f));
        //TODO: 1.change 256 into a parameter; 2. When the bullet is not activated it wont have any roots?
        //maybe i can simply make it a specified value as a marking.
    }
    
    /*public Bullet FindTail(Bullet cur) {
        Bullet next = cur.Section.Child;
        if (next != null) {
            return FindTail(next);
        }
        else {
            return cur;
        }
    }*/
    
    // Insert the node right below the root.
    // Call it after RefreshRoot().
    /*public void Insert() {
        //TODO: Reconstruct this, link list should be a seperated class from the node.
        Debug.Log(Root.ToString());
        if (BulletManager.Manager.BulletField[Root.x, Root.y].Child != null)
            _child = BulletManager.Manager.BulletField[Root.x, Root.y].Child;
    }*/
    
    /*public void Remove() {
        _parent.Section.Child = _child; 
        _child.Section.Parent = _parent;
        _parent = null;
        _child = null;
    }*/
}
#endregion

public class BulletManager : MonoBehaviour {
    public static BulletManager Manager;
    //using static can make it directly be accessed by BulletManager.bulletManager
    //without creating an instance of this class

    [SerializeField] private Bullet bulletPrefab;

    //the bullet prefab was filled in the blank
    //it actually get the address of the bullet component attached to the prefab
    //when the bullet instance(component) is created,
    //the system will automatically create a clone of the prefab for you
    [SerializeField] private GameObject bulletSpawner;

    //TODO: Temporarily initiated here, change it later.
    public GameObject Moon;
    public LaserHead Head;
    public LaserHead[] List;
    public int timer = 0;

    private PlayerController _player;
    public PlayerController Player { set; get; }

    private Stack<Bullet> _bulletPool;

    private Bullet _tempBullet;
    private BulletProperties _tempProp;

    #region StepEvents

    public void Step0_0(Bullet bullet) {
        _tempProp = bullet.Prop;
        _tempProp.speed += 0.03f;
        if (_tempProp.speed >= 4f)
            _tempProp.speed = 4f;

        //multiply it forward to make it faster
        _tempProp.worldPosition += Time.deltaTime * _tempProp.speed * _tempProp.direction;

        BulletRefresh(bullet, _tempProp);
        bullet.CheckOnField();
    }

    public void Step0_1(Bullet bullet) {
        _tempProp = bullet.Prop;
        _tempProp.speed += 0.03f;
        if (_tempProp.speed >= 3f)
            _tempProp.speed = 3f;

        //multiply it forward to make it faster
        _tempProp.worldPosition += Time.deltaTime * _tempProp.speed * _tempProp.direction;

        BulletRefresh(bullet, _tempProp);
        bullet.CheckOnField();
    }

    #endregion

    /// <summary>
    /// Creating Some bullets and add them to the bullet pool.
    /// </summary>
    /// <param name="num">The number of bullet to add,range 0 to 512</param>
    private void BulletPoolAdd(int num) {
        if (num < 0) {
            num = 0;
        }

        if (num > 512) {
            num = 512;
        }

        for (int i = 1; i <= num; i++) {
            _tempBullet = Instantiate(bulletPrefab);
            _tempBullet.gameObject.SetActive(false);
            _tempBullet.Player = _player;
            _bulletPool.Push(_tempBullet);
        }
    }

    /// <summary>
    /// Check the number of bullet in the pool
    /// </summary>
    /// <returns></returns>
    public int BulletPoolCheckSize() {
        return _bulletPool.Count;
    }

    /// <summary>
    /// Take a bullet out of the bullet pool and set it activated.
    /// </summary>
    /// <returns>the index of the activated bullet</returns>
    public Bullet BulletActivate() {
        if (_bulletPool.Count <= 16) {
            BulletPoolAdd(16);
        }

        _tempBullet = _bulletPool.Pop();
        _tempBullet.gameObject.SetActive(true);
        _tempBullet.Grazed = false;
        //_tempBullet.Renderer.enabled = true;
        return _tempBullet;
    }

    /// <summary>
    /// Recycle a bullet by inactivating it.
    /// </summary>
    /// <param name="bullet"></param>
    public void BulletInactivate(Bullet bullet) {
        bullet.transform.position = Vector3.zero;
        bullet.InactivateEvent();
        bullet.gameObject.SetActive(false);
        _bulletPool.Push(bullet);
    }


    // Insert the bullet into the bullet field for the first time where its root indicates its belonging.
    /// <summary>
    /// Inject the property struct into the bullet and refresh its states.
    /// Alert that this should be called after <c>BulletActivate()</c> <b>immediately</b>
    /// </summary>
    /// <param name="bullet">The index of bullet you want to refresh.</param>
    /// <param name="prop">The property of the bullet you want to inject.</param> 
    public void BulletRefresh(Bullet bullet, BulletProperties prop /*,Link link*/) {
        bullet.Prop = prop;
    }

    #region Abandoned

    //abandoned. bullet dealing with themselves are better
    //Update() is executed strictly one time a frame so there are no necessity to let the manager
    //deal with all the things which is not very object oriented.
    // Refresh the basic properties of all the bullet in active by traversing the bullet field.
    /*public void BulletRefreshAll() {
        Vector3 tempPos,tarPos;
        bool isOutsideField;
        float distance;
        foreach (Bullet temp in _bulletField) {
            tempPos = transform.position;
            // maybe use a function instead
            isOutsideField = tempPos.x < -64 || tempPos.x > Screen.width ||
                             tempPos.y < -64 || tempPos.y > Screen.height;
            if(isOutsideField){
                BulletInactivate(temp);
                temp.InactivateEvent();
                //dont forget to unregister the event
            }
            else {
                Step();
                //tarPos = player.transform.position;
                //distance = Vector3.Distance(tarPos, tempPos);
                // collision and graze check;
                //distance <= player.grazeRadius + bullet.radius;
                //countGraze += 1;bullet.tagGraze = true;
                //distance <= player.hitRadius + bullet.radius;

            }

        }
        /*for (int i = 0; i < 7; i++) {
            for (int j = 0; j < 5; j++) {
                for (Link link = _bulletField[i, j];link != null;link = link.Child.Section) {
                    _tempBullet = link.Current;
                    Debug.Log("Refreshing");
                    _tempBullet.Step();
                    int x = link.Root.x;
                    int y = link.Root.y;
                    if (x < 0 || x >= 7 || y < 0 || y >= 5) {
                        BulletInactivate(_tempBullet);
                        _tempBullet.InactivateEvent();
                    }
                    if (i != x || j != y) {
                        link.Remove();
                        link.RefreshRoot();
                        link.Insert();
                    }
                }
            }
        }
    }*/

    #endregion

    private void Awake() {
        //Singleton check
        if (Manager == null) Manager = this;
        else DestroyImmediate(this.gameObject);
    }

    private void Start() {
        _bulletPool = new Stack<Bullet>();
        _tempProp = new BulletProperties();
        BulletPoolAdd(512);
        Instantiate(bulletSpawner);
        bulletSpawner.transform.position = Moon.transform.position;
        List = new LaserHead[5];
        for (int i = 13; i <= 17; i++) {
            List[i - 13] = Instantiate(Head.gameObject).GetComponent<LaserHead>();
        }
    }

    private void Update() {
        timer++;
        for (int i = 13; i <= 17; i++) {
            var rad = Mathf.Deg2Rad * i * 18f;
            var dist = (10f + 2f * Mathf.Sin(Mathf.Deg2Rad * (i * 18f + timer)));
            var dir = new Vector3(Mathf.Cos(rad), Mathf.Sin((rad)), 0f);
            var o = new Vector3(0f, 9f, 0f);
            List[i - 13].transform.position = o + dist * dir;
        }
    }
}