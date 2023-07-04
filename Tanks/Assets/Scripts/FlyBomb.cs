using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class FlyBomb : MonoBehaviour
{
    public ParameterManager parameterManager;
    [SerializeField] float speedFly, angleUp, timeDeath;
    [SerializeField] float[] dictanceDrop;
    [SerializeField] private GameObject bomb;
    private GameObject player;
    private float dictance;
    private int numberBomb;
    private Rigidbody2D rb;
    private bool isEmpty, startFly = true;
    public delegate void DeathFly();
    public event DeathFly Notify;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        numberBomb = 1;
    }
    private void Update()
    {
        NewFly();
    }
    private void NewFly()
    {
        dictance = transform.position.x - player.transform.position.x;
        if (startFly)
        {
            rb.velocity = Vector2.right * speedFly;
            startFly = false;
        }
        if (numberBomb <= dictanceDrop.Length && dictance > dictanceDrop[numberBomb - 1])
        {
            GameObject BombObj = Instantiate(bomb, transform);
            BombObj.transform.SetParent(null);
            BombObj.transform.position = transform.position;
            numberBomb++;
        }
        else if (numberBomb > dictanceDrop.Length) isEmpty = true;
        if (isEmpty)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angleUp);
            rb.velocity = new Vector2(Mathf.Cos(angleUp * Mathf.Deg2Rad), Mathf.Sin(angleUp * Mathf.Deg2Rad)) * speedFly;
            StartCoroutine(DeathCoroutine());
            isEmpty = false;
        }
    }
    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(timeDeath);
        Death();
    }
    
    public void Death()
    {
        Notify?.Invoke();
        Destroy(gameObject);
    }
}
