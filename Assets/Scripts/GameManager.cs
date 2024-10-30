using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    public float health;
    public float maxhealth;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

	public int selectSKillTtype;
    [Header("# Game Object")]
    public PoolManager poolManager;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Enhance enhance;
    public GameObject enemyCleaner;
    public GameObject uiEnhance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enhance = uiEnhance.GetComponent<Enhance>();
        maxhealth = PlayerPrefs.GetFloat("maxHealth", maxhealth);
        player.attack = PlayerPrefs.GetFloat("attack", player.attack);
        player.speed = PlayerPrefs.GetFloat("speed", player.speed);
        enhance.coin = PlayerPrefs.GetFloat("coin", enhance.coin);
        enhance.healthEnhance = PlayerPrefs.GetInt("healthEnhance", enhance.healthEnhance);
        enhance.attackEnhance = PlayerPrefs.GetInt("attackEnhance", enhance.attackEnhance);
        enhance.speedEnhance = PlayerPrefs.GetInt("speedEnhance", enhance.speedEnhance);
        //GameStart(0); // 게임 시작 시 초기화

        PlayerPrefs.DeleteAll();
    }

    public void GameStart(int id)
    {
		selectSKillTtype = id;
        health = maxhealth;
        gameTime = 0;

        // 첫 선택 UI 설정
        //uiLevelUp.Select(0);
        //isLive = true;
        //uiLevelUp.Select(selectSKillTtype);
        isLive = true;

        player.gameObject.SetActive(true);
		SkillController skillController = player.AddComponent<SkillController>();
		player.GetComponent<SkillController>().selectSkillType = selectSKillTtype;

		skillController.skills = new Skill[5];
		skillController.skills[0] = GameObject.Find("ItemUI 0").AddComponent<Skill>();
		skillController.skills[0].skillData = poolManager.skillDatas[0];
		skillController.skills[1] = GameObject.Find("ItemUI 1").AddComponent<Skill>();
		skillController.skills[0].skillData = poolManager.skillDatas[1];
		skillController.skills[2] = GameObject.Find("ItemUI 2").AddComponent<Skill>();
		skillController.skills[0].skillData = poolManager.skillDatas[2];
		skillController.skills[3] = GameObject.Find("ItemUI 3").AddComponent<Skill>();
		skillController.skills[0].skillData = poolManager.skillDatas[3];

		Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        PlayerPrefs.SetFloat("maxHealth", maxhealth);
        PlayerPrefs.SetFloat("attack", player.attack);
        PlayerPrefs.SetFloat("speed", player.speed);
        PlayerPrefs.SetFloat("coin", enhance.coin);
        PlayerPrefs.SetInt("healthEnhance", enhance.healthEnhance);
        PlayerPrefs.SetInt("attackEnhance", enhance.attackEnhance);
        PlayerPrefs.SetInt("speedEnhance", enhance.speedEnhance);

        SceneManager.LoadScene(0);

        //maxhealth = PlayerPrefs.GetFloat("maxHealth", maxhealth);
        //player.attack = PlayerPrefs.GetFloat("attack", player.attack);
        //player.speed = PlayerPrefs.GetFloat("speed", player.speed);
        //PlayerPrefs.DeleteAll();
    }

    public void DoEnhance()
    {
        uiEnhance.SetActive(true);
    }

    private void Update()
    {
        if (!isLive)
            return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
