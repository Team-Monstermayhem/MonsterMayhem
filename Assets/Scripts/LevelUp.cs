using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    
    public float destroyTime = 2 * 10f;
    private float remainingTime;
    public Text timer;
    int min;
    int sec;
    bool isCountDownActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);

        remainingTime = destroyTime;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        if (isCountDownActive)
            yield break;
        isCountDownActive = true;

        while (remainingTime > 0)
        {
            remainingTime -= Time.unscaledDeltaTime;
            min = Mathf.FloorToInt(remainingTime / 60);
            sec = Mathf.FloorToInt(remainingTime % 60);
            timer.text = string.Format("{0:D2}:{1:D2}", min, sec);
            yield return null;
        }
        Hide();
        isCountDownActive = false;
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        //모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        //무작위로 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            //만렙 아이템의 경우는 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
