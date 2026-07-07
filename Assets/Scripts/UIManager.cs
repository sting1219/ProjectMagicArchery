using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI goldText;

    // ★ 추가된 변수들
    public GameObject craftButtonObject; // Craft 버튼 오브젝트 자체
    public Target targetScript;          // 맵에 있는 Target 스크립트 연결

    [Header("★ Tab Panels")]
    // 유니티 인스펙터에서 각 패널 부모 오브젝트를 연결합니다.
    public GameObject skillsPanelObject;
    public GameObject questsPanelObject;

    [Header("★ Stat UI Specs")]
    // 인스펙터에서 드래그 앤 드롭으로 6개의 스탯 슬롯 UI를 리스트에 등록합니다.
    public List<StatSlotUI> statSlots = new List<StatSlotUI>();


    private void Start()
    {
        // 게임 시작할 때는 Craft 버튼을 숨김
        ShowCraftButton(false);
        RefreshAllStatUI(); // 시작할 때 초기 데이터 배치

        // 게임 시작 시 기본적으로 Skills 패널을 켜고, Quests 패널은 끕니다.
        SelectTab(0);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        // [핵심] 경험치가 차오르는 모습을 실시간(매 프레임)으로 갱신해줍니다.
        RealtimeUpdateStatSliders();
    }

    public void SelectTab(int tabIndex)
    {
        if (skillsPanelObject != null && questsPanelObject != null)
        {
            // 0번이면 Skills ON / Quests OFF, 1번이면 반대
            skillsPanelObject.SetActive(tabIndex == 0);
            questsPanelObject.SetActive(tabIndex == 1);

            Debug.Log($"현재 활성화된 탭: {(tabIndex == 0 ? "Skills" : "Quests")}");
        }
    }

    // 전체 스탯 UI 틀을 완전히 새로고침 (텍스트, 레벨, 선택 상태 등)
    public void RefreshAllStatUI()
    {
        if (GameManager.Instance == null) return;

        foreach (StatSlotUI slot in statSlots)
        {
            if (slot == null) continue;

            // GameManager에서 해당 스탯의 백엔드 데이터를 찾아옵니다.
            if (GameManager.Instance.playerStats.TryGetValue(slot.statID, out StatData data))
            {
                // 현재 이 스탯이 유저가 선택한 스탯인지 판별하여 전달
                bool isSelected = (GameManager.Instance.selectedStatID == slot.statID);
                slot.UpdateUI(data, isSelected);
            }
        }
    }

    // 매 프레임 슬라이더 바 게이지를 부드럽게 실시간 갱신하는 함수
    private void RealtimeUpdateStatSliders()
    {
        foreach (StatSlotUI slot in statSlots)
        {
            if (slot == null) continue;

            if (GameManager.Instance.playerStats.TryGetValue(slot.statID, out StatData data))
            {
                bool isSelected = (GameManager.Instance.selectedStatID == slot.statID);

                // 매 프레임 수치 변경 반영
                slot.UpdateUI(data, isSelected);
            }
        }
    }

    // ★ 타겟 파괴/생성 시 버튼을 켜고 끄는 함수
    public void ShowCraftButton(bool show)
    {
        if (craftButtonObject != null)
        {
            craftButtonObject.SetActive(show);
        }
    }

    // ★ Craft 버튼을 눌렀을 때 실행될 함수
    public void OnClickCraftTarget()
    {
        if (targetScript != null)
        {
            targetScript.ResetTarget(); // [주석 해제] 타겟 HP 리셋 및 부활!
            ShowCraftButton(false);     // Craft 버튼 다시 숨기기
        }
    }
}