using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
	public Skill[] skills; // 스킬 배열 5개
	public Button[] skillButtons; // 스킬 버튼 배열 5개
	public int selectedSkillIndex = -1; // 현재 선택된 스킬 인덱스
	public int prevSelectedSkillIndex;
	
	public GameObject skillRangeInstance;

	void Start()
	{
		// 버튼 클릭 이벤트 등록
		for (int i = 0; i < skillButtons.Length; i++)
		{
			int index = i; // 클로저 문제 방지
			skillButtons[i].onClick.AddListener(() => ButtonClick(index)); // Button Component에 onclick event mapping
		}
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
		if (selectedSkillIndex != -1 && skills[selectedSkillIndex].level != 0 && skillRangeInstance == null)
		{
			skillRangeInstance = skills[selectedSkillIndex].ShowSkillRange();
			prevSelectedSkillIndex = selectedSkillIndex;
		}
		else if (selectedSkillIndex != -1 && skills[selectedSkillIndex].level != 0 && skillRangeInstance != null)
		{
			if (prevSelectedSkillIndex != selectedSkillIndex)
			{
				skillRangeInstance.SetActive(false);
				skillRangeInstance = skills[selectedSkillIndex].ShowSkillRange();
				prevSelectedSkillIndex = selectedSkillIndex;
			}
		}
		// level 1이상, 스킬 범위가 뜬 상황이 보장되어야 함.
		if (selectedSkillIndex != -1 && skills[selectedSkillIndex].level != 0 && skillRangeInstance != null && Input.GetMouseButtonDown(0)) // 스킬범위가 뜬 상태에서 화면을 클릭하면.
		{
			skillRangeInstance.SetActive(false); // 범위 숨김
			skillRangeInstance = null;
			skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // 스킬 사용
			selectedSkillIndex = -1; // 사용 후 선택 초기화
		}


	}

	private void ButtonClick(int skillIndex)
	{
		selectedSkillIndex = skillIndex;
		if (skills[selectedSkillIndex].level == 0)
			return;
		// 스킬 범위 표시
		if (skillRangeInstance == null)
			skillRangeInstance = skills[skillIndex].ShowSkillRange();
	}

}