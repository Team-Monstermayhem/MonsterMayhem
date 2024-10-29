using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	//public enum prefabType { enemy, skillProj, skillRang, baseAtk};
    public GameObject[] prefabs;
	public SkillData[] skillDatas;
    List<GameObject>[] pools;

    private void Awake()
    {
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				prefabs[5 + i * 4 + j] = skillDatas[i].skillRangeObjects[j];
			}
			for (int j = 0; j < 4; j++)
			{
				prefabs[21 + i * 4 + j] = skillDatas[i].projectiles[j];
			}
		}
		pools = new List<GameObject>[prefabs.Length]; // enemy 0, 1~4 baseAttack, 5~29 skill range, 30~54 skill projectile

        for (int i = 0; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (item != null && !item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }


        return select;
    }
}
