using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float _delay = 0.25f;
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] LayerMask _aimLayerMask;
    [SerializeField] Transform _firePoint;

    float _nextFireTime;
    List<PowerUp> _powerUps = new List<PowerUp>();

    public void AddPowerUp(PowerUp powerUp) => _powerUps.Add(powerUp);

    public void RemovePowerUp(PowerUp powerUp) => _powerUps.Remove(powerUp);

    void Update()
    {
        AimTowardMouse();
        
        if (ReadyToFire())
            Fire();
    }

    void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _aimLayerMask))
        {
            var destination = hitInfo.point;
            destination.y = transform.position.y;
            Vector3 direction = destination - transform.position;
            direction.Normalize();
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    bool ReadyToFire() => Time.time >= _nextFireTime;

    void Fire()
    {
        float delay = _delay;
        foreach (var powerUp in _powerUps)
            delay *= powerUp.DelayMultiplier;


        _nextFireTime = Time.time + delay;
        BlasterShot shot = Instantiate(_blasterShotPrefab, _firePoint.position, transform.rotation);
        shot.Launch(transform.forward);

        if(_powerUps.Any(t => t.SpreadShot))
        {
            shot = Instantiate(
                _blasterShotPrefab,
                _firePoint.position,
                Quaternion.Euler(transform.forward + transform.right));
            
            shot.Launch(transform.forward + transform.right);

            shot = Instantiate(
                _blasterShotPrefab,
                _firePoint.position,
                Quaternion.Euler(transform.forward - transform.right));
            
            shot.Launch(transform.forward - transform.right);
        }
    }

    
}
