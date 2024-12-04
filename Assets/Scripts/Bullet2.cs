using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float damage;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // 총알의 데미지를 설정하고 발사 방향과 속도 설정을 위한 메서드
    public void Init(float damage, Vector2 direction, float speed)
    {
        this.damage = damage;
        rigid.velocity = direction * speed; // 주어진 방향과 속도로 총알 발사
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        // 플레이어에게 데미지를 주기
        GameManager.instance.health -= damage;

        // 총알의 속도를 멈추고 비활성화
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // 일정 시간이 지나면 총알 자동 비활성화
        StartCoroutine(DestroyAfterTime(5f));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false); // 5초 후 자동으로 비활성화
    }
}
