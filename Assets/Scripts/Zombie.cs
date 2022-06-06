using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Zombie : MonoBehaviour
{
    public static event Action<Zombie> Died;
    
    [SerializeField] float _attackRange = 1f;
    [SerializeField] int _health = 2;

    int _currentHealth;

    NavMeshAgent _navMeshAgent;
    Animator _animator;

    bool Alive => _currentHealth > 0;

    void Awake()
    {
        _currentHealth = _health;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _navMeshAgent.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        var blasterShot = collision.collider.GetComponent<BlasterShot>();

        if (blasterShot != null)
        {
            _currentHealth--;
            if (_currentHealth <= 0)
                Die();
        }
    }

    public void StartWalking()
    {
        _navMeshAgent.enabled = true;
        _animator.SetBool("Moving", true);
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        _navMeshAgent.enabled = false;
        _animator.SetTrigger("Died");
        transform.position = new Vector3(transform.position.x, -0.8f, transform.position.z);
        Died?.Invoke(this);
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (!Alive)
            return;
        
        var player = FindObjectOfType<PlayerMovement>();
        
        if(_navMeshAgent.enabled)
            _navMeshAgent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) < _attackRange)
            Attack();
    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        _navMeshAgent.enabled = false;
    }

    #region Animation Callbacks
    void AttackComplete()
    {
        if(Alive)
            _navMeshAgent.enabled = true;
    }
    void AttackHit()
    {
        Debug.Log("Killed Player");
        SceneManager.LoadScene(0);
    }
    #endregion 
}
