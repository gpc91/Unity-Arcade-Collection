using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceInvadersBulletPool : MonoBehaviour
{

    public static Queue<SpaceInvadersBullet> BulletPool = new Queue<SpaceInvadersBullet>();
    public static int PoolSizeStart = 10;

    public SpaceInvadersBullet bulletPrefab;
    
    private void OnEnable()
    {
        if (BulletPool.Count < PoolSizeStart)
        {
            do
            {
                SpaceInvadersBullet bullet = CreateBullet();
                bullet.gameObject.SetActive(false);
                BulletPool.Enqueue(bullet);
            } while (BulletPool.Count < PoolSizeStart);
        }
    }

    /// <summary>
    /// Get an inactive bullet from the pool or create a new one and enqueue it if the pool needs to expand
    /// </summary>
    /// <returns>an unused bullet</returns>
    public SpaceInvadersBullet SpawnBullet()
    {
        SpaceInvadersBullet bullet;
        if (BulletPool.Peek().isActiveAndEnabled)
        {
            bullet = CreateBullet();
            BulletPool.Enqueue(bullet);
        }
        else
        {
            bullet = BulletPool.Dequeue();
            BulletPool.Enqueue(bullet);
        }   
        bullet.gameObject.SetActive(true);
        return bullet;
    }
    
                
    public SpaceInvadersBullet CreateBullet()
    {
        SpaceInvadersBullet bullet = Instantiate(bulletPrefab, transform, true);
        return bullet;
    }

    public void DespawnAll()
    {
        foreach (SpaceInvadersBullet bullet in BulletPool.Where(x => x.isActiveAndEnabled))
        {
            bullet.gameObject.SetActive(false);
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
