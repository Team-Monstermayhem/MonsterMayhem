using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class SkillBullet : SkillProjectiles
{
    public RuntimeAnimatorController[] animCon;
	Animator animator;
    public Rigidbody2D rigid;
	Vector3 direction;
	public LayerMask layerMask;
	private CircleCollider2D circleCollider;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		circleCollider = GetComponent<CircleCollider2D>();
	}

	public override void Init(int level, Vector3 mouseClickPos, SkillData data, int selectedSkillIndex)
	{
		base.Init(level, mouseClickPos, data, selectedSkillIndex);


		Vector3 pos = mouseClickPos;
		pos.z = 0;

		direction = (pos - GameManager.instance.player.transform.position).normalized;
		transform.up = direction;

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

		transform.position = GameManager.instance.player.transform.position;
		if (curPer >= 0)
		{
			rigid.velocity = direction * curSpeed;
		}
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("name : " + gameObject.name + ", collsion : " + collision.tag);
		if (!collision.CompareTag("Enemy") || curPer == -100)
			return;
		curPer--;
		if (curPer < 0)
		{
			//Debug.Log("È£ÃâµÊ. Stack Trace:\n" + System.Environment.StackTrace);
			rigid.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}

	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (!collision.CompareTag("Area") || curPer == -100)
			return;
		//Debug.Log("triggerexit. Stack Trace:\n" + System.Environment.StackTrace);
		gameObject.SetActive(false);
    }
}
