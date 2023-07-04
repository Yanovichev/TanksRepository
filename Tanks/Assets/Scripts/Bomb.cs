using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private bool isBigBomb;
    [SerializeField] private GameObject smallBomb;
    [SerializeField] private float speedBomb, attakArea;
    [SerializeField] private int countBomb, damageBigBomb, damageSmallBomb;
    [SerializeField] private AudioClip audioBang;

    private void CreateSmallBomb()
    {
        float deltaAngle = attakArea / (countBomb - 1);
        float startAngle = 90 - (attakArea / 2);
        for (int i = 0; i < countBomb; i++)
        {
            float angle = startAngle + deltaAngle * i;
            GameObject bomb = Instantiate(smallBomb);
            bomb.transform.SetParent(null);
            bomb.transform.position = transform.position + new Vector3(0f, 1f, 0f);
            Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
            Vector2 v = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speedBomb;
            rb.velocity = v;

        }
    }
    private void Bang()
    {
        GameObject nativeAudio = new GameObject();
        nativeAudio.AddComponent<LifeTimeBangBomb>();
        AudioSource audioSours = nativeAudio.AddComponent<AudioSource>();
        audioSours.clip = audioBang;
        audioSours.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponent<Bomb>() == null)
        {
            if (isBigBomb)
            {
                if (collision.gameObject.GetComponent<EnemyManager>() != null)
                {
                    collision.gameObject.GetComponent<EnemyManager>().SetDamage(damageBigBomb);
                    CreateSmallBomb();
                    Bang();
                    Destroy(this.gameObject);
                }
                if (collision.gameObject.GetComponent<EnemyManager>() == null)
                {
                    CreateSmallBomb();
                    Bang();
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<EnemyManager>() != null)
                {
                    collision.gameObject.GetComponent<EnemyManager>().SetDamage(damageSmallBomb);
                    Destroy(this.gameObject);
                }
                if (collision.gameObject.GetComponent<EnemyManager>() == null)
                {
                    Destroy(this.gameObject);
                }
            }
        }  
    }
}
