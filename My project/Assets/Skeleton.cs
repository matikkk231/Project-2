using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SpriteRenderer _spriteRenderer;


    private int _lifes = 3;
    private bool _stuned = false;
    private float _howMuchStuned = 0;

    enum States
    {
        SkeletonsIdle = 0,
        SkeletonsWalking = 1,
        SkeletonsKicking = 2,
        SkeletonsGettingDamage = 3,
        SkeletonsDying = 4
    }

    private States SkeletonsState
    {
        get { return (States)_animator.GetInteger("SkeletonsState"); }
        set { _animator.SetInteger("SkeletonsState", (int)value); }
    }

    private float _movementSpeed = 0.5f;

    private void Walk()
    {
        if (_stuned)
        {
            _howMuchStuned = _howMuchStuned + Time.deltaTime;
            if (_howMuchStuned > 0.1f)
            {
                _howMuchStuned = 0;
                _stuned = false;
            }
        }

        if (Hero.Instance.transform.position.x > _transform.position.x && _stuned == false)
        {
            _spriteRenderer.flipX = true;

            _transform.Translate(_movementSpeed * Time.deltaTime, 0, 0);
            if (_movementSpeed != 0)
            {
                SkeletonsState = States.SkeletonsWalking;
            }
        }
        if(Hero.Instance.transform.position.x < _transform.position.x && _stuned == false)
        {
            _spriteRenderer.flipX = false;
            _transform.Translate(_movementSpeed * (-1) * Time.deltaTime, 0, 0);
            if (_movementSpeed != 0)
            {
                SkeletonsState = States.SkeletonsWalking;
            }
        }
    }

    // private void Kicking(Collision2D collision)
    // {
    //     if (collision.gameObject == Hero.instance.gameObject)
    //     {
    //         
    //     }
    // }

    private void GetDamage(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SharksFire>() != null)
        {
            float timePassed = 0;
            _lifes--;
            SkeletonsState = States.SkeletonsGettingDamage;
            _stuned = true;
            if (_lifes == 0)
            {
                _movementSpeed = 0;
                SkeletonsState = States.SkeletonsDying;
                _audioSource.Play();
                StartCoroutine(DestroySkeletonAsync());
            }
        }
    }

    private IEnumerator DestroySkeletonAsync()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GetDamage(col);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }
    
    
}