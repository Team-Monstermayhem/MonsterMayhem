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

	}
	// Update is called once per frame
	void Update()
	{
		inputVec.x = Input.GetAxis("Horizontal");
		inputVec.y = Input.GetAxis("Vertical");
		/*f (Input.GetKeyDown(KeyCode.Alpha1))
		{
			
		}*/

	}

	void FixedUpdate()
	{
		Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
		// 위치이동
		rigid.MovePosition(rigid.position + nextVec);
	}

	private void LateUpdate()
	{
		anim.SetFloat("Speed", inputVec.magnitude);
		if (inputVec.x != 0)
		{
			spriteRenderer.flipX = (inputVec.x < 0);
		}
	}
}
