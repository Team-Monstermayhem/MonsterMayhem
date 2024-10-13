using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed;
	public float health;
	public float maxHealth;
	public RuntimeAnimatorController[] animCon;
	public Rigidbody2D target;

	bool isLive;

	Rigidbody2D rigid;
	Collider2D coll;
	Animator anim;
	SpriteRenderer spriteRenderer;

	WaitForFixedUpdate wait;

	// Start is called before the first frame update
	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		coll = GetComponent<Collider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		wait = new WaitForFixedUpdate();
	}

	private void FixedUpdate()
	{
		if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
			return;
		Vector2 dirVec = target.position - rigid.position;
		Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + nextVec);
		rigid.velocity = Vector2.zero;
	}

	private void LateUpdate()
	{
		spriteRenderer.flipX = target.position.x < rigid.position.x;
	}

	private void OnEnable()
	{
		target = GameManager.instance.player.GetComponent<Rigidbody2D>();
		isLive = true;
		health = maxHealth;

        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

	public void Init(SpawnData data)
	{
		anim.runtimeAnimatorController = animCon[data.spriteType];
		speed = data.speed;
		maxHealth = data.health;
		health = data.health;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Bullet") && isLive)
			health -= collision.GetComponent<SkillProjectiles>().curDamage;
		else
			return;
		StartCoroutine(KnockBack());
		//Debug.Log("Name : " + collision);
		if (health > 0)
		{
			anim.SetTrigger("Hit");
		} else
		{
			isLive = false;
			coll.enabled = false;
			rigid.simulated = false;
			spriteRenderer.sortingOrder = 1;
			anim.SetBool("Dead", true);
			GameManager.instance.kill++;
			GameManager.instance.GetExp();
			//Dead();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// 충돌한 오브젝트가 벽일 경우
		if (collision.gameObject.CompareTag("Wall") && isLive)
			health -= collision.gameObject.GetComponent<SkillProjectiles>().curDamage;
		else
			return;
		StartCoroutine(KnockBack());

		if (health > 0)
		{
			anim.SetTrigger("Hit");
		}
		else
		{
			isLive = false;
			coll.enabled = false;
			rigid.simulated = false;
			spriteRenderer.sortingOrder = 1;
			anim.SetBool("Dead", true);
			GameManager.instance.kill++;
			GameManager.instance.GetExp();
			//Dead();
		}
	}

	IEnumerator KnockBack()
	{
		yield return wait; // 다음 하나의 물리 프레임 딜레이.
		Vector3 playerPos = GameManager.instance.transform.position;
		Vector3 dirVec = transform.position - playerPos;
		rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

		//yield return new WaitForSeconds(2f); // 2초쉬기
	}

	void Dead()
	{
		gameObject.SetActive(false);
	}
}
