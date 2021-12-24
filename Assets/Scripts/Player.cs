using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity{
    public SessionData sessionData;
    public GameEntityDissolver dissolver;
    public PlayerInputActions input;
    public float speed;
    public float damping;
    public Vector2 maxScaleDistort;
    public bool CanControl { get; private set; }

    private float move;
    private float currentMove;
    private float moveVelocity;
    private float MAX_POSITION;

    private void Awake() {
        input = new PlayerInputActions();
        input.Player.Move.performed += ctx => move = ctx.ReadValue<float>();
        input.Player.Move.canceled += _ => move = 0f;
        MAX_POSITION = sessionData.gridWidth > 0 ? sessionData.gridWidth / 2f : 0f;
    }

    private void Update() {
        if(Active && CanControl) {
            UpdateMovement();
            Distort();
        }
    }

    public override void Activate() {
        transform.localScale = Vector2.one;
        base.Activate();
    }

    private void UpdateMovement() {
        currentMove = Mathf.SmoothDamp(currentMove, move * speed, ref moveVelocity, damping);
        Vector2 position = transform.position;
        position.x += currentMove * Time.deltaTime;
        position = new Vector2(Mathf.Clamp(position.x, -MAX_POSITION, MAX_POSITION), position.y);
        transform.position = position;
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
        float distort = GetMovementDistort();
        Vector2 scale = Vector2.Lerp(Vector2.one, maxScaleDistort, distort);
        transform.localScale = scale;
    }

    private float GetMovementDistort() {
        return speed > 0 ? Mathf.Abs(currentMove / speed) : 0;
    }
}