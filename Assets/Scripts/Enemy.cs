using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private bool hit = false; // delays the time it takes to destroy. 

    private Player _player;
    private Animator _anim;
    private BoxCollider2D col;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is Null");
        }
        _anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {  
            //sets 10 pointsd for killing this enemy. 
            //Make switch statement to call which enemy was killed. <3 
            _anim.SetBool("Hit", true);
            _player.ScoreKeeper(10);
            col.enabled = false;
            Destroy(this.gameObject, 2.5f);
        }

        if(other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            _player.Damage();
        }
    }
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float randomX = Random.Range(-15f, 15f);
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y <= -15f)
        {
            transform.position = new Vector3(randomX, 9, 0);
        }
    }
}
