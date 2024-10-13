using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
	Transform player;
    // Start is called before the first frame update
    void Start()
    {
		player = GameManager.instance.player.transform;

	}

    // Update is called once per frame
    void Update()
    {
		transform.position = player.position;
    }
}
