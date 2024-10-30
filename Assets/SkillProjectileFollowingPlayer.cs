using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectileFollowingPlayer : MonoBehaviour
{
	Transform player;
	public float dieTime;
    // Start is called before the first frame update
    void Start()
    {
		player = GameManager.instance.player.transform;
		Destroy(gameObject, dieTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}
