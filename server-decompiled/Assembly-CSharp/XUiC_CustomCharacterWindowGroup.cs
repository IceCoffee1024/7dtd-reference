using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000DFE RID: 3582
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_CustomCharacterWindowGroup : XUiController
{
	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x06006D9E RID: 28062 RVA: 0x002A927A File Offset: 0x002A747A
	[XuiXmlBinding("ismale")]
	public bool IsMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.cbxGender != null && this.cbxGender.SelectedIndex == 1;
		}
	}

	// Token: 0x06006D9F RID: 28063 RVA: 0x002A9294 File Offset: 0x002A7494
	public override void Init()
	{
		base.Init();
		this.gr = GameRandomManager.Instance.CreateGameRandom();
		SDCSDataUtils.SetupData();
	}

	// Token: 0x06006DA0 RID: 28064 RVA: 0x002A92B4 File Offset: 0x002A74B4
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

	// Token: 0x06006DA1 RID: 28065 RVA: 0x002A932D File Offset: 0x002A752D
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

	// Token: 0x06006DA2 RID: 28066 RVA: 0x002A9350 File Offset: 0x002A7550
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

	// Token: 0x06006DA3 RID: 28067 RVA: 0x002A93E0 File Offset: 0x002A75E0
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

	// Token: 0x06006DA4 RID: 28068 RVA: 0x002A95A8 File Offset: 0x002A77A8
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

	// Token: 0x06006DA5 RID: 28069 RVA: 0x002A9624 File Offset: 0x002A7824
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

	// Token: 0x06006DA6 RID: 28070 RVA: 0x002A96E3 File Offset: 0x002A78E3
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006DA7 RID: 28071 RVA: 0x002A96EC File Offset: 0x002A78EC
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

	// Token: 0x06006DA8 RID: 28072 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006DA9 RID: 28073 RVA: 0x002A99B1 File Offset: 0x002A7BB1
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

	// Token: 0x06006DAA RID: 28074 RVA: 0x002A99DC File Offset: 0x002A7BDC
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

	// Token: 0x06006DAB RID: 28075 RVA: 0x002A9A44 File Offset: 0x002A7C44
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupGenders()
	{
		this.cbxGender.Elements.Clear();
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("female", Localization.Get("xuiBoolMaleOff", false, null)));
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("male", Localization.Get("xuiBoolMaleOn", false, null)));
	}

	// Token: 0x06006DAC RID: 28076 RVA: 0x002A9AB0 File Offset: 0x002A7CB0
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

	// Token: 0x06006DAD RID: 28077 RVA: 0x002A9BCC File Offset: 0x002A7DCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupRaces(bool _isMale)
	{
		this.cbxRace.Elements.Clear();
		foreach (string text in SDCSDataUtils.GetRaceList(_isMale))
		{
			this.cbxRace.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiRace" + text.ToLower(), false, null)));
		}
	}

	// Token: 0x06006DAE RID: 28078 RVA: 0x002A9C58 File Offset: 0x002A7E58
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

	// Token: 0x06006DAF RID: 28079 RVA: 0x002A9CD8 File Offset: 0x002A7ED8
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

	// Token: 0x06006DB0 RID: 28080 RVA: 0x002A9D3F File Offset: 0x002A7F3F
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyRace(string _name)
	{
		this.previewWindow.Archetype.Race = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.RaceName = _name;
		}
	}

	// Token: 0x06006DB1 RID: 28081 RVA: 0x002A9D68 File Offset: 0x002A7F68
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

	// Token: 0x06006DB2 RID: 28082 RVA: 0x002A9E00 File Offset: 0x002A8000
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

	// Token: 0x06006DB3 RID: 28083 RVA: 0x002A9E87 File Offset: 0x002A8087
	[XuiBindEvent("OnValueChanged", "cbxVariant")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxVariantOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyVariant(StringParsers.ParseSInt32(_newValue.InternalName, 0, -1, NumberStyles.Integer));
		this.updatePreview(true);
	}

	// Token: 0x06006DB4 RID: 28084 RVA: 0x002A9EA4 File Offset: 0x002A80A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyVariant(int _variant)
	{
		this.previewWindow.Archetype.Variant = _variant;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.VariantNumber = _variant;
		}
	}

	// Token: 0x06006DB5 RID: 28085 RVA: 0x002A9ECC File Offset: 0x002A80CC
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

	// Token: 0x06006DB6 RID: 28086 RVA: 0x002A9F60 File Offset: 0x002A8160
	[XuiBindEvent("OnValueChanged", "cbxEyeColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxEyeColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyEyeColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DB7 RID: 28087 RVA: 0x002A9F75 File Offset: 0x002A8175
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEyeColor(string _name)
	{
		this.previewWindow.Archetype.EyeColorName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.EyeColor = _name;
		}
	}

	// Token: 0x06006DB8 RID: 28088 RVA: 0x002A9F9C File Offset: 0x002A819C
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

	// Token: 0x06006DB9 RID: 28089 RVA: 0x002AA064 File Offset: 0x002A8264
	[XuiBindEvent("OnValueChanged", "cbxHairStyle")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairStyle(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DBA RID: 28090 RVA: 0x002AA079 File Offset: 0x002A8279
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairStyle(string _name)
	{
		this.previewWindow.Archetype.Hair = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairName = _name;
		}
	}

	// Token: 0x06006DBB RID: 28091 RVA: 0x002AA0A0 File Offset: 0x002A82A0
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

	// Token: 0x06006DBC RID: 28092 RVA: 0x002AA138 File Offset: 0x002A8338
	[XuiBindEvent("OnValueChanged", "cbxHairColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DBD RID: 28093 RVA: 0x002AA14D File Offset: 0x002A834D
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairColor(string _name)
	{
		this.previewWindow.Archetype.HairColor = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairColor = _name;
		}
	}

	// Token: 0x06006DBE RID: 28094 RVA: 0x002AA174 File Offset: 0x002A8374
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

	// Token: 0x06006DBF RID: 28095 RVA: 0x002AA250 File Offset: 0x002A8450
	[XuiBindEvent("OnValueChanged", "cbxMustaches")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxMustachesOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyMustache(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DC0 RID: 28096 RVA: 0x002AA265 File Offset: 0x002A8465
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyMustache(string _name)
	{
		this.previewWindow.Archetype.MustacheName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.MustacheName = _name;
		}
	}

	// Token: 0x06006DC1 RID: 28097 RVA: 0x002AA28C File Offset: 0x002A848C
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

	// Token: 0x06006DC2 RID: 28098 RVA: 0x002AA368 File Offset: 0x002A8568
	[XuiBindEvent("OnValueChanged", "cbxChops")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxChopsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyChops(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DC3 RID: 28099 RVA: 0x002AA37D File Offset: 0x002A857D
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyChops(string _name)
	{
		this.previewWindow.Archetype.ChopsName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.ChopsName = _name;
		}
	}

	// Token: 0x06006DC4 RID: 28100 RVA: 0x002AA3A4 File Offset: 0x002A85A4
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

	// Token: 0x06006DC5 RID: 28101 RVA: 0x002AA480 File Offset: 0x002A8680
	[XuiBindEvent("OnValueChanged", "cbxBeards")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxBeardsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyBeard(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DC6 RID: 28102 RVA: 0x002AA495 File Offset: 0x002A8695
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyBeard(string _name)
	{
		this.previewWindow.Archetype.BeardName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.BeardName = _name;
		}
	}

	// Token: 0x06006DC7 RID: 28103 RVA: 0x002AA4BC File Offset: 0x002A86BC
	public static void Open(XUi _xui, Action _onClosed)
	{
		XUiC_CustomCharacterWindowGroup childByType = _xui.GetChildByType<XUiC_CustomCharacterWindowGroup>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.WindowGroup, false);
	}

	// Token: 0x0400537F RID: 21375
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasChanges;

	// Token: 0x04005380 RID: 21376
	public PlayerProfile PlayerProfile;

	// Token: 0x04005381 RID: 21377
	public Archetype Archetype;

	// Token: 0x04005382 RID: 21378
	[XuiBindComponent("cbxRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxRace;

	// Token: 0x04005383 RID: 21379
	[XuiBindComponent("cbxGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxGender;

	// Token: 0x04005384 RID: 21380
	[XuiBindComponent("cbxEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxEyeColor;

	// Token: 0x04005385 RID: 21381
	[XuiBindComponent("cbxHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairStyle;

	// Token: 0x04005386 RID: 21382
	[XuiBindComponent("cbxHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairColor;

	// Token: 0x04005387 RID: 21383
	[XuiBindComponent("cbxMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxMustaches;

	// Token: 0x04005388 RID: 21384
	[XuiBindComponent("cbxChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxChops;

	// Token: 0x04005389 RID: 21385
	[XuiBindComponent("cbxBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxBeards;

	// Token: 0x0400538A RID: 21386
	[XuiBindComponent("cbxFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxVariant;

	// Token: 0x0400538B RID: 21387
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x0400538C RID: 21388
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x0400538D RID: 21389
	[XuiBindComponent("btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x0400538E RID: 21390
	[XuiBindComponent("btnRandomize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRandomize;

	// Token: 0x0400538F RID: 21391
	[XuiBindComponent("btnLockRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockRace;

	// Token: 0x04005390 RID: 21392
	[XuiBindComponent("btnLockGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockGender;

	// Token: 0x04005391 RID: 21393
	[XuiBindComponent("btnLockFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockFace;

	// Token: 0x04005392 RID: 21394
	[XuiBindComponent("btnLockEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockEyeColor;

	// Token: 0x04005393 RID: 21395
	[XuiBindComponent("btnLockHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairStyle;

	// Token: 0x04005394 RID: 21396
	[XuiBindComponent("btnLockHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairColor;

	// Token: 0x04005395 RID: 21397
	[XuiBindComponent("btnLockMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockMustaches;

	// Token: 0x04005396 RID: 21398
	[XuiBindComponent("btnLockChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockChops;

	// Token: 0x04005397 RID: 21399
	[XuiBindComponent("btnLockBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockBeards;

	// Token: 0x04005398 RID: 21400
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom gr;

	// Token: 0x04005399 RID: 21401
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosed;

	// Token: 0x02000DFF RID: 3583
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct NameData
	{
		// Token: 0x06006DCB RID: 28107 RVA: 0x002AA502 File Offset: 0x002A8702
		public NameData(string _internalName, string _formattedName)
		{
			this.InternalName = _internalName;
			this.formattedName = _formattedName;
		}

		// Token: 0x06006DCC RID: 28108 RVA: 0x002AA512 File Offset: 0x002A8712
		public override string ToString()
		{
			return this.formattedName;
		}

		// Token: 0x0400539A RID: 21402
		public readonly string InternalName;

		// Token: 0x0400539B RID: 21403
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string formattedName;
	}
}
