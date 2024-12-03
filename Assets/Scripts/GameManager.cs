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
    public LevelUp tutorialLevelUp;
    public GameObject JoyArrow;
    public GameObject SkillArrow;

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
        if (isTutorial)
        {
            tutorialText.text = "조이스틱을 이용하여 캐릭터를 이동시켜 보세요.";
            JoyArrow.SetActive(true);
            StartCoroutine(MoveEffect(JoyArrow));
        }

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
        tutorialText.text = "플레이 할 속성을 선택하세요. 선택한 속성에 따라 스킬이 달라집니다.";

        // 이동 튜토리얼
        yield return new WaitUntil(() => player.inputVec != Vector2.zero); // 플레이어가 조이스틱을 조작하면 다음 단계로 진행
        JoyArrow.SetActive(false);
        yield return new WaitForSeconds(2f); // 2초 대기
       

        // ----------------몬스터 삽입---------------------

        // 스킬 선택 튜토리얼
        tutorialText.text = "몬스터를 잡아 경험치를 얻어 레벨업을 하면 스킬이나 효과를 선택할 수 있습니다.";
        uiLevelUp.Show();
        level++;
        yield return new WaitUntil(() => tutorialLevelUp.tutorialSkillSelected); // 고를 때까지 대기

        // 스킬 사용 튜토리얼
        tutorialText.text = "활성화 된 스킬을 터치&드래그 해 사용하세요.";
        SkillArrow.SetActive(true);
        StartCoroutine(MoveEffect(SkillArrow));
        yield return new WaitUntil(() => tutorialSkill[0].skillUsed || tutorialSkill[1].skillUsed || tutorialSkill[2].skillUsed);
        SkillArrow.SetActive(false);
        yield return new WaitForSeconds(2f); // 2초 대기

        tutorialText.text = "튜토리얼 종료!";
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
}