using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class SkillController : MonoBehaviour
{
	public Skill[] skills; // 스킬 배열 5개
	//public Button[] skillButtons; // 스킬 버튼 배열 5개
	public int selectedSkillIndex; // 현재 선택된 스킬 인덱스
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
		} // 여기서 스킬이 선택되야 함.
		if (selectedSkillIndex == -1)
			return;
		Debug.Log("selectedSkill Index : " + selectedSkillIndex  + ", " + skillRangeInstance);
		// 즉시 시전 스킬
		if (skills[selectedSkillIndex].level != 0 && ((selectedSkillIndex == 0 && selectSkillType == 2) || (selectedSkillIndex == 2 && selectSkillType == 3)))
		{
			Debug.Log("dont show skill Range");
			skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition), selectedSkillIndex); // 스킬 사용
			selectedSkillIndex = -1; // 사용 후 선택 초기화	
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
							skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition), selectedSkillIndex); // 스킬 사용
							selectedSkillIndex = -1; // 사용 후 선택 초기화	
							return;	
						}*/

		}
		// level 1이상, 스킬 범위가 뜬 상황이 보장되어야 함.
		if (skills[selectedSkillIndex].level != 0 && skillRangeInstance != null && Input.GetMouseButtonDown(0)) // 스킬범위가 뜬 상태에서 화면을 클릭하면.
		{
			skillRangeInstance.SetActive(false); // 범위 숨김
			skillRangeInstance = null;
			skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition), selectedSkillIndex); // 스킬 사용
			selectedSkillIndex = -1; // 사용 후 선택 초기화
		}
	}
}
/*
	private void ButtonClick(int skillIndex)
	{
		selectedSkillIndex = skillIndex;
		if (skills[selectedSkillIndex].level == 0)
			return;
		// 스킬 범위 표시
		if (skillRangeInstance == null)
			skillRangeInstance = skills[skillIndex].ShowSkillRange();
	}

}*/