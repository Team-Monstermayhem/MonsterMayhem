using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class SkillController : MonoBehaviour
{
	public Skill[] skills; // ��ų �迭 5��
	//public Button[] skillButtons; // ��ų ��ư �迭 5��
	public int selectedSkillIndex; // ���� ���õ� ��ų �ε���
	public int prevSelectedSkillIndex;
	public int selectSkillType;
	public GameObject skillRangeInstance;

	void Start()
	{
		selectedSkillIndex = -1;
	}

	void Update()
	{
		for (int i = 0; i < 5; i++)
		{
			if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
			{
				selectedSkillIndex = i;
				break;
			}
		} // ���⼭ ��ų�� ���õǾ� ��.
		if (selectedSkillIndex == -1)
			return;
		Debug.Log("selectedSkill Index : " + selectedSkillIndex  + ", " + skillRangeInstance);
		// ��� ���� ��ų
		if (skills[selectedSkillIndex].level != 0 && ((selectedSkillIndex == 0 && selectSkillType == 2) || (selectedSkillIndex == 2 && selectSkillType == 3)))
		{
			Debug.Log("dont show skill Range");
			skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition), selectedSkillIndex); // ��ų ���
			selectedSkillIndex = -1; // ��� �� ���� �ʱ�ȭ	
			return;
		}
		if (skills[selectedSkillIndex].level != 0 && skillRangeInstance == null)
		{
			skillRangeInstance = skills[selectedSkillIndex].ShowSkillRange(selectedSkillIndex);
			prevSelectedSkillIndex = selectedSkillIndex;
		}
		else if (skills[selectedSkillIndex].level != 0 && skillRangeInstance != null)
		{
			if (prevSelectedSkillIndex != selectedSkillIndex)
			{
				skillRangeInstance.SetActive(false);
				skillRangeInstance = skills[selectedSkillIndex].ShowSkillRange(selectedSkillIndex);
				prevSelectedSkillIndex = selectedSkillIndex;
			}
			/*			else if (selectedSkillIndex == 1 && (selectSkillType == 1 || selectSkillType == 2))
						{
							skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition), selectedSkillIndex); // ��ų ���
							selectedSkillIndex = -1; // ��� �� ���� �ʱ�ȭ	
							return;	
						}*/

		}
		// level 1�̻�, ��ų ������ �� ��Ȳ�� ����Ǿ�� ��.
		if (skills[selectedSkillIndex].level != 0 && skillRangeInstance != null && Input.GetMouseButtonDown(0)) // ��ų������ �� ���¿��� ȭ���� Ŭ���ϸ�.
		{
			skillRangeInstance.SetActive(false); // ���� ����
			skillRangeInstance = null;
			skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition), selectedSkillIndex); // ��ų ���
			selectedSkillIndex = -1; // ��� �� ���� �ʱ�ȭ
		}
	}
}
/*
	private void ButtonClick(int skillIndex)
	{
		selectedSkillIndex = skillIndex;
		if (skills[selectedSkillIndex].level == 0)
			return;
		// ��ų ���� ǥ��
		if (skillRangeInstance == null)
			skillRangeInstance = skills[skillIndex].ShowSkillRange();
	}

}*/