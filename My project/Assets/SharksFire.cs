using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SharksFire : MonoBehaviour
{
    [FormerlySerializedAs("rigidbody2D")] [SerializeField] private Rigidbody2D _rigidbody2D;
    private Vector2 _movement = new Vector2();
    private int _speed = 5;

    private SharksFire[] _sharksFire = new SharksFire[1];

    private int _sharksID = 0;

    // private void SaveSharksFire()
    // {
    //     bool checkEmptySpaceExist;
    //     for (int i = 0; i < _sharksFire.Length; i++)
    //     {
    //         if (_sharksFire[i] == null)
    //         {
    //             _sharksFire[i] = this;
    //             checkEmptySpaceExist = true;
    //             break;
    //         }
    //     }
    //
    //     if (checkEmptySpaceExist = false)
    //     {
    //         Array.Resize(ref _sharksFire, _sharksFire.Length + 1);
    //         _sharksFire[_sharksFire.Length] = this;
    //     }
    // }

    

    [SerializeField] private Animator _animator;

    [FormerlySerializedAs("transform")] [SerializeField] private Transform _transform;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _movementSpeed = 2;


    // Start is called before the first frame update
    void Start()
    {
        if (Hero.WhereSharkLooking == Hero.SharksDirection.right)
        {
            _spriteRenderer.flipX = true;
        }

        if (Hero.WhereSharkLooking == Hero.SharksDirection.left)
        {
            _spriteRenderer.flipX = false;
            _movementSpeed = _movementSpeed * (-1);
        }
    }

    private void DestroySharksFire(Collision2D col)
    {
        if (col.gameObject != Hero.Instance.gameObject)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        DestroySharksFire(col);
    }

    private float _howMuchTimePassedSinceSpawned = 0;

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(_movementSpeed * Time.deltaTime, 0, 0);
    }
}