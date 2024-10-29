using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Vector2 inputVec;
	public float speed;
	public Scanner scanner;
	public GameObject[] skillBtns;

	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;
	Animator anim;


	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		scanner = GetComponent<Scanner>();
		SkillController skillConTroller = this.AddComponent<SkillController>();
		skillConTroller.skills = new Skill[5];
		skillConTroller.skills[0] = GameObject.Find("ItemUI 0").GetComponent<Skill>();
		skillConTroller.skills[1] = GameObject.Find("ItemUI 1").GetComponent<Skill>();
		skillConTroller.skills[2] = GameObject.Find("ItemUI 2").GetComponent<Skill>();
		skillConTroller.skills[3] = GameObject.Find("ItemUI 3").GetComponent<Skill>();

	}
	// Update is called once per frame
	void Update()
	{
		if (!GameManager.instance.isLive)
			return;
		inputVec.x = Input.GetAxis("Horizontal");
		inputVec.y = Input.GetAxis("Vertical");
		/*f (Input.GetKeyDown(KeyCode.Alpha1))
		{
			
		}*/

	}

	void FixedUpdate()
	{
        if (!GameManager.instance.isLive)
            return;
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
		// ��ġ�̵�
		rigid.MovePosition(rigid.position + nextVec);
	}

	private void LateUpdate()
	{
        if (!GameManager.instance.isLive)
            return;
        anim.SetFloat("Speed", inputVec.magnitude);
		if (inputVec.x != 0)
		{
			spriteRenderer.flipX = (inputVec.x < 0);
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (!GameManager.instance.isLive)
			return;

		GameManager.instance.health -= Time.deltaTime * 10;

		if (GameManager.instance.health < 0)
		{
			for (int index = 2; index < transform.childCount; index++)
			{
				transform.GetChild(index).gameObject.SetActive(false);
			}

			anim.SetTrigger("Dead");
			GameManager.instance.GameOver();
		}
    }
}
