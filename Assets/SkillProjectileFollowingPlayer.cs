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
    }

	private void OnEnable()
	{
		dieTime = 3f;
		Invoke("DeActive", dieTime);	
	}

	// Update is called once per frame
	void Update()
    {
		//transform.up = player.transform.forward;
        transform.position = player.position;
    }

	void DeActive()
	{
		gameObject.SetActive(false);
	}
}
