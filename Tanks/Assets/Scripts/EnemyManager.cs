using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int reward;
    [SerializeField] private GameObject targetAmmo, ammoPrefab, tower, HealthBar;
    [SerializeField] private float leftBoard, rightBoard, speedMove, timeRate, workDistance, fireDistance;
    public float timeDeathPeople;
    [SerializeField] private int damage, health, armor;
    private GameObject player;
    private Rigidbody2D rb;
    private float time;
    [SerializeField] private bool isWork, isFireType, isFire, isDeath, isTyrel;
    public bool isPeople;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioBang;
    public delegate void delegateTank(GameObject gameObject);
    public event delegateTank DeathTank;
    private float maxHealth;

    public int Health
    {
        get { return health; }
        set
        {
            if (value <= 0)
            {
                health = 0;
                Death();
            }
            else
            {
                health = value;
                float lengthOneHealth = 1f / maxHealth;
                float lengthHealth = lengthOneHealth * value;
                HealthBar.transform.localScale = new Vector3(lengthHealth, 1f, 1f);
                HealthBar.transform.localPosition = new Vector3(2.6146f * lengthHealth - 2.6147f, HealthBar.transform.localPosition.y, HealthBar.transform.localPosition.z);

            }
        }
    }
    private void Awake()
    {
        player = GameObject.FindFirstObjectByType<Player>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
    }
    private void Update()
    {
        time += Time.deltaTime;
        CheckDistance();
        if (!isTyrel)
        {
            if (!isFireType) Move();
            if (isFireType && !isFire) Move();
            else if (isFireType && isFire) Stop();
            if (isPeople && isDeath) Stop();
        }

        if (time > timeRate && isWork && !isFireType && !isDeath)
        {
            Fire();
            time = 0;
        }
        if (time > timeRate && isFireType && isFire && !isDeath)
        {
            Fire();
            time = 0;
        }
    }
    private void CheckDistance()
    {
        if (!isDeath)
        {
            float distance = (player.transform.position - gameObject.transform.position).magnitude;
            if (!isWork && distance < workDistance)
            {
                isWork = true;
                if (animator != null)
                {
                    animator.SetBool("isMove", true);
                }
            }
            if (isFireType && distance <= fireDistance)
            {
                isFire = true;
                if (animator != null)
                {
                    animator.SetBool("isMove", false);
                }
            }
            if (isFireType && distance > fireDistance)
            {
                isFire = false;
                if (animator != null)
                {
                    animator.SetBool("isMove", true);
                }
            }
        }      
    }
    private void Move()
    {
        float distance = (player.transform.position - gameObject.transform.position).magnitude;
        if (isWork && !isDeath)
        {
            if (rb.velocity == Vector2.zero)
            {
                rb.velocity = Vector2.left * speedMove;
            }
            if (distance < leftBoard)
            {
                rb.velocity = Vector2.right * speedMove;
            }
            else if (distance > rightBoard)
            {
                rb.velocity = Vector2.left * speedMove;
            }
        }   
    }
    private void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    private void Fire()
    {
        Vector2 forward = player.transform.position - targetAmmo.transform.position;
        float g = Physics2D.gravity.y;
        float x = forward.x;
        float y = forward.y;
        float AngleInRadians = tower.transform.rotation.eulerAngles.z * Mathf.PI / 180;
        float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));
        forward = new Vector2(Mathf.Cos(AngleInRadians), Mathf.Sin(AngleInRadians));
        GameObject ammo = Instantiate(ammoPrefab, targetAmmo.transform);
        ammo.transform.SetParent(null);
        ammo.GetComponent<Rigidbody2D>().velocity = forward * v;
        ammo.GetComponent<Ammo>().damage = damage;

        Bang();
    }
    public void SetDamage(int damage)
    {
        int resultDamage = damage - armor;
        if (resultDamage <= 5) resultDamage = 5;
        Health -= resultDamage;
    }
    private void Death()
    {
        DeathTank?.Invoke(this.gameObject);
        Destroy(this.gameObject);
    }
    private void Bang()
    {
        GameObject nativeAudio = new GameObject();
        nativeAudio.AddComponent<LifeTimeBangBomb>();
        AudioSource audioSours = nativeAudio.AddComponent<AudioSource>();
        audioSours.clip = audioBang;
        audioSours.volume = 0.2f;
        audioSours.Play();
    }
    public IEnumerator PeopleDeath(float time)
    {
        GetComponent<Collider2D>().isTrigger = true;
        if (animator != null) animator.SetBool("isDeath", true);
        isDeath = true;
        Debug.Log(time);
        yield return new WaitForSeconds(time);
        Death();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && isPeople)
        { 
            StartCoroutine(PeopleDeath(timeDeathPeople));
        }  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ammo>() != null && isPeople)
        {
            StartCoroutine(PeopleDeath(timeDeathPeople));
        }
        if (collision.gameObject.GetComponent<Bomb>() != null && isPeople)
        {
            StartCoroutine(PeopleDeath(timeDeathPeople));
        }
    }
}
