﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    enum Dash
    {
        Nope,
        Dashing,
        AfterDash,

    }



    Dash dashCon;
    AttackState attackCon;

    public float normalSpeed;
    public float currentSpeed;
    public float maxSpeed;
    public KeyBinding keys;

    // test fields
    public OnAttack onAttack;

    private float d_currentSpeed;
    private Vector3 mousePos;
    private OnUnitLook onUnitLook;
    private float time_dashTime;
    // Use this for initialization

    private Vector3 moveVector;
    private bool condition_facingRight;
    private float time_afterDash;


    void Start ()
    {
        keys = GetComponent<KeyBinding>();
        keys.Init();
        moveVector = new Vector3(0, 0, 0);
        d_currentSpeed = currentSpeed;
        //onUnitLook = gameObject.AddComponent<OnUnitLook>();
        onUnitLook = gameObject.GetComponent<OnUnitLook>();
        onUnitLook.SetTarget(mousePos);
        time_dashTime = 0;
        time_afterDash = 0;
        dashCon = Dash.Nope;
        onAttack.Init();
        attackCon = onAttack.GetAttackState();
    }

    void LateUpdate()
    {
        Control();
        Move();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        onUnitLook.RotationUpdate(mousePos);
        RotationControl();
    }

    void Control()
    {
        if (Input.GetKey(keys.GetKey("Up")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.y = 1;
        }
        else if (Input.GetKey(keys.GetKey("Down")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.y = -1;
        }
        else
        {
            moveVector.y = 0;
        }

        if (Input.GetKey(keys.GetKey("Left")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.x = -1;
        }
        else if (Input.GetKey(keys.GetKey("Right")) && onAttack.GetAttackState() != AttackState.Attacking)
        {
            moveVector.x = 1;
        }
        else
        {
            moveVector.x = 0;
        }

        if (Input.GetKey(keys.GetKey("Dash")) && dashCon == Dash.Nope)
        {
            dashCon = Dash.Dashing;
        }

        if (Input.GetKey(keys.GetKey("Attack1")) && onAttack.GetAttackState() == AttackState.Nope)
        {
            onAttack.SetAttackState(AttackState.Attacking);
        }
    }

    void Move()
    {
        transform.position += moveVector.normalized * currentSpeed * Time.deltaTime;

        switch (dashCon)
        {
            case Dash.Dashing:
                //if (moveVector.y != 0)
                //{
                //    transform.position += moveVector.normalized * currentSpeed * 2 * Time.deltaTime;
                //}
                //else
                //{
                //    //if (condition_facingRight)
                //    //{
                //    //    transform.position += new Vector3(1, 0, 0) * currentSpeed * 3 * Time.deltaTime;
                //    //}
                //    //else
                //    //{
                //    //    transform.position += new Vector3(-1, 0, 0) * currentSpeed * 3 * Time.deltaTime;
                //    //}

                //    transform.position += new Vector3(1, 0, 0) * currentSpeed * 3 * Time.deltaTime;

                //}

                transform.position += moveVector.normalized * currentSpeed * 3 * Time.deltaTime;

                break;

            case Dash.AfterDash:
                if (time_afterDash > 0.5f)
                {
                    time_afterDash = 0;
                    dashCon = Dash.Nope;

                }
                else
                {
                    time_afterDash += Time.deltaTime;
                }
                break;
        }

        DashCondition();

    }

    void DashCondition()
    {
        if (time_dashTime > 0.3f)
        {
            time_dashTime = 0;
            dashCon = Dash.AfterDash;
        }
        else
        {
            time_dashTime += Time.deltaTime;
        }
    }

    void RotationControl()
    {
        if (transform.localScale.x > 0) // nhìn về bên phải 
        {
            if (mousePos.x > transform.position.x)
            {
                condition_facingRight = true;
            }
        }
        else
        {
            if (mousePos.x < transform.position.x)
            {
                condition_facingRight = false;
            }
        }
    }

}
