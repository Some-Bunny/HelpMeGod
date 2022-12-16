using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class NodeLayerButton : MonoBehaviour
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
        SetupDropdown<SerializedPathWrapMode>(triggerDropdown);

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
        NodePathLayerHandler.Instance.SetSelectedLayer(this);
    }

    public void SetText(string text)
    {
        this.textComponent.text = text;

    }

    public void OnValueChanged()
    {
        WrapMode = Enums.GetEnumValue<SerializedPathWrapMode>(triggerDropdown.options[triggerDropdown.value].text);
        Debug.Log(WrapMode);
        Debug.Log("changed");
    }

    private float m_lastClick;
    private bool m_selected;
    private GameObject selectedPanel;
    public TilemapHandler map;
    private Text textComponent;
    public Dropdown triggerDropdown;
    public List<Transform> triggerDropdowns;
    public SerializedPathWrapMode WrapMode;

}
