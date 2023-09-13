using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    private PredictShootTrajectory predictShootTrajectory;
    [SerializeField] private Transform bullebSpawnPosition;
    [SerializeField] private Rigidbody bulletPrefabRigidbody;
    [SerializeField] private float maxShootPower;
    [SerializeField] private float minShootPower;
    [SerializeField] private float loadShootPower;
    [SerializeField] private float currShootPower;
    [SerializeField] private GameEvent shootGameEvent;
    [SerializeField] private float shootCooldown = 5f;
    private float currShotCooldown;
    private bool canShoot = true;


    private void Awake()
    {
        currShotCooldown = shootCooldown;
        currShootPower = minShootPower;
        predictShootTrajectory = GetComponent<PredictShootTrajectory>();
    }
    private void Update()
    {
        MyInput();
        if (!canShoot)
            ResetCannon();
        if (canShoot)
            predictShootTrajectory.DrawProjection(bullebSpawnPosition, currShootPower, bulletPrefabRigidbody.mass);
    }

    private void ResetCannon()
    {
        if (currShotCooldown <= 0)
        {
            canShoot = true;
            currShotCooldown = shootCooldown;
            predictShootTrajectory.OnLinereRendererAndMark();
            return;
        }
        else
        {
            currShotCooldown -= Time.deltaTime;
        }
        
            
    }

    private void MyInput()
    {
        if (canShoot)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (currShootPower < maxShootPower)
                {
                    currShootPower += loadShootPower * Time.deltaTime;
                }
                else if (currShootPower >= maxShootPower)
                {
                    currShootPower = maxShootPower;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Shoot();
                currShootPower = minShootPower;
            }
        }
    }
    private void Shoot()
    {
        shootGameEvent.Raise(this, shootCooldown);
        bulletPrefabRigidbody = Instantiate(bulletPrefabRigidbody, bullebSpawnPosition.position, Quaternion.identity);
        bulletPrefabRigidbody.AddForce(bullebSpawnPosition.up * currShootPower, ForceMode.Impulse);
        canShoot = false;
        predictShootTrajectory.OffLinereRendererAndMark();
    }
}
