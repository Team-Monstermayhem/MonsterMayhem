using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public int[] nextExp = { 3, 5, 7, 12, 20, 35, 50, 70, 95, 112, 150, 200 };

	public int selectSKillType;
    [Header("# Game Object")]
    public PoolManager poolManager;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public Enhance enhance;
    public GameObject enemyCleaner;
    public GameObject uiEnhance;
    public GameObject uiMainButton;

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
		selectSKillType = id;
        health = maxhealth;
        gameTime = 0;

        // 첫 선택 UI 설정
        //uiLevelUp.Select(0);
        //isLive = true;
        //uiLevelUp.Select(selectSKillTtype);
        isLive = true;

        player.gameObject.SetActive(true);
		SkillController skillController = player.AddComponent<SkillController>();
		player.GetComponent<SkillController>().selectSkillType = selectSKillType;

		skillController.skills = new Skill[5];
		for (int i = 0; i < 3; i++)
		{
			skillController.skills[i] = GameObject.Find("ItemUI " + i).GetComponent<Skill>();
			skillController.skills[i].skillData = poolManager.skillDatas[selectSKillType];
			Image skillIcon = skillController.skills[i].transform.GetChild(0).GetComponent<Image>();
			skillIcon.sprite= poolManager.skillDatas[selectSKillType].skillIcons[i];

            GameObject skillObject = GameObject.Find("Skill " + i);
            Skill levelUpSkill = skillObject.GetComponent<Skill>();
            levelUpSkill.skillData = poolManager.skillDatas[selectSKillType];
            levelUpSkill.transform.GetChild(0).GetComponent<Image>().sprite = poolManager.skillDatas[selectSKillType].skillIcons[i];
            Text[] texts = skillObject.GetComponentsInChildren<Text>();
            texts[1].text = "스킬 " + i;
            texts[2].text = "스킬을 강화합니다.";
        }
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

    public void GameQuit()
    {
        Application.Quit();
    }

    public void DoEnhance()
    {
        uiEnhance.SetActive(true);
    }

    public void SelectType()
    {
        uiMainButton.SetActive(false);
    }

    public void StartTutorial()
    {

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
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
