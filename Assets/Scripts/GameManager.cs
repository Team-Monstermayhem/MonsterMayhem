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

    public bool isTutorial = false;
    public GameObject tutorialUI;
    public Text tutorialText;
    public Skill[] tutorialSkill;
	public int tutorialSkillUsed;
    public LevelUp tutorialLevelUp;
    public GameObject JoyArrow;
    public GameObject SkillArrow;

	public SkillController skillController;
	[Header("# Spawner Reference")]
    public Spawner spawner;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
		tutorialSkillUsed = 0;

		if (spawner == null)
        {
            spawner = Object.FindAnyObjectByType<Spawner>(); 
            if (spawner == null)
            {
                Debug.LogError("Spawner를 찾을 수 없음");
            }
        }

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

	public void initSkillButton()
	{

	}

    public void GameStart(int id)
    {
        selectSKillType = id;
        health = maxhealth;
        gameTime = 0;
        if (isTutorial)
        {
			
            tutorialText.text = "Use the joystick to move the player";
            JoyArrow.SetActive(true);
            StartCoroutine(MoveEffect(JoyArrow));
        }

        // 첫 선택 UI 설정
        //uiLevelUp.Select(0);
        //isLive = true;
        //uiLevelUp.Select(selectSKillTtype);
        isLive = true;

        player.gameObject.SetActive(true);
        skillController = player.AddComponent<SkillController>();
        player.GetComponent<SkillController>().selectSkillType = selectSKillType;
		Debug.Log("tutorial?");
		skillController.skills = new Skill[5];
        for (int i = 0; i < 3; i++)
        {
            skillController.skills[i] = GameObject.Find("SkillButton " + i).GetComponent<Skill>();
            skillController.skills[i].skillData = poolManager.skillDatas[selectSKillType];
            Image skillIcon = skillController.skills[i].transform.GetChild(0).GetComponent<Image>();
            skillIcon.sprite = poolManager.skillDatas[selectSKillType].skillIcons[i];

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
        uiMainButton.SetActive(false);
        isTutorial = true;

        // 튜토리얼 UI 활성화
        tutorialUI.SetActive(true);

        StartCoroutine(TutorialSequence()); // 튜토리얼 순서 코루틴 실행

    }
    IEnumerator TutorialSequence()
    {
        // 속성 선택 튜토리얼
        tutorialText.text = "Choose an attribute to play with. \nThe skills will vary based on the selected attribute.";

        // 이동 튜토리얼
        yield return new WaitUntil(() => player.inputVec != Vector2.zero); // 플레이어가 조이스틱을 조작하면 다음 단계로 진행
        JoyArrow.SetActive(false);
        yield return new WaitForSeconds(2f); // 2초 대기

		// ----------------몬스터 삽입---------------------
		tutorialText.text = "Try catching the monsters! \nDefeating monsters will earn you experience points.";

		spawner.SpawnTutorialWave(1);

        yield return new WaitUntil(() => level >= 1); 
        //yield return new WaitForSeconds(1f);


		// 스킬 선택 튜토리얼
		tutorialText.text = "Defeat monsters to gain experience points, \nlevel up, and choose skills or effects.";
		//uiLevelUp.Show();
		//level++;
		yield return new WaitUntil(() => tutorialLevelUp.tutorialSkillSelected); // 고를 때까지 대기

		// 스킬 사용 튜토리얼
		tutorialText.text = "Touch the activated skill to use it.";
		SkillArrow.SetActive(true);
		StartCoroutine(MoveEffect(SkillArrow));
		yield return new WaitUntil(() => (tutorialSkillUsed != 0));
		SkillArrow.SetActive(false);
		yield return new WaitForSeconds(2f); // 2초 대기

		tutorialText.text = "Try out all the skills and survive through three waves!";
		spawner.SpawnTutorialWave(1);
		yield return new WaitUntil(() => level >= 2);
		yield return new WaitForSeconds(8f);


		spawner.SpawnTutorialWave(3);
		//yield return new WaitUntil(() => level >= 3);
		yield return new WaitForSeconds(7f);

		spawner.SpawnTutorialWave(5);
		//yield return new WaitUntil(() => level >= 4);
		yield return new WaitForSeconds(6f);

		yield return new WaitUntil(() => (tutorialSkill[0].level >= 0) && (tutorialSkill[1].level >=0) && (tutorialSkill[2].level >= 0));

		tutorialText.text = "Tutorial Completed!";
        yield return new WaitForSeconds(2f); // 2초 대기

        GameRetry();
    }
    IEnumerator MoveEffect(GameObject arrow)
    {
        Vector3 originalPos = arrow.transform.position;

        while (arrow.activeSelf)
        {
            // 위아래 움직이기
            for (float offset = 0f; offset <= 2f; offset += 0.1f)
            {
                arrow.transform.position = originalPos + new Vector3(0, offset, 0);
                yield return new WaitForSeconds(0.03f);
            }

            for (float offset = 2f; offset >= 0f; offset -= 0.1f)
            {
                arrow.transform.position = originalPos + new Vector3(0, offset, 0);
                yield return new WaitForSeconds(0.03f);
            }
        }

        arrow.transform.position = originalPos;
    }

    private void Update()
    {
        if (!isLive || isTutorial)
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

	public void clickSkill0Button()
	{
		skillController.selectedSkillIndex = 0;
		skillController.ButtonClick();
	}
	public void clickSkill1Button()
	{
		skillController.selectedSkillIndex = 1;
		skillController.ButtonClick();
	}
	public void clickSkill2Button()
	{
		skillController.selectedSkillIndex = 2;
		skillController.ButtonClick();
	}
}