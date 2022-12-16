using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class EnemyLayerButton : MonoBehaviour
{

	public bool Selected
	{
		get { return m_selected; }
		set
		{
			this.m_selected = value;
			this.selectedPanel.SetActive(value);
		}
	}

    void Awake()
    {
        this.selectedPanel = transform.Find("Selected Panel").gameObject;
        this.textComponent = transform.Find("Text").GetComponent<Text>();
        SetupCategoryDropdowns();
    }

    private void SetupCategoryDropdowns()
    {
        triggerDropdowns = new List<Transform>();
        SetupDropdown<RoomEventTriggerCondition>(triggerDropdown);

        triggerDropdowns.Add(triggerDropdown.transform);
    }

    private void SetupDropdown<T>(Dropdown dropdown) where T : Enum
    {
        List<string> options = new List<string>();
        foreach (var e in Enum.GetValues(typeof(T)))
        {
            options.Add(e.ToString());
        }
        dropdown.AddOptions(options);

    }

    public void OnClick()
    {
        this.Selected = true;
        this.selectedPanel.SetActive(true);
        EnemyLayerHandler.Instance.SetSelectedLayer(this);
    }

    public void SetText(string text)
    {
        this.textComponent.text = text;

    }

    public void OnValueChanged()
    {
        triggerCondition = Enums.GetEnumValue<RoomEventTriggerCondition>(triggerDropdown.options[triggerDropdown.value].text);
        Debug.Log(triggerCondition);
        Debug.Log("changed");
    }

    private float m_lastClick;
    private bool m_selected;
    private GameObject selectedPanel;
    public TilemapHandler map;
    private Text textComponent;
    public Dropdown triggerDropdown;
    public List<Transform> triggerDropdowns;
    public RoomEventTriggerCondition triggerCondition;
}
