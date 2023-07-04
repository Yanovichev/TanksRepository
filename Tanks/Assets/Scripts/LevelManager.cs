using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> enemys;
    public int useLevelNumber, timeWait, reward;
    public List<Level> levels;
    [SerializeField] private LobbiManager lobbiManager;
    [SerializeField] private bool isPlaying;
    [SerializeField] private TMP_Text countEnemy, textReward;
    [SerializeField] private Transform targetRewardMove;
    [SerializeField] private GameObject moveableReward;
    [SerializeField] private ManagerWindows managerWindows;
    [SerializeField] private ParameterManager parameterManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private TutorialManager TutorialManager;
    [SerializeField] private Init init;
    [SerializeField] private GameObject canvas, cam;

    private void Awake()
    {

    }
    public int LastLevel
    {
        get { return init.playerData.lastLevel; }
        set
        {
            init.playerData.lastLevel = value;
            init.Save();
            Debug.Log("Save parameters");
        }
    }

    public void ClearGameSpace()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            Destroy(enemys[i]);
        }
        enemys.Clear();
        EnemyManager[] enemyManagers = FindObjectsOfType<EnemyManager>();
        if (enemyManagers.Length > 0)
        {
            for (int i = 0; i < enemyManagers.Length; i++)
            {
                Destroy(enemyManagers[i].gameObject);
            }
        }
        FlyBomb flyBomb = FindObjectOfType<FlyBomb>();
        if (flyBomb != null) flyBomb.Death();
        FlyRepair flyRepair = FindObjectOfType<FlyRepair>();
        if (flyRepair != null) flyRepair.Death();
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        if (bombs.Length > 0)
        {
            for (int i = 0; i < bombs.Length; i++)
            {
                Destroy(bombs[i].gameObject);
            }
        }
        RepairBox[] repairs = FindObjectsOfType<RepairBox>();
        if (repairs.Length > 0)
        {
            for (int i = 0; i < repairs.Length; i++)
            {
                Destroy(repairs[i].gameObject);
            }
        }
    }

    public void GoGame()
    {
        for (int i = 0; i < levels[useLevelNumber - 1]. enemys.Count; i++)
        {
            GameObject enemy = Instantiate(levels[useLevelNumber - 1].enemys[i]);
            enemy.transform.position = new Vector3(levels[useLevelNumber - 1].positioSpawn[i], 0f, 0f);
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            BossManager bossManager = enemy.GetComponent<BossManager>();
            if (enemyManager != null)
            {
                enemyManager.DeathTank += DeathTank;
                enemys.Add(enemy);
            }
            if (bossManager != null)
            {
                bossManager.DeathTank += DeathTank;
                enemys.Add(enemy);
            }
        }
        countEnemy.text = enemys.Count.ToString();
        reward = 0;
    }
    private void DeathTank(GameObject gameObject)
    {
        GameObject useReward = Instantiate(moveableReward, targetRewardMove);
        Vector3 camToEnemy = gameObject.transform.position - cam.transform.position;
        Vector3 posMoney = new Vector3(canvas.transform.position.x + camToEnemy.x, canvas.transform.position.y + camToEnemy.y, canvas.transform.position.z);
        MoveRewards moveRewards = useReward.GetComponent<MoveRewards>();
        moveRewards.posSpawn = posMoney;
        moveRewards.Move();

        if (gameObject.GetComponent<EnemyManager>() != null)
        {
            useReward.GetComponent<MoveRewards>().reward = gameObject.GetComponent<EnemyManager>().reward;
            reward += gameObject.GetComponent<EnemyManager>().reward;
        }
        else if (gameObject.GetComponent<BossManager>() != null)
        {
            useReward.GetComponent<MoveRewards>().reward = gameObject.GetComponent<BossManager>().reward;
            reward += gameObject.GetComponent<BossManager>().reward;
        }
        enemys.Remove(gameObject);
        countEnemy.text = enemys.Count.ToString();    
        if (enemys.Count == 0)
        {
            StartCoroutine(Victory());
        }     
    }
    private IEnumerator Victory()
    {
        if (lobbiManager.godMode) lobbiManager.OffGodMode();
        yield return new WaitForSeconds(timeWait);
        if (LastLevel < levels.Count && useLevelNumber == LastLevel)
        {
            LastLevel++;
            managerWindows.nextLevelButton.SetActive(true);
        }
        else if (useLevelNumber < LastLevel)
        {
            managerWindows.nextLevelButton.SetActive(true);
        }
        else
        {
            managerWindows.nextLevelButton.SetActive(false);
        }
        Time.timeScale = 0f;
        if (Init.Instance.language == "ru")
            textReward.text = $"Ваш выигрыш: {reward}";
        else if (Init.Instance.language == "en")
            textReward.text = $"Ваш выигрыш: {reward}";
        else if (Init.Instance.language == "tr")
            textReward.text = $"Ваш выигрыш: {reward}";
        parameterManager.AmmoCount += reward;
        if (!init.playerData.isNoFirstStart)
        {
            TutorialManager.OpenVictoryTutorial();
        }
        else managerWindows.ToVictoryPanel();
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;
        useLevelNumber++;
    }



    
}
