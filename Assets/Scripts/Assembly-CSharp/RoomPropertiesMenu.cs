using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(HideableObject))]
public class RoomPropertiesMenu : MonoBehaviour
{
	
	private void Awake()
	{
		this.m_hideable = base.GetComponent<HideableObject>();
		HideableObject hideable = this.m_hideable;
		hideable.OnShow = (Action)Delegate.Combine(hideable.OnShow, new Action(this.Show));
		this.applyButton = this.GetChild<Button>("Apply Button");
		this.backButton = this.GetChild<Button>("Back Button");
		Transform buttonContainer = base.transform.Find("Chambers Panel").Find("Chamber Button Container");
		this.chamberButtons = new Dictionary<string, ChamberButton>();
		foreach (ChamberButton button in buttonContainer.GetComponentsInChildren<ChamberButton>())
		{
			this.chamberButtons.Add(button.tileset.ToString(), button);
		}
		this.SetupCategoryDropdowns();
	}

	
	private void Start()
	{
		RoomPropertiesMenu.Instance = this;
		base.gameObject.SetActive(false);
		this.applyButton.interactable = false;
	}

	
	public void OnCategoryChanged()
	{
		Enums.RoomCategory category = Enums.GetEnumValue<Enums.RoomCategory>(this.categoryDropdown.options[this.categoryDropdown.value].text);
		Enums.RoomCategory roomCategory = category;
		if (roomCategory != Enums.RoomCategory.SPECIAL)
		{
			if (roomCategory != Enums.RoomCategory.BOSS)
			{
				if (roomCategory != Enums.RoomCategory.SECRET)
				{
					this.SetActiveSubcatDropdown(this.normalDropdown.transform);
				}
				else
				{
					this.SetActiveSubcatDropdown(this.secretDropdownDummy);
				}
			}
			else
			{
				this.SetActiveSubcatDropdown(this.bossDropdown.transform);
			}
		}
		else
		{
			this.SetActiveSubcatDropdown(this.specialDropdown.transform);
		}
	}

	
	private void SetActiveSubcatDropdown(Transform dropdown)
	{
		foreach (Transform scd in this.subCategoryDropdowns)
		{
			scd.gameObject.SetActive(scd == dropdown.transform);
		}
	}

	
	public void OnValueChanged()
	{
		this.dirty = true;
		this.applyButton.interactable = true;
	}

	
	public void OnBackClicked()
	{
		this.m_hideable.Hide();
	}

	
	public void OnApplyClicked()
	{
		this.applyButton.interactable = false;
		RoomProperties properties = Manager.Instance.roomProperties;
		foreach (ChamberButton button in this.chamberButtons.Values)
		{
			properties.validTilesets[button.tileset.ToString()] = button.Toggled;
		}
		properties.category = this.GetCategory<Enums.RoomCategory>(this.categoryDropdown);
		properties.normalSubCategory = this.GetCategory<Enums.RoomNormalSubCategory>(this.normalDropdown);
		properties.specialSubCategory = this.GetCategory<Enums.RoomSpecialSubCategory>(this.specialDropdown);
		properties.bossSubCategory = this.GetCategory<Enums.RoomBossSubCategory>(this.bossDropdown);
        properties.superSpecialRooms = this.GetCategoryBase<Enums.SuperSpecialRooms>(this.BossRoomPool);

        properties.weight = float.Parse(this.weightField.text);
        properties.visualSubtype = int.Parse(this.visualSubtypeField.text);

        properties.shuffleReinforcementPositions = this.shuffleReinforcementPositionsButton.Toggled;
		properties.darkRoom = this.darkRoomButton.Toggled;
		properties.doFloorDecoration = this.floorDecoButton.Toggled;
		properties.doWallDecoration = this.wallDecoButton.Toggled;
		properties.doLighting = this.lightingButton.Toggled;
		properties.visualSubtype = int.Parse(this.visualSubtypeField.text);

        properties.AmbientLight_R = int.Parse(this.AmbientColor_R.text);
        properties.AmbientLight_G = int.Parse(this.AmbientColor_G.text);
        properties.AmbientLight_B = int.Parse(this.AmbientColor_B.text);
        properties.usesAmbientLight = this.usesAmbientLighting.Toggled;

    }

    public void AmbientColorChanged_R()
    {
        float val;
        bool success = float.TryParse(this.AmbientColor_R.text, out val);
        bool flag = !success;
        if (flag)
        {
            this.AmbientColor_R.text = "1";
        }
    }

    public void AmbientColorChanged_G()
    {
        float val;
        bool success = float.TryParse(this.AmbientColor_G.text, out val);
        bool flag = !success;
        if (flag)
        {
            this.AmbientColor_R.text = "1";
        }
    }

    public void AmbientColorChanged_B()
    {
        float val;
        bool success = float.TryParse(this.AmbientColor_B.text, out val);
        bool flag = !success;
        if (flag)
        {
            this.AmbientColor_R.text = "1";
        }
    }

    public void OnWeightChanged()
	{
		float val;
		bool success = float.TryParse(this.weightField.text, out val);
		bool flag = !success;
		if (flag)
		{
			this.weightField.text = "1";
		}
	}

