using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShield : MonoBehaviour
{
	public float result;
	public ParticleSystem healingEffect; // 힐링 파티클 시스템
	private void OnEnable()
	{
		result = GameManager.instance.health;
	}
	private void OnDisable()
	{
		//Debug.Log("health : " + result);
		result = result - GameManager.instance.health;
		//.Log("health : " + GameManager.instance.health);
		//D//ebug.Log("result : " + result * 1.5f * GetComponent<SkillProjectiles>().curlevel);
		GameManager.instance.health += result * 0.5f * GetComponent<SkillProjectiles>().curlevel;
		//Debug.Log("fianl health : " + GameManager.instance.health);
		if (healingEffect != null)
		{
			healingEffect.transform.position = GameManager.instance.player.transform.position;
			healingEffect.Play();
		}
	}
}
