﻿using UnityEngine;
using System;

/// <summary>
/// General character class for handling the physics and animations of individual characters
/// </summary>
/// Author: James Liu
/// Authored on 07/01/2015
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Character : GensoBehaviour {

    [SerializeField]
    private int maxJumps;

    [SerializeField]
    private AnimationCurve jumpPower;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    public int PlayerNumber { get; set; }
    public Transform RespawnPosition { get; set; }

    public float Height {
        get {
            return Collider ? Collider.height : 0;
        }
        protected set {
            if (Collider)
                Collider.height = value;
        }
    }

    public int JumpCount { get; private set; }

    protected virtual void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<CapsuleCollider>();
    }

    public void Jump()
    {
        Debug.Log("Jump?!");
        Rigidbody.AddForce(transform.up * 300);
        JumpCount++;
    }

    private void HandleMovement()
    {
        Vector3 movement = Rigidbody.velocity;
        movement.x = Mathf.Lerp(Rigidbody.velocity.x, Input.GetAxis("Horizontal") * 10, 0.1f);
        Rigidbody.velocity = movement;

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Update()
    {
        HandleMovement();
    }

}
