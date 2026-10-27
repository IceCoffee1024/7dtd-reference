using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E18 RID: 3608
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_CustomCharacterWindowGroup : XUiController
{
	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x06006DBB RID: 28091 RVA: 0x002ADB32 File Offset: 0x002ABD32
	[XuiXmlBinding("ismale")]
	public bool IsMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.cbxGender != null && this.cbxGender.SelectedIndex == 1;
		}
	}

	// Token: 0x06006DBC RID: 28092 RVA: 0x002ADB4C File Offset: 0x002ABD4C
	public override void Init()
	{
		base.Init();
		this.gr = GameRandomManager.Instance.CreateGameRandom();
		SDCSDataUtils.SetupData();
	}

	// Token: 0x06006DBD RID: 28093 RVA: 0x002ADB6C File Offset: 0x002ABD6C
	public override void OnOpen()
	{
		this.windowGroup.openWindowOnEsc = XUiC_PlayerProfile.ID;
		base.OnOpen();
		this.PlayerProfile = null;
		this.Archetype = null;
		this.Archetype = Archetype.GetArchetype(ProfileSDF.CurrentProfileName());
		if (this.Archetype != null)
		{
			this.Archetype = this.Archetype.Clone();
		}
		else
		{
			string text = ProfileSDF.CurrentProfileName();
			this.PlayerProfile = PlayerProfile.LoadProfile(text).Clone();
		}
		this.setInitialOptions();
	}

	// Token: 0x06006DBE RID: 28094 RVA: 0x002ADBE5 File Offset: 0x002ABDE5
	public override void OnClose()
	{
		base.OnClose();
		Action action = this.onClosed;
		if (action != null)
		{
			action();
		}
		this.onClosed = null;
	}

	// Token: 0x06006DBF RID: 28095 RVA: 0x002ADC08 File Offset: 0x002ABE08
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				ThreadManager.RunTaskAfterFrames(delegate
				{
					this.BtnApply_OnPress(null, 0);
				}, 1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasReleased)
			{
				ThreadManager.RunTaskAfterFrames(delegate
				{
					this.BtnBack_OnPress(null, 0);
				}, 1);
			}
		}
	}

	// Token: 0x06006DC0 RID: 28096 RVA: 0x002ADC98 File Offset: 0x002ABE98
	[PublicizedFrom(EAccessModifier.Private)]
	public void setInitialOptions()
	{
		if (this.PlayerProfile != null)
		{
			this.setupGenders();
			this.cbxGender.SelectedIndex = (this.PlayerProfile.IsMale ? 1 : 0);
			this.setupRaces(this.PlayerProfile.IsMale);
			this.setupVariants(this.PlayerProfile.IsMale, this.PlayerProfile.RaceName);
			this.setupHairStyles(this.PlayerProfile.IsMale);
			this.setupMustaches(this.PlayerProfile.IsMale);
			this.setupChops(this.PlayerProfile.IsMale);
			this.setupBeards(this.PlayerProfile.IsMale);
			this.setupEyeColors();
			this.setupHairColors();
			this.setSelectedRace(this.PlayerProfile.RaceName, false);
			this.setSelectedVariant(this.PlayerProfile.VariantNumber, false);
			this.setSelectedGeneric(this.PlayerProfile.EyeColor, this.cbxEyeColor, new Action<string>(this.applyEyeColor), false);
			this.setSelectedGeneric(this.PlayerProfile.HairName, this.cbxHairStyle, new Action<string>(this.applyHairStyle), false);
			this.setSelectedGeneric(this.PlayerProfile.MustacheName, this.cbxMustaches, new Action<string>(this.applyMustache), false);
			this.setSelectedGeneric(this.PlayerProfile.ChopsName, this.cbxChops, new Action<string>(this.applyChops), false);
			this.setSelectedGeneric(this.PlayerProfile.BeardName, this.cbxBeards, new Action<string>(this.applyBeard), false);
			this.setSelectedGeneric(this.PlayerProfile.HairColor, this.cbxHairColor, new Action<string>(this.applyHairColor), false);
			this.resetLocks();
			return;
		}
		Log.Error("PlayerProfile is null");
	}

	// Token: 0x06006DC1 RID: 28097 RVA: 0x002ADE60 File Offset: 0x002AC060
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetLocks()
	{
		this.btnLockRace.IsSelected = true;
		this.btnLockGender.IsSelected = true;
		this.btnLockFace.IsSelected = false;
		this.btnLockEyeColor.IsSelected = false;
		this.btnLockHairStyle.IsSelected = false;
		this.btnLockHairColor.IsSelected = false;
		this.btnLockMustaches.IsSelected = false;
		this.btnLockChops.IsSelected = false;
		this.btnLockBeards.IsSelected = false;
	}

	// Token: 0x06006DC2 RID: 28098 RVA: 0x002ADEDC File Offset: 0x002AC0DC
	[XuiBindEvent("OnPress", "btnApply")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnApply_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.hasChanges)
		{
			if (this.PlayerProfile != null)
			{
				ProfileSDF.SaveProfile(ProfileSDF.CurrentProfileName(), this.PlayerProfile.ProfileArchetype, this.PlayerProfile.IsMale, this.PlayerProfile.RaceName, this.PlayerProfile.VariantNumber, this.PlayerProfile.EyeColor, this.PlayerProfile.HairName, this.PlayerProfile.HairColor, this.PlayerProfile.MustacheName, this.PlayerProfile.ChopsName, this.PlayerProfile.BeardName);
			}
			else if (this.Archetype != null)
			{
				Archetype.SetArchetype(this.Archetype);
				Archetype.SaveArchetypesToFile();
			}
			this.hasChanges = false;
		}
		this.close();
	}

	// Token: 0x06006DC3 RID: 28099 RVA: 0x002ADF9B File Offset: 0x002AC19B
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006DC4 RID: 28100 RVA: 0x002ADFA4 File Offset: 0x002AC1A4
	[XuiBindEvent("OnPress", "btnRandomize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRandomize_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnLockGender.IsSelected)
		{
			this.cbxGender.SelectedIndex = this.gr.RandomRange(2);
			this.cbxGender.TriggerValueChangedEvent(this.cbxGender.Elements[0]);
		}
		if (!this.btnLockRace.IsSelected)
		{
			this.cbxRace.SelectedIndex = this.gr.RandomRange(this.cbxRace.Elements.Count);
			this.cbxRace.TriggerValueChangedEvent(this.cbxRace.Elements[0]);
		}
		if (!this.btnLockFace.IsSelected)
		{
			this.cbxVariant.SelectedIndex = this.gr.RandomRange(this.cbxVariant.Elements.Count);
			this.cbxVariant.TriggerValueChangedEvent(this.cbxVariant.Elements[0]);
		}
		if (!this.btnLockEyeColor.IsSelected)
		{
			this.cbxEyeColor.SelectedIndex = this.gr.RandomRange(this.cbxEyeColor.Elements.Count);
			this.cbxEyeColor.TriggerValueChangedEvent(this.cbxEyeColor.Elements[0]);
		}
		if (!this.btnLockHairStyle.IsSelected)
		{
			this.cbxHairStyle.SelectedIndex = this.gr.RandomRange(this.cbxHairStyle.Elements.Count);
			this.cbxHairStyle.TriggerValueChangedEvent(this.cbxHairStyle.Elements[0]);
		}
		if (!this.btnLockHairColor.IsSelected)
		{
			this.cbxHairColor.SelectedIndex = this.gr.RandomRange(this.cbxHairColor.Elements.Count);
			this.cbxHairColor.TriggerValueChangedEvent(this.cbxHairColor.Elements[0]);
		}
		if (!this.btnLockMustaches.IsSelected)
		{
			this.cbxMustaches.SelectedIndex = this.gr.RandomRange(this.cbxMustaches.Elements.Count);
			this.cbxMustaches.TriggerValueChangedEvent(this.cbxMustaches.Elements[0]);
		}
		if (!this.btnLockChops.IsSelected)
		{
			this.cbxChops.SelectedIndex = this.gr.RandomRange(this.cbxChops.Elements.Count);
			this.cbxChops.TriggerValueChangedEvent(this.cbxChops.Elements[0]);
		}
		if (!this.btnLockBeards.IsSelected)
		{
			this.cbxBeards.SelectedIndex = this.gr.RandomRange(this.cbxBeards.Elements.Count);
			this.cbxBeards.TriggerValueChangedEvent(this.cbxBeards.Elements[0]);
		}
	}

	// Token: 0x06006DC5 RID: 28101 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006DC6 RID: 28102 RVA: 0x002AE269 File Offset: 0x002AC469
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePreview(bool _zoomToHead = true)
	{
		this.previewWindow.MakePreview();
		if (_zoomToHead)
		{
			this.previewWindow.ZoomToHead();
		}
		this.hasChanges = true;
		this.IsDirty = true;
	}

	// Token: 0x06006DC7 RID: 28103 RVA: 0x002AE294 File Offset: 0x002AC494
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSelectedGeneric(string _name, XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> _combo, Action<string> _applyFunc, bool _applyToPreview = false)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			for (int i = 0; i < _combo.Elements.Count; i++)
			{
				if (_combo.Elements[i].InternalName.EqualsCaseInsensitive(_name))
				{
					_combo.SelectedIndex = i;
					return;
				}
			}
		}
		_combo.SelectedIndex = 0;
		if (_applyToPreview)
		{
			_applyFunc(_combo.Value.InternalName);
		}
	}

	// Token: 0x06006DC8 RID: 28104 RVA: 0x002AE2FC File Offset: 0x002AC4FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupGenders()
	{
		this.cbxGender.Elements.Clear();
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("female", Localization.Get("xuiBoolMaleOff", false, null)));
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("male", Localization.Get("xuiBoolMaleOn", false, null)));
	}

	// Token: 0x06006DC9 RID: 28105 RVA: 0x002AE368 File Offset: 0x002AC568
	[XuiBindEvent("OnValueChanged", "cbxGender")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxGenderOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		string internalName = _newValue.InternalName;
		this.previewWindow.Archetype.Sex = internalName;
		bool flag = internalName == "male";
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.IsMale = flag;
		}
		string internalName2 = this.cbxRace.Value.InternalName;
		int num = StringParsers.ParseSInt32(this.cbxVariant.Value.InternalName, 0, -1, NumberStyles.Integer);
		this.setupRaces(flag);
		this.setSelectedRace(internalName2, true);
		this.setupVariants(flag, this.cbxRace.Value.InternalName);
		this.setSelectedVariant(num, true);
		this.setupHairStyles(flag);
		this.setupMustaches(flag);
		this.setupChops(flag);
		this.setupBeards(flag);
		this.setSelectedGeneric("", this.cbxMustaches, new Action<string>(this.applyMustache), true);
		this.setSelectedGeneric("", this.cbxChops, new Action<string>(this.applyChops), true);
		this.setSelectedGeneric("", this.cbxBeards, new Action<string>(this.applyBeard), true);
		this.updatePreview(false);
	}

	// Token: 0x06006DCA RID: 28106 RVA: 0x002AE484 File Offset: 0x002AC684
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupRaces(bool _isMale)
	{
		this.cbxRace.Elements.Clear();
		foreach (string text in SDCSDataUtils.GetRaceList(_isMale))
		{
			this.cbxRace.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiRace" + text.ToLower(), false, null)));
		}
	}

	// Token: 0x06006DCB RID: 28107 RVA: 0x002AE510 File Offset: 0x002AC710
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSelectedRace(string _raceName, bool _applyToPreview = false)
	{
		if (!string.IsNullOrEmpty(_raceName))
		{
			for (int i = 0; i < this.cbxRace.Elements.Count; i++)
			{
				if (this.cbxRace.Elements[i].InternalName.EqualsCaseInsensitive(_raceName))
				{
					this.cbxRace.SelectedIndex = i;
					return;
				}
			}
		}
		this.cbxRace.SelectedIndex = 0;
		if (_applyToPreview)
		{
			this.applyRace(this.cbxRace.Value.InternalName);
		}
	}

	// Token: 0x06006DCC RID: 28108 RVA: 0x002AE590 File Offset: 0x002AC790
	[XuiBindEvent("OnValueChanged", "cbxRace")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxRaceOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyRace(_newValue.InternalName);
		int num = StringParsers.ParseSInt32(this.cbxVariant.Value.InternalName, 0, -1, NumberStyles.Integer);
		this.setupVariants(this.previewWindow.Archetype.IsMale, this.cbxRace.Value.InternalName);
		this.setSelectedVariant(num, true);
		this.updatePreview(false);
	}

	// Token: 0x06006DCD RID: 28109 RVA: 0x002AE5F7 File Offset: 0x002AC7F7
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyRace(string _name)
	{
		this.previewWindow.Archetype.Race = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.RaceName = _name;
		}
	}

	// Token: 0x06006DCE RID: 28110 RVA: 0x002AE620 File Offset: 0x002AC820
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupVariants(bool _isMale, string _raceName)
	{
		this.cbxVariant.Elements.Clear();
		int num = 1;
		foreach (string text in SDCSDataUtils.GetVariantList(_isMale, _raceName))
		{
			this.cbxVariant.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, string.Format("{0} {1:00}", Localization.Get("lblFace", false, null), num)));
			num++;
		}
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x002AE6B8 File Offset: 0x002AC8B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSelectedVariant(int _variant, bool _applyToPreview = false)
	{
		if (_variant != -1)
		{
			for (int i = 0; i < this.cbxVariant.Elements.Count; i++)
			{
				if (StringParsers.ParseSInt32(this.cbxVariant.Elements[i].InternalName, 0, -1, NumberStyles.Integer) == _variant)
				{
					this.cbxVariant.SelectedIndex = i;
					return;
				}
			}
		}
		this.cbxVariant.SelectedIndex = 0;
		if (_applyToPreview)
		{
			this.applyVariant(StringParsers.ParseSInt32(this.cbxVariant.Value.InternalName, 0, -1, NumberStyles.Integer));
		}
	}

	// Token: 0x06006DD0 RID: 28112 RVA: 0x002AE73F File Offset: 0x002AC93F
	[XuiBindEvent("OnValueChanged", "cbxVariant")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxVariantOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyVariant(StringParsers.ParseSInt32(_newValue.InternalName, 0, -1, NumberStyles.Integer));
		this.updatePreview(true);
	}

	// Token: 0x06006DD1 RID: 28113 RVA: 0x002AE75C File Offset: 0x002AC95C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyVariant(int _variant)
	{
		this.previewWindow.Archetype.Variant = _variant;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.VariantNumber = _variant;
		}
	}

	// Token: 0x06006DD2 RID: 28114 RVA: 0x002AE784 File Offset: 0x002AC984
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupEyeColors()
	{
		this.cbxEyeColor.Elements.Clear();
		int num = 1;
		foreach (string text in SDCSDataUtils.GetEyeColorNames())
		{
			this.cbxEyeColor.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, string.Format("{0} {1:00}", Localization.Get("xuiCharacterColorSlotEyes", false, null), num)));
			num++;
		}
	}

	// Token: 0x06006DD3 RID: 28115 RVA: 0x002AE818 File Offset: 0x002ACA18
	[XuiBindEvent("OnValueChanged", "cbxEyeColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxEyeColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyEyeColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DD4 RID: 28116 RVA: 0x002AE82D File Offset: 0x002ACA2D
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEyeColor(string _name)
	{
		this.previewWindow.Archetype.EyeColorName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.EyeColor = _name;
		}
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x002AE854 File Offset: 0x002ACA54
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupHairStyles(bool _isMale)
	{
		this.cbxHairStyle.Elements.Clear();
		this.cbxHairStyle.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("", Localization.Get("xuiCharacterHairStyle", false, null) + " 00"));
		int num = 1;
		foreach (string text in SDCSDataUtils.GetHairNames(_isMale, SDCSDataUtils.HairTypes.Hair))
		{
			this.cbxHairStyle.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, string.Format("{0} {1:00}", Localization.Get("xuiCharacterHairStyle", false, null), num)));
			num++;
		}
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x002AE91C File Offset: 0x002ACB1C
	[XuiBindEvent("OnValueChanged", "cbxHairStyle")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairStyle(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x002AE931 File Offset: 0x002ACB31
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairStyle(string _name)
	{
		this.previewWindow.Archetype.Hair = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairName = _name;
		}
	}

	// Token: 0x06006DD8 RID: 28120 RVA: 0x002AE958 File Offset: 0x002ACB58
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupHairColors()
	{
		this.cbxHairColor.Elements.Clear();
		int num = 1;
		foreach (SDCSDataUtils.HairColorData hairColorData in SDCSDataUtils.GetHairColorNames())
		{
			this.cbxHairColor.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(hairColorData.PrefabName, string.Format("{0} {1:00}", Localization.Get("xuiCharacterHairColor", false, null), num)));
			num++;
		}
	}

	// Token: 0x06006DD9 RID: 28121 RVA: 0x002AE9F0 File Offset: 0x002ACBF0
	[XuiBindEvent("OnValueChanged", "cbxHairColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DDA RID: 28122 RVA: 0x002AEA05 File Offset: 0x002ACC05
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairColor(string _name)
	{
		this.previewWindow.Archetype.HairColor = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairColor = _name;
		}
	}

	// Token: 0x06006DDB RID: 28123 RVA: 0x002AEA2C File Offset: 0x002ACC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupMustaches(bool _isMale)
	{
		this.cbxMustaches.Elements.Clear();
		this.cbxMustaches.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("", Localization.Get("xuiCharacterMustaches", false, null) + " 00"));
		foreach (string text in SDCSDataUtils.GetHairNames(_isMale, SDCSDataUtils.HairTypes.Mustache))
		{
			string text2 = text;
			if (text2.Length == 1)
			{
				text2 = text2.Insert(0, "0");
			}
			this.cbxMustaches.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiCharacterMustaches", false, null) + " " + Localization.Get(text2, false, null)));
		}
	}

	// Token: 0x06006DDC RID: 28124 RVA: 0x002AEB08 File Offset: 0x002ACD08
	[XuiBindEvent("OnValueChanged", "cbxMustaches")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxMustachesOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyMustache(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DDD RID: 28125 RVA: 0x002AEB1D File Offset: 0x002ACD1D
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyMustache(string _name)
	{
		this.previewWindow.Archetype.MustacheName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.MustacheName = _name;
		}
	}

	// Token: 0x06006DDE RID: 28126 RVA: 0x002AEB44 File Offset: 0x002ACD44
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupChops(bool _isMale)
	{
		this.cbxChops.Elements.Clear();
		this.cbxChops.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("", Localization.Get("xuiCharacterChops", false, null) + " 00"));
		foreach (string text in SDCSDataUtils.GetHairNames(_isMale, SDCSDataUtils.HairTypes.Chops))
		{
			string text2 = text;
			if (text2.Length == 1)
			{
				text2 = text2.Insert(0, "0");
			}
			this.cbxChops.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiCharacterChops", false, null) + " " + Localization.Get(text2, false, null)));
		}
	}

	// Token: 0x06006DDF RID: 28127 RVA: 0x002AEC20 File Offset: 0x002ACE20
	[XuiBindEvent("OnValueChanged", "cbxChops")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxChopsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyChops(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DE0 RID: 28128 RVA: 0x002AEC35 File Offset: 0x002ACE35
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyChops(string _name)
	{
		this.previewWindow.Archetype.ChopsName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.ChopsName = _name;
		}
	}

	// Token: 0x06006DE1 RID: 28129 RVA: 0x002AEC5C File Offset: 0x002ACE5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupBeards(bool _isMale)
	{
		this.cbxBeards.Elements.Clear();
		this.cbxBeards.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("", Localization.Get("xuiCharacterBeards", false, null) + " 00"));
		foreach (string text in SDCSDataUtils.GetHairNames(_isMale, SDCSDataUtils.HairTypes.Beard))
		{
			string text2 = text;
			if (text2.Length == 1)
			{
				text2 = text2.Insert(0, "0");
			}
			this.cbxBeards.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiCharacterBeards", false, null) + " " + Localization.Get(text2, false, null)));
		}
	}

	// Token: 0x06006DE2 RID: 28130 RVA: 0x002AED38 File Offset: 0x002ACF38
	[XuiBindEvent("OnValueChanged", "cbxBeards")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxBeardsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyBeard(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DE3 RID: 28131 RVA: 0x002AED4D File Offset: 0x002ACF4D
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyBeard(string _name)
	{
		this.previewWindow.Archetype.BeardName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.BeardName = _name;
		}
	}

	// Token: 0x06006DE4 RID: 28132 RVA: 0x002AED74 File Offset: 0x002ACF74
	public static void Open(XUi _xui, Action _onClosed)
	{
		XUiC_CustomCharacterWindowGroup childByType = _xui.GetChildByType<XUiC_CustomCharacterWindowGroup>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.WindowGroup, false);
	}

	// Token: 0x040053DB RID: 21467
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasChanges;

	// Token: 0x040053DC RID: 21468
	public PlayerProfile PlayerProfile;

	// Token: 0x040053DD RID: 21469
	public Archetype Archetype;

	// Token: 0x040053DE RID: 21470
	[XuiBindComponent("cbxRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxRace;

	// Token: 0x040053DF RID: 21471
	[XuiBindComponent("cbxGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxGender;

	// Token: 0x040053E0 RID: 21472
	[XuiBindComponent("cbxEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxEyeColor;

	// Token: 0x040053E1 RID: 21473
	[XuiBindComponent("cbxHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairStyle;

	// Token: 0x040053E2 RID: 21474
	[XuiBindComponent("cbxHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairColor;

	// Token: 0x040053E3 RID: 21475
	[XuiBindComponent("cbxMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxMustaches;

	// Token: 0x040053E4 RID: 21476
	[XuiBindComponent("cbxChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxChops;

	// Token: 0x040053E5 RID: 21477
	[XuiBindComponent("cbxBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxBeards;

	// Token: 0x040053E6 RID: 21478
	[XuiBindComponent("cbxFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxVariant;

	// Token: 0x040053E7 RID: 21479
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x040053E8 RID: 21480
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053E9 RID: 21481
	[XuiBindComponent("btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x040053EA RID: 21482
	[XuiBindComponent("btnRandomize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRandomize;

	// Token: 0x040053EB RID: 21483
	[XuiBindComponent("btnLockRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockRace;

	// Token: 0x040053EC RID: 21484
	[XuiBindComponent("btnLockGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockGender;

	// Token: 0x040053ED RID: 21485
	[XuiBindComponent("btnLockFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockFace;

	// Token: 0x040053EE RID: 21486
	[XuiBindComponent("btnLockEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockEyeColor;

	// Token: 0x040053EF RID: 21487
	[XuiBindComponent("btnLockHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairStyle;

	// Token: 0x040053F0 RID: 21488
	[XuiBindComponent("btnLockHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairColor;

	// Token: 0x040053F1 RID: 21489
	[XuiBindComponent("btnLockMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockMustaches;

	// Token: 0x040053F2 RID: 21490
	[XuiBindComponent("btnLockChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockChops;

	// Token: 0x040053F3 RID: 21491
	[XuiBindComponent("btnLockBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockBeards;

	// Token: 0x040053F4 RID: 21492
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom gr;

	// Token: 0x040053F5 RID: 21493
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosed;

	// Token: 0x02000E19 RID: 3609
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct NameData
	{
		// Token: 0x06006DE8 RID: 28136 RVA: 0x002AEDBA File Offset: 0x002ACFBA
		public NameData(string _internalName, string _formattedName)
		{
			this.InternalName = _internalName;
			this.formattedName = _formattedName;
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x002AEDCA File Offset: 0x002ACFCA
		public override string ToString()
		{
			return this.formattedName;
		}

		// Token: 0x040053F6 RID: 21494
		public readonly string InternalName;

		// Token: 0x040053F7 RID: 21495
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string formattedName;
	}
}
