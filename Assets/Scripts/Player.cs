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
    WaitForFixedUpdate wait;

    private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		scanner = GetComponent<Scanner>();
        wait = new WaitForFixedUpdate();
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
        if (GameManager.instance.health <= 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

	void OnCollisionStay2D(Collision2D collision)
	{
		if (!GameManager.instance.isLive)
			return;

		GameManager.instance.health -= Time.deltaTime * 10;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet2") || !GameManager.instance.isLive)
            return;
        GameManager.instance.health -= collision.GetComponent<Bullet2>().damage;
        StartCoroutine(KnockBack());
    }
    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
}
