using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CircularSector : MonoBehaviour
{
	public GameObject target;
	private void Update()
	{
		Debug.DrawRay(transform.position, (target.transform.position - transform.position) *10f, Color.red, 5f);
	}
}