    public void OnSubtypeChanged()
    {
        int val;
        bool success = int.TryParse(this.visualSubtypeField.text, out val);
        bool flag = !success;
        if (flag)
        {
            this.visualSubtypeField.text = "0";
        }
		if (subtypeChanged != null) { subtypeChanged(); }
    }
	public Action subtypeChanged;

    private T GetCategory<T>(Dropdown dropdown) where T : Enum
	{
		string val = dropdown.options[dropdown.value].text;
		return Enums.GetEnumValue<T>(val);
	}
    private T GetCategoryBase<T>(Dropdown dropdown) where T : Enum
    {
        string val = dropdown.options[dropdown.value].text;
        return Enums.GetEnumValueBase<T>(val);
    }

    private T GetChild<T>(string name)
	{
		return base.transform.Find(name).GetComponent<T>();
	}

	
	public void Show()
	{
		bool flag = !Manager.Instance || !Manager.Instance.LateStartCompleted;
		if (!flag)
		{
			RoomProperties properties = Manager.Instance.roomProperties;
			foreach (KeyValuePair<string, bool> tileset in properties.validTilesets)
			{
				bool flag2 = this.chamberButtons.ContainsKey(tileset.Key);
				if (flag2)
				{
					this.chamberButtons[tileset.Key].Toggled = tileset.Value;
				}
			}
			this.weightField.text = properties.weight.ToString();
            this.visualSubtypeField.text = properties.visualSubtype.ToString();

            this.shuffleReinforcementPositionsButton.Toggled = properties.shuffleReinforcementPositions;
			this.floorDecoButton.Toggled = properties.doFloorDecoration;
			this.wallDecoButton.Toggled = properties.doWallDecoration;
			this.lightingButton.Toggled = properties.doLighting;

            this.AmbientColor_R.text = properties.AmbientLight_R.ToString();
            this.AmbientColor_G.text = properties.AmbientLight_G.ToString();
            this.AmbientColor_B.text = properties.AmbientLight_B.ToString();
			this.usesAmbientLighting.Toggled = properties.usesAmbientLight;
            this.InitializeDropdowns();
		}
	}

	
	public void InitializeDropdowns()
	{
		RoomProperties props = Manager.Instance.roomProperties;
		this.categoryDropdown.value = (int)props.category;
		this.categoryDropdown.RefreshShownValue();
		this.normalDropdown.value = (int)props.normalSubCategory;
		this.normalDropdown.RefreshShownValue();
		this.specialDropdown.value = (int)props.specialSubCategory;
		this.specialDropdown.RefreshShownValue();
		this.bossDropdown.value = (int)props.bossSubCategory;
		this.bossDropdown.RefreshShownValue();

        this.BossRoomPool.value = (int)props.superSpecialRooms;
        this.BossRoomPool.RefreshShownValue();
    }

	
	private void SetupCategoryDropdowns()
	{
		this.subCategoryDropdowns = new List<Transform>();
		this.SetupDropdown<Enums.RoomCategory>(this.categoryDropdown);
		this.SetupDropdown<Enums.RoomNormalSubCategory>(this.normalDropdown);
		this.SetupDropdown<Enums.RoomSpecialSubCategory>(this.specialDropdown);
		this.SetupDropdown<Enums.RoomBossSubCategory>(this.bossDropdown);
        this.SetupDropdown<Enums.SuperSpecialRooms>(this.BossRoomPool);

        this.subCategoryDropdowns.Add(this.normalDropdown.transform);
		this.subCategoryDropdowns.Add(this.specialDropdown.transform);
		this.subCategoryDropdowns.Add(this.bossDropdown.transform);
		this.subCategoryDropdowns.Add(this.secretDropdownDummy);
	}

	
	private void SetupDropdown<T>(Dropdown dropdown) where T : Enum
	{
		List<string> options = new List<string>();
		foreach (object e in Enum.GetValues(typeof(T)))
		{
			options.Add(e.ToString());
		}
		dropdown.AddOptions(options);
	}

	
	public static RoomPropertiesMenu Instance;

	
	public Dictionary<string, ChamberButton> chamberButtons;

	
	private bool dirty;

	
	private Button applyButton;

	
	private Button backButton;

	
	public ToggleButton shuffleReinforcementPositionsButton;

	
	public ToggleButton darkRoomButton;

	
	public ToggleButton floorDecoButton;

	
	public ToggleButton wallDecoButton;

	
	public ToggleButton lightingButton;


    public Dropdown categoryDropdown;

	
	public Dropdown normalDropdown;

	
	public Dropdown specialDropdown;

	
	public Dropdown bossDropdown;

	
	public InputField weightField;


    public InputField visualSubtypeField;



    public Dropdown BossRoomPool;


    public Transform secretDropdownDummy;

	
	public List<Transform> subCategoryDropdowns;

	
	private HideableObject m_hideable;

    public InputField AmbientColor_R;
    public InputField AmbientColor_G;
    public InputField AmbientColor_B;

    public ToggleButton usesAmbientLighting;

    public ToggleButton nodeModifyTilemap;

}
