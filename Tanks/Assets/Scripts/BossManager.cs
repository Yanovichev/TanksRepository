using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public int reward;
    [SerializeField] private GameObject[] targetsAmmo, towers;
    [SerializeField] private SpawnBossEnenmy[] spawnTanks;
    [SerializeField] private GameObject ammoPrefab, targetSpawn, HealthBar;
    [SerializeField] private float timeRate, workDistance, detaTimeSpawn;
    [SerializeField] private int damage, armor, stage;
    [SerializeField] private int[] health, maxHealth;
    [SerializeField] private bool isWork, isSpawn;
    [SerializeField] private Collider2D tankCollider;
    [SerializeField] private List<GameObject> helpEnemys;
    private GameObject player;
    private float time;
    [SerializeField] private AudioClip audioBang;

    public delegate void delegateTank(GameObject gameObject);
    public event delegateTank DeathTank;

    public int Health
    {
        get { return health[stage]; }
        set
        {
            if (value <= 0)
            {
                health[stage] = 0;
                NextStage();
                float lengthHealth = 0f;
                HealthBar.transform.localScale = new Vector3(lengthHealth, 1f, 1f);
                HealthBar.transform.localPosition = new Vector3(2.6146f * lengthHealth - 2.6147f, HealthBar.transform.localPosition.y, HealthBar.transform.localPosition.z);
            }
            else
            {
                health[stage] = value;
                float lengthOneHealth = 1f / maxHealth[stage];
                float lengthHealth = lengthOneHealth * value;
                HealthBar.transform.localScale = new Vector3(lengthHealth, 1f, 1f);
                HealthBar.transform.localPosition = new Vector3(2.6146f * lengthHealth - 2.6147f, HealthBar.transform.localPosition.y, HealthBar.transform.localPosition.z);
            }
        }
    }
    private void Awake()
    {
        player = GameObject.FindFirstObjectByType<Player>().gameObject;
    }
    private void Update()
    {
        time += Time.deltaTime;
        CheckDistance();

        if (isWork && !isSpawn && time > timeRate && stage < health.Length)
        {
            Fire();
            time = 0;
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
    private void NextStage()
    {
        stage++;
        if (stage >= health.Length) Death();
        else StartCoroutine(SpawnEnemys());
    }
    private void Death()
    {
        DeathTank?.Invoke(this.gameObject);
        Destroy(this.gameObject);
    }
    private void CheckDistance()
    {
        float distance = (player.transform.position - gameObject.transform.position).magnitude;
        if (!isWork && distance < workDistance) isWork = true;
    }
    private void Fire()
    {
        Vector2 forward = player.transform.position - targetsAmmo[stage].transform.position;
        float g = Physics2D.gravity.y;
        float x = forward.x;
        float y = forward.y;
        float AngleInRadians = towers[stage].transform.rotation.eulerAngles.z * Mathf.PI / 180;
        float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));
        forward = new Vector2(Mathf.Cos(AngleInRadians), Mathf.Sin(AngleInRadians));
        GameObject ammo = Instantiate(ammoPrefab, targetsAmmo[stage].transform);
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
    private void DeathHelpTank(GameObject helpTank)
    {
        helpEnemys.Remove(helpTank);
        if (helpEnemys.Count < 1)
        {
            isSpawn = false;
            tankCollider.enabled = true;
        }
    }
    IEnumerator SpawnEnemys()
    {
        isSpawn = true;
        tankCollider.enabled = false;
        for (int i = 0; i < spawnTanks[stage - 1].enemys.Length; i++)
        {
            yield return new WaitForSeconds(detaTimeSpawn);
            GameObject tank = Instantiate(spawnTanks[stage - 1].enemys[i], targetSpawn.transform);
            tank.transform.SetParent(null);
            tank.GetComponent<EnemyManager>().DeathTank += DeathHelpTank;
            helpEnemys.Add(tank);
        }
    }
}
