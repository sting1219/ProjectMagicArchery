using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI levelText;
    public Slider expSlider;

    [Header("Settings")]
    public string statID;

    // [변경] 기존 Button 대신 Toggle을 제어합니다.
    private Toggle slotToggle;

    private void Awake()
    {
        slotToggle = GetComponent<Toggle>();
        if (slotToggle != null)
        {
            // 토글 상태가 바뀔 때 실행될 리스너 등록
            slotToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    // GameManager의 데이터를 받아와 UI를 새로고침
    public void UpdateUI(StatData data, bool isSelected)
    {
        if (data == null) return;

        statNameText.text = data.displayName;
        levelText.text = $"Lv. {data.level}";

        if (data.maxExp > 0)
        {
            expSlider.value = (float)(data.currentExp / data.maxExp);
        }
        else
        {
            expSlider.value = 0f;
        }

        // [핵심] 백엔드 선택 상태와 토글의 체크 상태를 동기화 (무한 루프 방지를 위해 리스너 잠시 차단 후 갱신)
        if (slotToggle != null)
        {
            slotToggle.SetIsOnWithoutNotify(isSelected);
        }

        // 시각적 피드백 (선택 시 초록색, 해제 시 흰색)
        statNameText.color = isSelected ? Color.green : Color.white;
    }

    // 토글이 켜지거나 꺼질 때 호출되는 함수
    private void OnToggleValueChanged(bool isOn)
    {
        // [핵심] 토글이 "켜지는(체크되는) 순간"에만 GameManager에 알림
        if (isOn && GameManager.Instance != null)
        {
            GameManager.Instance.SelectStat(statID);

            // UI 매니저를 통해 다른 스탯 슬롯들의 텍스트 색상 등 새로고침
            UIManager uiManager = FindAnyObjectByType<UIManager>();
            if (uiManager != null)
            {
                uiManager.RefreshAllStatUI();
            }
        }
    }
}