using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000DE9 RID: 3561
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_CustomCharacterWindowGroup : XUiController
{
	// Token: 0x17000C11 RID: 3089
	// (get) Token: 0x06006CFF RID: 27903 RVA: 0x002A5DE6 File Offset: 0x002A3FE6
	[XuiXmlBinding("ismale")]
	public bool IsMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.cbxGender != null && this.cbxGender.SelectedIndex == 1;
		}
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x002A5E00 File Offset: 0x002A4000
	public override void Init()
	{
		base.Init();
		this.gr = GameRandomManager.Instance.CreateGameRandom();
		SDCSDataUtils.SetupData();
	}

	// Token: 0x06006D01 RID: 27905 RVA: 0x002A5E20 File Offset: 0x002A4020
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

	// Token: 0x06006D02 RID: 27906 RVA: 0x002A5E99 File Offset: 0x002A4099
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

	// Token: 0x06006D03 RID: 27907 RVA: 0x002A5EBC File Offset: 0x002A40BC
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

	// Token: 0x06006D04 RID: 27908 RVA: 0x002A5F4C File Offset: 0x002A414C
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

	// Token: 0x06006D05 RID: 27909 RVA: 0x002A6114 File Offset: 0x002A4314
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

	// Token: 0x06006D06 RID: 27910 RVA: 0x002A6190 File Offset: 0x002A4390
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

	// Token: 0x06006D07 RID: 27911 RVA: 0x002A624F File Offset: 0x002A444F
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006D08 RID: 27912 RVA: 0x002A6258 File Offset: 0x002A4458
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

	// Token: 0x06006D09 RID: 27913 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006D0A RID: 27914 RVA: 0x002A651D File Offset: 0x002A471D
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

	// Token: 0x06006D0B RID: 27915 RVA: 0x002A6548 File Offset: 0x002A4748
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

	// Token: 0x06006D0C RID: 27916 RVA: 0x002A65B0 File Offset: 0x002A47B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupGenders()
	{
		this.cbxGender.Elements.Clear();
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("female", Localization.Get("xuiBoolMaleOff", false, null)));
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("male", Localization.Get("xuiBoolMaleOn", false, null)));
	}

	// Token: 0x06006D0D RID: 27917 RVA: 0x002A661C File Offset: 0x002A481C
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

	// Token: 0x06006D0E RID: 27918 RVA: 0x002A6738 File Offset: 0x002A4938
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupRaces(bool _isMale)
	{
		this.cbxRace.Elements.Clear();
		foreach (string text in SDCSDataUtils.GetRaceList(_isMale))
		{
			this.cbxRace.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiRace" + text.ToLower(), false, null)));
		}
	}

	// Token: 0x06006D0F RID: 27919 RVA: 0x002A67C4 File Offset: 0x002A49C4
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

	// Token: 0x06006D10 RID: 27920 RVA: 0x002A6844 File Offset: 0x002A4A44
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

	// Token: 0x06006D11 RID: 27921 RVA: 0x002A68AB File Offset: 0x002A4AAB
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyRace(string _name)
	{
		this.previewWindow.Archetype.Race = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.RaceName = _name;
		}
	}

	// Token: 0x06006D12 RID: 27922 RVA: 0x002A68D4 File Offset: 0x002A4AD4
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

	// Token: 0x06006D13 RID: 27923 RVA: 0x002A696C File Offset: 0x002A4B6C
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

	// Token: 0x06006D14 RID: 27924 RVA: 0x002A69F3 File Offset: 0x002A4BF3
	[XuiBindEvent("OnValueChanged", "cbxVariant")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxVariantOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyVariant(StringParsers.ParseSInt32(_newValue.InternalName, 0, -1, NumberStyles.Integer));
		this.updatePreview(true);
	}

	// Token: 0x06006D15 RID: 27925 RVA: 0x002A6A10 File Offset: 0x002A4C10
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyVariant(int _variant)
	{
		this.previewWindow.Archetype.Variant = _variant;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.VariantNumber = _variant;
		}
	}

	// Token: 0x06006D16 RID: 27926 RVA: 0x002A6A38 File Offset: 0x002A4C38
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

	// Token: 0x06006D17 RID: 27927 RVA: 0x002A6ACC File Offset: 0x002A4CCC
	[XuiBindEvent("OnValueChanged", "cbxEyeColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxEyeColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyEyeColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006D18 RID: 27928 RVA: 0x002A6AE1 File Offset: 0x002A4CE1
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEyeColor(string _name)
	{
		this.previewWindow.Archetype.EyeColorName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.EyeColor = _name;
		}
	}

	// Token: 0x06006D19 RID: 27929 RVA: 0x002A6B08 File Offset: 0x002A4D08
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

	// Token: 0x06006D1A RID: 27930 RVA: 0x002A6BD0 File Offset: 0x002A4DD0
	[XuiBindEvent("OnValueChanged", "cbxHairStyle")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairStyle(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006D1B RID: 27931 RVA: 0x002A6BE5 File Offset: 0x002A4DE5
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairStyle(string _name)
	{
		this.previewWindow.Archetype.Hair = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairName = _name;
		}
	}

	// Token: 0x06006D1C RID: 27932 RVA: 0x002A6C0C File Offset: 0x002A4E0C
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

	// Token: 0x06006D1D RID: 27933 RVA: 0x002A6CA4 File Offset: 0x002A4EA4
	[XuiBindEvent("OnValueChanged", "cbxHairColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006D1E RID: 27934 RVA: 0x002A6CB9 File Offset: 0x002A4EB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairColor(string _name)
	{
		this.previewWindow.Archetype.HairColor = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairColor = _name;
		}
	}

	// Token: 0x06006D1F RID: 27935 RVA: 0x002A6CE0 File Offset: 0x002A4EE0
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

	// Token: 0x06006D20 RID: 27936 RVA: 0x002A6DBC File Offset: 0x002A4FBC
	[XuiBindEvent("OnValueChanged", "cbxMustaches")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxMustachesOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyMustache(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006D21 RID: 27937 RVA: 0x002A6DD1 File Offset: 0x002A4FD1
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyMustache(string _name)
	{
		this.previewWindow.Archetype.MustacheName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.MustacheName = _name;
		}
	}

	// Token: 0x06006D22 RID: 27938 RVA: 0x002A6DF8 File Offset: 0x002A4FF8
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

	// Token: 0x06006D23 RID: 27939 RVA: 0x002A6ED4 File Offset: 0x002A50D4
	[XuiBindEvent("OnValueChanged", "cbxChops")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxChopsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyChops(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006D24 RID: 27940 RVA: 0x002A6EE9 File Offset: 0x002A50E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyChops(string _name)
	{
		this.previewWindow.Archetype.ChopsName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.ChopsName = _name;
		}
	}

	// Token: 0x06006D25 RID: 27941 RVA: 0x002A6F10 File Offset: 0x002A5110
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

	// Token: 0x06006D26 RID: 27942 RVA: 0x002A6FEC File Offset: 0x002A51EC
	[XuiBindEvent("OnValueChanged", "cbxBeards")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxBeardsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyBeard(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006D27 RID: 27943 RVA: 0x002A7001 File Offset: 0x002A5201
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyBeard(string _name)
	{
		this.previewWindow.Archetype.BeardName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.BeardName = _name;
		}
	}

	// Token: 0x06006D28 RID: 27944 RVA: 0x002A7028 File Offset: 0x002A5228
	public static void Open(XUi _xui, Action _onClosed)
	{
		XUiC_CustomCharacterWindowGroup childByType = _xui.GetChildByType<XUiC_CustomCharacterWindowGroup>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.WindowGroup, false);
	}

	// Token: 0x040052DC RID: 21212
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasChanges;

	// Token: 0x040052DD RID: 21213
	public PlayerProfile PlayerProfile;

	// Token: 0x040052DE RID: 21214
	public Archetype Archetype;

	// Token: 0x040052DF RID: 21215
	[XuiBindComponent("cbxRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxRace;

	// Token: 0x040052E0 RID: 21216
	[XuiBindComponent("cbxGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxGender;

	// Token: 0x040052E1 RID: 21217
	[XuiBindComponent("cbxEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxEyeColor;

	// Token: 0x040052E2 RID: 21218
	[XuiBindComponent("cbxHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairStyle;

	// Token: 0x040052E3 RID: 21219
	[XuiBindComponent("cbxHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairColor;

	// Token: 0x040052E4 RID: 21220
	[XuiBindComponent("cbxMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxMustaches;

	// Token: 0x040052E5 RID: 21221
	[XuiBindComponent("cbxChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxChops;

	// Token: 0x040052E6 RID: 21222
	[XuiBindComponent("cbxBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxBeards;

	// Token: 0x040052E7 RID: 21223
	[XuiBindComponent("cbxFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxVariant;

	// Token: 0x040052E8 RID: 21224
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x040052E9 RID: 21225
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040052EA RID: 21226
	[XuiBindComponent("btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x040052EB RID: 21227
	[XuiBindComponent("btnRandomize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRandomize;

	// Token: 0x040052EC RID: 21228
	[XuiBindComponent("btnLockRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockRace;

	// Token: 0x040052ED RID: 21229
	[XuiBindComponent("btnLockGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockGender;

	// Token: 0x040052EE RID: 21230
	[XuiBindComponent("btnLockFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockFace;

	// Token: 0x040052EF RID: 21231
	[XuiBindComponent("btnLockEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockEyeColor;

	// Token: 0x040052F0 RID: 21232
	[XuiBindComponent("btnLockHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairStyle;

	// Token: 0x040052F1 RID: 21233
	[XuiBindComponent("btnLockHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairColor;

	// Token: 0x040052F2 RID: 21234
	[XuiBindComponent("btnLockMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockMustaches;

	// Token: 0x040052F3 RID: 21235
	[XuiBindComponent("btnLockChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockChops;

	// Token: 0x040052F4 RID: 21236
	[XuiBindComponent("btnLockBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockBeards;

	// Token: 0x040052F5 RID: 21237
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom gr;

	// Token: 0x040052F6 RID: 21238
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosed;

	// Token: 0x02000DEA RID: 3562
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct NameData
	{
		// Token: 0x06006D2C RID: 27948 RVA: 0x002A706E File Offset: 0x002A526E
		public NameData(string _internalName, string _formattedName)
		{
			this.InternalName = _internalName;
			this.formattedName = _formattedName;
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x002A707E File Offset: 0x002A527E
		public override string ToString()
		{
			return this.formattedName;
		}

		// Token: 0x040052F7 RID: 21239
		public readonly string InternalName;

		// Token: 0x040052F8 RID: 21240
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string formattedName;
	}
}
