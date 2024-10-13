using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeFollowingMouse : MonoBehaviour
{
	private void Awake()
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	// Update is called once per frame
	void Update()
    {
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
/*		Vector3 direction = (pos - transform.position).normalized;
		direction.z = 0;
		transform.up = direction;
*/
		transform.position = pos;
    }
}
