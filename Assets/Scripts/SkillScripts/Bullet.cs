using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Bullet : SkillProjectiles
{
    public RuntimeAnimatorController[] animCon;
	Animator animator;
    public Rigidbody2D rigid;
	Vector3 direction;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

	}

	public override void Init(int level, Vector3 mouseClickPos, SkillData data)
	{
		base.Init(level, mouseClickPos, data);


		Vector3 pos = mouseClickPos;
		pos.z = 0;

		direction = (pos - GameManager.instance.player.transform.position).normalized;
		transform.up = direction;
		transform.position = GameManager.instance.player.transform.position;
		if (curPer >= 0)
		{
			rigid.velocity = direction * curSpeed;
		}
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Enemy") || curPer == -100)
			return;

		curPer--;
		if (curPer < 0)
		{
			rigid.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}

	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (!collision.CompareTag("Area") || curPer == -100)
			return;

		gameObject.SetActive(false);
    }
}
