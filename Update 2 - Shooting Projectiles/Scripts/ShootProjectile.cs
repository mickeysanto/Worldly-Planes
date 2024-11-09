using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    private ObjectPool objPool;
    private GameObject projectile;

    public ShootProjectile(ObjectPool pObjPool)
    {
        objPool = pObjPool;
    }
     
    public void Shoot(GameObject spawnPoint)
    {
        projectile = objPool.GetPooledObject();

        // if there is an object available in the object pool reset its position and activate it
        if(projectile != null)
        {
            projectile.transform.position = spawnPoint.transform.position;
            projectile.transform.rotation = spawnPoint.transform.rotation;
            projectile.SetActive(true);
        }
    }
}
