using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damage;
	public int per;
	public Player player;


	Rigidbody2D rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	public void Init(float damage, int per, Vector3 dir)
	{
		this.damage = damage * player.attack / 10;
		this.per = per;

		if (per >= 0)
		{
			rigid.velocity = dir * 15f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!(collision.CompareTag("Enemy") || collision.CompareTag("Boss")) || per == -100)
			return ;
		per--;

		if (per < 0)
		{
			rigid.velocity = Vector2.zero;
			gameObject.SetActive(false);
			per = 100;
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Area") || per == -100)
			return;
		gameObject.SetActive(false);
	}
}
