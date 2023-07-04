using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damage;
    [SerializeField] private float lifeTime;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > lifeTime) Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().SetDamage(damage);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.GetComponent<EnemyManager>() != null)
        {
            EnemyManager enemyManager = collision.gameObject.GetComponent<EnemyManager>();
            if (!enemyManager.isPeople)
            {
                enemyManager.SetDamage(damage);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
        if (collision.gameObject.GetComponent<BossManager>() != null)
        {
            BossManager bossManager = collision.gameObject.GetComponent<BossManager>();
            bossManager.SetDamage(damage);
            Destroy(gameObject);
        }
        
    }
}
