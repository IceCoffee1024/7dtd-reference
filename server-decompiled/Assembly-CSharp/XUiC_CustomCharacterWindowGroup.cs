using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E0D RID: 3597
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_CustomCharacterWindowGroup : XUiController
{
	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06006D96 RID: 28054 RVA: 0x002AC60A File Offset: 0x002AA80A
	[XuiXmlBinding("ismale")]
	public bool IsMale
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.cbxGender != null && this.cbxGender.SelectedIndex == 1;
		}
	}

	// Token: 0x06006D97 RID: 28055 RVA: 0x002AC624 File Offset: 0x002AA824
	public override void Init()
	{
		base.Init();
		this.gr = GameRandomManager.Instance.CreateGameRandom();
		SDCSDataUtils.SetupData();
	}

	// Token: 0x06006D98 RID: 28056 RVA: 0x002AC644 File Offset: 0x002AA844
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

	// Token: 0x06006D99 RID: 28057 RVA: 0x002AC6BD File Offset: 0x002AA8BD
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

	// Token: 0x06006D9A RID: 28058 RVA: 0x002AC6E0 File Offset: 0x002AA8E0
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

	// Token: 0x06006D9B RID: 28059 RVA: 0x002AC770 File Offset: 0x002AA970
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

	// Token: 0x06006D9C RID: 28060 RVA: 0x002AC938 File Offset: 0x002AAB38
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

	// Token: 0x06006D9D RID: 28061 RVA: 0x002AC9B4 File Offset: 0x002AABB4
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

	// Token: 0x06006D9E RID: 28062 RVA: 0x002ACA73 File Offset: 0x002AAC73
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPress(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x06006D9F RID: 28063 RVA: 0x002ACA7C File Offset: 0x002AAC7C
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

	// Token: 0x06006DA0 RID: 28064 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006DA1 RID: 28065 RVA: 0x002ACD41 File Offset: 0x002AAF41
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

	// Token: 0x06006DA2 RID: 28066 RVA: 0x002ACD6C File Offset: 0x002AAF6C
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

	// Token: 0x06006DA3 RID: 28067 RVA: 0x002ACDD4 File Offset: 0x002AAFD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupGenders()
	{
		this.cbxGender.Elements.Clear();
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("female", Localization.Get("xuiBoolMaleOff", false, null)));
		this.cbxGender.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData("male", Localization.Get("xuiBoolMaleOn", false, null)));
	}

	// Token: 0x06006DA4 RID: 28068 RVA: 0x002ACE40 File Offset: 0x002AB040
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

	// Token: 0x06006DA5 RID: 28069 RVA: 0x002ACF5C File Offset: 0x002AB15C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupRaces(bool _isMale)
	{
		this.cbxRace.Elements.Clear();
		foreach (string text in SDCSDataUtils.GetRaceList(_isMale))
		{
			this.cbxRace.Elements.Add(new XUiC_CustomCharacterWindowGroup.NameData(text, Localization.Get("xuiRace" + text.ToLower(), false, null)));
		}
	}

	// Token: 0x06006DA6 RID: 28070 RVA: 0x002ACFE8 File Offset: 0x002AB1E8
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

	// Token: 0x06006DA7 RID: 28071 RVA: 0x002AD068 File Offset: 0x002AB268
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

	// Token: 0x06006DA8 RID: 28072 RVA: 0x002AD0CF File Offset: 0x002AB2CF
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyRace(string _name)
	{
		this.previewWindow.Archetype.Race = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.RaceName = _name;
		}
	}

	// Token: 0x06006DA9 RID: 28073 RVA: 0x002AD0F8 File Offset: 0x002AB2F8
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

	// Token: 0x06006DAA RID: 28074 RVA: 0x002AD190 File Offset: 0x002AB390
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

	// Token: 0x06006DAB RID: 28075 RVA: 0x002AD217 File Offset: 0x002AB417
	[XuiBindEvent("OnValueChanged", "cbxVariant")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxVariantOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyVariant(StringParsers.ParseSInt32(_newValue.InternalName, 0, -1, NumberStyles.Integer));
		this.updatePreview(true);
	}

	// Token: 0x06006DAC RID: 28076 RVA: 0x002AD234 File Offset: 0x002AB434
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyVariant(int _variant)
	{
		this.previewWindow.Archetype.Variant = _variant;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.VariantNumber = _variant;
		}
	}

	// Token: 0x06006DAD RID: 28077 RVA: 0x002AD25C File Offset: 0x002AB45C
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

	// Token: 0x06006DAE RID: 28078 RVA: 0x002AD2F0 File Offset: 0x002AB4F0
	[XuiBindEvent("OnValueChanged", "cbxEyeColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxEyeColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyEyeColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DAF RID: 28079 RVA: 0x002AD305 File Offset: 0x002AB505
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEyeColor(string _name)
	{
		this.previewWindow.Archetype.EyeColorName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.EyeColor = _name;
		}
	}

	// Token: 0x06006DB0 RID: 28080 RVA: 0x002AD32C File Offset: 0x002AB52C
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

	// Token: 0x06006DB1 RID: 28081 RVA: 0x002AD3F4 File Offset: 0x002AB5F4
	[XuiBindEvent("OnValueChanged", "cbxHairStyle")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairStyle(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DB2 RID: 28082 RVA: 0x002AD409 File Offset: 0x002AB609
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairStyle(string _name)
	{
		this.previewWindow.Archetype.Hair = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairName = _name;
		}
	}

	// Token: 0x06006DB3 RID: 28083 RVA: 0x002AD430 File Offset: 0x002AB630
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

	// Token: 0x06006DB4 RID: 28084 RVA: 0x002AD4C8 File Offset: 0x002AB6C8
	[XuiBindEvent("OnValueChanged", "cbxHairColor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxHairColorOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyHairColor(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DB5 RID: 28085 RVA: 0x002AD4DD File Offset: 0x002AB6DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyHairColor(string _name)
	{
		this.previewWindow.Archetype.HairColor = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.HairColor = _name;
		}
	}

	// Token: 0x06006DB6 RID: 28086 RVA: 0x002AD504 File Offset: 0x002AB704
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

	// Token: 0x06006DB7 RID: 28087 RVA: 0x002AD5E0 File Offset: 0x002AB7E0
	[XuiBindEvent("OnValueChanged", "cbxMustaches")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxMustachesOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyMustache(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DB8 RID: 28088 RVA: 0x002AD5F5 File Offset: 0x002AB7F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyMustache(string _name)
	{
		this.previewWindow.Archetype.MustacheName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.MustacheName = _name;
		}
	}

	// Token: 0x06006DB9 RID: 28089 RVA: 0x002AD61C File Offset: 0x002AB81C
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

	// Token: 0x06006DBA RID: 28090 RVA: 0x002AD6F8 File Offset: 0x002AB8F8
	[XuiBindEvent("OnValueChanged", "cbxChops")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxChopsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyChops(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DBB RID: 28091 RVA: 0x002AD70D File Offset: 0x002AB90D
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyChops(string _name)
	{
		this.previewWindow.Archetype.ChopsName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.ChopsName = _name;
		}
	}

	// Token: 0x06006DBC RID: 28092 RVA: 0x002AD734 File Offset: 0x002AB934
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

	// Token: 0x06006DBD RID: 28093 RVA: 0x002AD810 File Offset: 0x002ABA10
	[XuiBindEvent("OnValueChanged", "cbxBeards")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void cbxBeardsOnValueChanged(XUiController _sender, XUiC_CustomCharacterWindowGroup.NameData _oldValue, XUiC_CustomCharacterWindowGroup.NameData _newValue)
	{
		this.applyBeard(_newValue.InternalName);
		this.updatePreview(true);
	}

	// Token: 0x06006DBE RID: 28094 RVA: 0x002AD825 File Offset: 0x002ABA25
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyBeard(string _name)
	{
		this.previewWindow.Archetype.BeardName = _name;
		if (this.PlayerProfile != null)
		{
			this.PlayerProfile.BeardName = _name;
		}
	}

	// Token: 0x06006DBF RID: 28095 RVA: 0x002AD84C File Offset: 0x002ABA4C
	public static void Open(XUi _xui, Action _onClosed)
	{
		XUiC_CustomCharacterWindowGroup childByType = _xui.GetChildByType<XUiC_CustomCharacterWindowGroup>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.WindowGroup, false);
	}

	// Token: 0x040053B6 RID: 21430
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasChanges;

	// Token: 0x040053B7 RID: 21431
	public PlayerProfile PlayerProfile;

	// Token: 0x040053B8 RID: 21432
	public Archetype Archetype;

	// Token: 0x040053B9 RID: 21433
	[XuiBindComponent("cbxRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxRace;

	// Token: 0x040053BA RID: 21434
	[XuiBindComponent("cbxGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxGender;

	// Token: 0x040053BB RID: 21435
	[XuiBindComponent("cbxEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxEyeColor;

	// Token: 0x040053BC RID: 21436
	[XuiBindComponent("cbxHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairStyle;

	// Token: 0x040053BD RID: 21437
	[XuiBindComponent("cbxHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxHairColor;

	// Token: 0x040053BE RID: 21438
	[XuiBindComponent("cbxMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxMustaches;

	// Token: 0x040053BF RID: 21439
	[XuiBindComponent("cbxChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxChops;

	// Token: 0x040053C0 RID: 21440
	[XuiBindComponent("cbxBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxBeards;

	// Token: 0x040053C1 RID: 21441
	[XuiBindComponent("cbxFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_CustomCharacterWindowGroup.NameData> cbxVariant;

	// Token: 0x040053C2 RID: 21442
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x040053C3 RID: 21443
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053C4 RID: 21444
	[XuiBindComponent("btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x040053C5 RID: 21445
	[XuiBindComponent("btnRandomize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRandomize;

	// Token: 0x040053C6 RID: 21446
	[XuiBindComponent("btnLockRace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockRace;

	// Token: 0x040053C7 RID: 21447
	[XuiBindComponent("btnLockGender", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockGender;

	// Token: 0x040053C8 RID: 21448
	[XuiBindComponent("btnLockFace", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockFace;

	// Token: 0x040053C9 RID: 21449
	[XuiBindComponent("btnLockEyeColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockEyeColor;

	// Token: 0x040053CA RID: 21450
	[XuiBindComponent("btnLockHairStyle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairStyle;

	// Token: 0x040053CB RID: 21451
	[XuiBindComponent("btnLockHairColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockHairColor;

	// Token: 0x040053CC RID: 21452
	[XuiBindComponent("btnLockMustaches", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockMustaches;

	// Token: 0x040053CD RID: 21453
	[XuiBindComponent("btnLockChops", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockChops;

	// Token: 0x040053CE RID: 21454
	[XuiBindComponent("btnLockBeards", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnLockBeards;

	// Token: 0x040053CF RID: 21455
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom gr;

	// Token: 0x040053D0 RID: 21456
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosed;

	// Token: 0x02000E0E RID: 3598
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct NameData
	{
		// Token: 0x06006DC3 RID: 28099 RVA: 0x002AD892 File Offset: 0x002ABA92
		public NameData(string _internalName, string _formattedName)
		{
			this.InternalName = _internalName;
			this.formattedName = _formattedName;
		}

		// Token: 0x06006DC4 RID: 28100 RVA: 0x002AD8A2 File Offset: 0x002ABAA2
		public override string ToString()
		{
			return this.formattedName;
		}

		// Token: 0x040053D1 RID: 21457
		public readonly string InternalName;

		// Token: 0x040053D2 RID: 21458
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string formattedName;
	}
}
