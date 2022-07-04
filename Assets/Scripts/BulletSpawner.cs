using UnityEngine;

public class BulletSpawner : MonoBehaviour {
    private Bullet _tempBullet;
    private BulletProperties _tempProp;
    private long[] _timer;

    private void Movement() {
        transform.position = 
            new Vector3(3f * Mathf.Cos(Mathf.Deg2Rad * _timer[2]/5f), 
                        3f * Mathf.Sin(_timer[2]/5f * Mathf.Deg2Rad),
                        0);
    }
    private void Spawn() {
        //Debug.Log("Spawning");
        for (int i = 1; i <= 10; i ++) {
            float degree;
            Vector3 direction;
            //pick a bullet out of the pool
            _tempBullet = BulletManager.Manager.BulletActivate();
            if (_tempBullet == null) Debug.Log("NullRef!!");
            //TODO: It do happen in rare occasions, figure it out later.
            
            //some necessary calculations.
            degree = (i * 36 + _timer[2]) * Mathf.Deg2Rad;
            direction = 
                new Vector3(Mathf.Cos(degree), Mathf.Sin(degree),0f);
            //Debug.Log(_tempProp.Direction.ToString());
            
            //TODO: 1. Package these up.
            //fill in the initial properties
            //**Remember to initialize it before use.**
            //fill in the index of the bullet
            _tempProp.bullet = _tempBullet;
            _tempProp.radius = 0.1f;
            _tempProp.direction = direction;
            _tempProp.worldPosition = transform.position + 1f * (Vector3)direction;
            _tempProp.speed = 0f;
            _tempProp.color = Color.HSVToRGB(i/10f, 0.5f, (_timer[1] + 150f)/300f);
            _tempProp.spawnTime = _timer[0];
            
            //initialize the bullet
            BulletManager.Manager.BulletRefresh(_tempBullet, _tempProp);
            
            //register the target event
            _tempBullet.StepEvent += BulletManager.Manager.Step0_0;
        }
    }

    private void ResetTimerAll() {
        for (int i = 0; i < 16; i++) {
            _timer[i] = 0;
        }
    }

    private void Start() {
        //**One time for all**
        _tempProp = new BulletProperties();
        _timer = new long[16];
        ResetTimerAll();
    }

    void Update() {
        _timer[0]++;
        _timer[1]++;
        _timer[2]++;
        Movement();
        if (_timer[1] >= 150) {
            _timer[1] = 0;
        }

        if (_timer[1] % 20 == 0) {
            Spawn();
        }
    }
}
