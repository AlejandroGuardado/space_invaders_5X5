using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour{
    public List<Bullet> bullets;
    public float speed;
    public Color color;
    [SerializeField]
    protected float cooldown;
    protected Pool<Bullet> pool;

    public bool CanShoot { get; private set; }
    public float Cooldown { get { return cooldown; } }

    private void Awake() {
        pool = new Pool<Bullet>(bullets);
        pool.Clear();
        CanShoot = true;
    }

    public void Clear() {
        StopAllCoroutines();
        pool.Clear();
        CanShoot = true;
    }

    public bool Fire(Vector2 position) {
        if (!CanShoot) return false;
        if (!OnFire(position)) return false;
        CanShoot = false;
        StartCoroutine(OnCooldown());
        return true;
    }

    public abstract bool OnFire(Vector2 position);

    private IEnumerator OnCooldown() {
        yield return new WaitForSeconds(cooldown);
        CanShoot = true;
    }
}