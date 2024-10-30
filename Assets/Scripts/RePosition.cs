using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosition : MonoBehaviour
{
	Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();	
    }
    private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Area"))
			return;
		Vector3 playerPosition = GameManager.instance.player.transform.position;
		Vector3 myPos = transform.position;
		//Vector3 playerDir = GameManager.instance.player.inputVec;


		switch (transform.tag)
		{
			case "Ground":
				float diffX = playerPosition.x - myPos.x;
				float diffY = playerPosition.y - myPos.y;
				float dirX = diffX < 0 ? -1 : 1;
				float dirY = diffY < 0 ? -1 : 1;
				diffX = Mathf.Abs(diffX);
				diffY = Mathf.Abs(diffY);


				if (diffX > diffY)
				{
					transform.Translate(Vector3.right * dirX * 40);
				}
				else if (diffX < diffY)
				{
					transform.Translate(Vector3.up * dirY * 40);
				}
				break;
			case "Enemy":
				if (coll.enabled)
				{
					Vector3 dist = playerPosition - myPos;
					Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
					transform.Translate(ran + dist * 2);
				}
				break;
			case "Boss":
                if (coll.enabled)
                {
					Vector3 dist = playerPosition - myPos;
					Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
					transform.Translate(ran + dist * 2);
				}
                break;
        }


	}
}
