using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // [SerializeField] private Animation _animation;
    [SerializeField] private Animator _animation;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public byte Rotation;


    private float _timePassedSinceLastShoot = 3;
    private float _timePassedSinceLastCheckPosition = 0;

    public static Hero Instance { get; set; }

    [SerializeField] private GameObject _heroPosition;
    private object _spawnBulletsPosition;
    [SerializeField] private GameObject _bullets;

    public int live = 3;

    public enum States
    {
        Hero_idle = 0,
        Hero_walking = 1,
        Hero_shooting = 2,
        Hero_dying = 3
    }

    public enum SharksDirection
    {
        right,
        left
    }

    static public SharksDirection WhereSharkLooking;

    [SerializeField] private Transform _position;
    [SerializeField] private Rigidbody2D _rigidbody2d;
    private Vector2 _movement;

    private void Walk()
    {
        int speed = 1;
        _movement.x = Input.GetAxis("Horizontal") * speed;
        _rigidbody2d.velocity = new Vector2(_movement.x, _rigidbody2d.velocity.y);
        if (_rigidbody2d.velocity != Vector2.zero)
        {
            State = States.Hero_walking;
        }

        if (_movement.x > 0)
        {
            _spriteRenderer.flipX = true;
            WhereSharkLooking = SharksDirection.right;
        }
        else if (_movement.x < 0)
        {
            _spriteRenderer.flipX = false;
            WhereSharkLooking = SharksDirection.left;
        }
    }


    private void Stay()
    {
        if (_rigidbody2d.velocity == Vector2.zero)
        {
            State = States.Hero_idle;
        }
    }

    private void Shoot()
    {
        _timePassedSinceLastShoot += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_timePassedSinceLastShoot > 2)
            {
                Vector3 distanceBulletSpawnFromShark = new Vector3(_heroPosition.transform.position.x,
                    _heroPosition.transform.position.y, _heroPosition.transform.position.z);
                State = States.Hero_shooting;
                _timePassedSinceLastShoot = 0;
                if (WhereSharkLooking == SharksDirection.right)
                {
                    distanceBulletSpawnFromShark.x += 1;
                }

                Instantiate(_bullets, distanceBulletSpawnFromShark, Quaternion.identity);
            }
        }
    }

    private States State
    {
        get { return (States)_animation.GetInteger("State"); }
        set { _animation.SetInteger("State", (int)value); }
    }


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        Walk();
        Stay();
        Shoot();
    }
}