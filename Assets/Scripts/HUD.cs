using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, BossHealth}
    public InfoType type;

    Text myText;
    Slider mySlider;

    private Boss boss; // Boss ������Ʈ�� ������ ����
    public Vector3 bossHealthOffset = new Vector3(0, 1.5f, 0); // ���� �Ӹ� �� ������ ����

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        if (type == InfoType.BossHealth)
        {
            if (boss == null)
            {
                GameObject bossObject = GameObject.FindWithTag("Boss"); // Boss �±׷� ���� ������Ʈ ã��
                if (bossObject != null)
                {
                    boss = bossObject.GetComponent<Boss>(); // Boss ������Ʈ ��������
                }
            }

            if (boss != null)
            {
                mySlider.value = boss.health / boss.maxHealth;
                // ���� �Ӹ� �� ��ġ�� ����ٴϰ� ����
                Vector3 worldPosition = boss.transform.position + bossHealthOffset;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                transform.position = screenPosition;
                if (!boss.isLive)
                    mySlider.gameObject.SetActive(false);
            }
        }
        else
        {
            switch (type)
            {
                case InfoType.Exp:
                    float curExp = GameManager.instance.exp;
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                    mySlider.value = curExp / maxExp;
                    break;

                case InfoType.Level:
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                    break;
                case InfoType.Kill:
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                    break;
                case InfoType.Time:
                    float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                    break;
                case InfoType.Health:
                    float curHealth = GameManager.instance.health;

                    float maxHealth = GameManager.instance.maxhealth;
					mySlider.value = curHealth / maxHealth;
					break;

            }
        }
    }
}
