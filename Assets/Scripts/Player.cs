using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : GameEntity{
    public BoxCollider2D box;
    public SessionData sessionData;
    public GameEntityDissolver dissolver;
    public PlayerInputActions input;
    public PlayerGunInventory gunInventory;
    public float speed;
    public float damping;
    public Vector2 maxScaleDistort;
    public Vector2 weaponFireOffset;
    public Shockwave shockwave;
    public bool CanControl { get; private set; }

    private float move;
    private float currentMove;
    private float moveVelocity;
    private float MAX_POSITION;
    private float shockwaveDuration;

    [HideInInspector]
    public UnityEvent<float> OnWeaponFired;

    private void Awake() {
        input = new PlayerInputActions();
        input.Player.Move.performed += ctx => move = ctx.ReadValue<float>();
        input.Player.Move.canceled += _ => move = 0f;
        input.Player.Fire.performed += ctx => Fire();
        MAX_POSITION = sessionData.gridWidth > 0 ? sessionData.gridWidth / 2f : 0f;
        shockwaveDuration = sessionData.playerFireShockwaveDuration;
    }

    private void Update() {
        if(Active && CanControl) {
            UpdateMovement();
            Distort();
        }
    }

    public override void Activate() {
        transform.localScale = Vector2.one;
        dissolver.Clear();
        base.Activate();
    }

    public override void Deactivate() {
        shockwave.Clear();
        move = 0f;
        currentMove = 0f;
        moveVelocity = 0f;
        base.Deactivate();
    }

    private void UpdateMovement() {
        currentMove = Mathf.SmoothDamp(currentMove, move * speed, ref moveVelocity, damping);
        Vector2 position = transform.position;
        position.x += currentMove * Time.deltaTime;
        position = new Vector2(Mathf.Clamp(position.x, -MAX_POSITION, MAX_POSITION), position.y);
        transform.position = position;
    }

    public void Fire() {
        bool fire = gunInventory.Fire((Vector2)transform.position + weaponFireOffset, out float cooldown);
        if (!fire) return;
        shockwave.CreateShockwave(shockwaveDuration);
        OnWeaponFired.Invoke(cooldown);
    }

    public void GainControl() {
        CanControl = true;
        input.Player.Enable();
    }

    public void RemoveControl() {
        CanControl = false;
        input.Player.Disable();
    }

    private void Distort() {
        float distort = Mathf.Max(GetMovementDistort(), shockwave.Current);
        Vector2 scale = Vector2.Lerp(Vector2.one, maxScaleDistort, distort);
        transform.localScale = scale;
    }

    public void Kill() {
        RemoveControl();
        StartCoroutine(OnKill());
    }

    public void OnMove(float move) {
        this.move = move;
    }

    private IEnumerator OnKill() {
        box.enabled = false;
        dissolver.Dissolve();
        yield return new WaitForSeconds(dissolver.dissolveTime);
        Deactivate();
    }

    private float GetMovementDistort() {
        return speed > 0 ? Mathf.Abs(currentMove / speed) : 0;
    }
}