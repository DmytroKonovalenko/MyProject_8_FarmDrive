using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 9.0f;
    private Rigidbody _playerRb;

    public Vector3 dir;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!GameManager1.instance.gameOver)
        {
            Movement();
        }

        if (GameManager1.instance.freezeTiles)
        {
            _playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager1.instance.IncrementScore(1);

            if (dir == Vector3.forward)
            {
                dir = Vector3.left;
            }
            else
            {
                dir = Vector3.forward;
            }
        }

        if (!GameManager1.instance.gameOver)
            transform.Translate(dir * _speed * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TopTile" || other.tag == "LeftTile" || other.tag == "StartTile")
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -Vector3.up);
            if (!Physics.Raycast(downRay, out hit))
            {
               
                _playerRb.velocity = new Vector3(0, _playerRb.velocity.y, 0);
                GameManager1.instance.EndGame();
            }
        }
    }
}
