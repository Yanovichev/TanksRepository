using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FireManager : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject AmmoPrefab, tower;
    [SerializeField] private float speedAmmo, timeRate;
    [SerializeField] private Transform ammoTarget;
    [SerializeField] private ParameterManager parameterManager;
    [SerializeField] private GameObject indicatorRate;
    [SerializeField] private AudioClip audioBang;

    FirePhone firePhone;
    private float time, speedRotateIndicatorRate = 5f;
    private bool isRate;

    public int Damage
    {
        get { return damage; }
        set
        {
            damage = value;
        }
    }
    private void Start()
    {
        parameterManager = GameObject.FindGameObjectWithTag("ParameterManager").GetComponent<ParameterManager>();
        time = timeRate;
        firePhone = FindObjectOfType<FirePhone>();
    }
    private void Update()
    {
        if (Init.Instance.mobile) FireMobile();
        else Fire();
        time += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        Rate();
    }
    private void Rate()
    {
        if (time < timeRate)
        {
            indicatorRate.SetActive(true);
            indicatorRate.transform.Rotate(0f, 0f, 1 * speedRotateIndicatorRate);
        }
        else if (time > timeRate)
        {
            indicatorRate.SetActive(false);
        }
    }
    private void Fire()
    {
        if (Input.GetMouseButton(0) && time > timeRate && parameterManager.AmmoCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject ammo = Instantiate(AmmoPrefab, ammoTarget);
            ammo.transform.SetParent(null);
            Rigidbody2D rb = ammo.GetComponent<Rigidbody2D>();
            float angle = tower.transform.rotation.eulerAngles.z;
            Vector2 forwardAmmo = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.velocity = forwardAmmo * speedAmmo;
            ammo.GetComponent<Ammo>().damage = Damage;
            time = 0;
            parameterManager.AmmoCount--;

            Bang();
        }
    }
    private void FireMobile()
    {
        if (firePhone.isFire && time > timeRate && parameterManager.AmmoCount > 0)
        {
            GameObject ammo = Instantiate(AmmoPrefab, ammoTarget);
            ammo.transform.SetParent(null);
            Rigidbody2D rb = ammo.GetComponent<Rigidbody2D>();
            float angle = tower.transform.rotation.eulerAngles.z;
            Vector2 forwardAmmo = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.velocity = forwardAmmo * speedAmmo;
            ammo.GetComponent<Ammo>().damage = Damage;
            time = 0;
            parameterManager.AmmoCount--;

            Bang();
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
}
