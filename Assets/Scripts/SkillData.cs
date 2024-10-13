using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName = "Scripable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { fire=0, earth=1, water=2, wind=3, elevtric=4}
    [Header("# Main Info")]
    public SkillType skillType;
    public int skillId;
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
	public int maxLevel = 5;

	[Header("# Skill Object")]
	public GameObject[] projectiles;
	public GameObject[] skillRangeObjects;

	[Header("# Level Data")]
    public float baseDamage; // lv 0 데미지
    public int baseCount; // lv 0 관통력
    public float[] damages;
    public int[] counts;
	public float[] cooldowns; // 각 레벨의 쿨타임
}