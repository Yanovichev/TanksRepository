using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyRepair : MonoBehaviour
{
    public ParameterManager parameterManager;
    [SerializeField] float dictanceDrop, speedFly, angleUp, timeDeath;
    [SerializeField] private GameObject repairBox;
    private GameObject player;
    private float dictance;
    private Rigidbody2D rb;
    private bool isDrop = false, isStartFly = true, isUp;
    public delegate void DeathFly();
    public event DeathFly Notify;
    // Update is called once per frame
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        NewFly();
    }
    private void NewFly()
    {
        dictance = transform.position.x - player.transform.position.x;
        if (isStartFly)
        {
            rb.velocity = Vector2.right * speedFly;
            isStartFly = false;
        }
        if (dictance > dictanceDrop && !isDrop)
        {
            GameObject repairBoxObj = Instantiate(repairBox, transform);
            repairBoxObj.transform.SetParent(null);
            repairBoxObj.transform.position = transform.position;
            isDrop = true;
        }
        if (isDrop && !isUp)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angleUp);
            rb.velocity = new Vector2(Mathf.Cos(angleUp * Mathf.Deg2Rad), Mathf.Sin(angleUp * Mathf.Deg2Rad)) * speedFly;
            StartCoroutine(DeathCoroutine());
            isUp = true;
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
