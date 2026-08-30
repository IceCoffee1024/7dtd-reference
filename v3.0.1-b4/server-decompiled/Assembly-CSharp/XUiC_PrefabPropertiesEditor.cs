using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000FB9 RID: 4025
[Preserve]
public class XUiC_PrefabPropertiesEditor : XUiController
{
	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x06007EF7 RID: 32503 RVA: 0x00303132 File Offset: 0x00301332
	// (set) Token: 0x06007EF8 RID: 32504 RVA: 0x0030313C File Offset: 0x0030133C
	public Prefab Prefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.prefab;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value != this.prefab)
			{
				this.prefab = value;
				for (int i = 0; i < this.featureLists.Count; i++)
				{
					this.featureLists[i].EditPrefab = value;
				}
			}
		}
	}

	// Token: 0x06007EF9 RID: 32505 RVA: 0x00303184 File Offset: 0x00301384
	public override void Init()
	{
		base.Init();
		XUiC_PrefabPropertiesEditor.ID = base.WindowGroup.Id;
		foreach (XUiC_PrefabFeatureEditorList xuiC_PrefabFeatureEditorList in base.GetChildrenByType<XUiC_PrefabFeatureEditorList>(null))
		{
			this.featureLists.Add(xuiC_PrefabFeatureEditorList);
			xuiC_PrefabFeatureEditorList.FeatureChanged += this.featureChangedCallback;
		}
		XUiC_ComboBoxInt xuiC_ComboBoxInt = base.GetChildById("cbxDifficultyTier") as XUiC_ComboBoxInt;
		if (xuiC_ComboBoxInt != null)
		{
			this.cbxDifficultyTier = xuiC_ComboBoxInt;
			this.cbxDifficultyTier.OnValueChanged += this.CbxDifficultyTier_OnValueChanged;
		}
		XUiC_TextInput xuiC_TextInput = base.GetChildById("txtThemeRepeatDistance") as XUiC_TextInput;
		if (xuiC_TextInput != null)
		{
			this.txtThemeRepeatDistance = xuiC_TextInput;
			this.txtThemeRepeatDistance.OnChangeHandler += this.TxtThemeRepeatDistance_OnChangeHandler;
		}
		XUiC_TextInput xuiC_TextInput2 = base.GetChildById("txtDuplicateRepeatDistance") as XUiC_TextInput;
		if (xuiC_TextInput2 != null)
		{
			this.txtDuplicateRepeatDistance = xuiC_TextInput2;
			this.txtDuplicateRepeatDistance.OnChangeHandler += this.TxtDuplicateRepeatDistance_OnChangeHandler;
		}
		((XUiC_SimpleButton)base.GetChildById("btnSave")).OnPressed += this.BtnSave_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnOpenInEditor")).OnPressed += this.BtnOpenInEditor_OnOnPressed;
	}

	// Token: 0x06007EFA RID: 32506 RVA: 0x003032E0 File Offset: 0x003014E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void featureChangedCallback(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected)
	{
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06007EFB RID: 32507 RVA: 0x003032F5 File Offset: 0x003014F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOpenInEditor_OnOnPressed(XUiController _sender, int _mouseButton)
	{
		Process.Start(this.prefab.location.FullPathNoExtension + ".xml");
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007EFC RID: 32508 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007EFD RID: 32509 RVA: 0x00303334 File Offset: 0x00301534
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.FileBrowserSelection)
		{
			this.prefab.SaveXMLData(this.prefab.location);
			PrefabEditModeManager.Instance.LoadXml(this.prefab.location);
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007EFE RID: 32510 RVA: 0x00303394 File Offset: 0x00301594
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.cbxDifficultyTier != null)
		{
			this.cbxDifficultyTier.Value = (long)((ulong)this.Prefab.DifficultyTier);
		}
		if (this.txtThemeRepeatDistance != null)
		{
			this.txtThemeRepeatDistance.Text = this.Prefab.ThemeRepeatDistance.ToString(NumberFormatInfo.InvariantInfo);
		}
		if (this.txtDuplicateRepeatDistance != null)
		{
			this.txtDuplicateRepeatDistance.Text = this.Prefab.DuplicateRepeatDistance.ToString(NumberFormatInfo.InvariantInfo);
		}
		this.IsDirty = true;
	}

	// Token: 0x06007EFF RID: 32511 RVA: 0x00303424 File Offset: 0x00301624
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxDifficultyTier_OnValueChanged(XUiController _sender, long _oldvalue, long _newvalue)
	{
		byte b = (byte)_newvalue;
		if (this.Prefab.DifficultyTier == b)
		{
			return;
		}
		this.Prefab.DifficultyTier = b;
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06007F00 RID: 32512 RVA: 0x00303464 File Offset: 0x00301664
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtThemeRepeatDistance_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		XUiC_TextInput xuiC_TextInput = (XUiC_TextInput)_sender;
		if (_text.Length < 1)
		{
			xuiC_TextInput.Text = "0";
		}
		else if (_text.Length > 1 && _text[0] == '0')
		{
			xuiC_TextInput.Text = _text.Substring(1);
		}
		int num;
		if (!int.TryParse(xuiC_TextInput.Text, out num) || num < 0)
		{
			xuiC_TextInput.Text = this.Prefab.ThemeRepeatDistance.ToString(NumberFormatInfo.InvariantInfo);
			return;
		}
		if (this.Prefab.ThemeRepeatDistance == num)
		{
			return;
		}
		this.Prefab.ThemeRepeatDistance = num;
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06007F01 RID: 32513 RVA: 0x00303510 File Offset: 0x00301710
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtDuplicateRepeatDistance_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		XUiC_TextInput xuiC_TextInput = (XUiC_TextInput)_sender;
		if (_text.Length < 1)
		{
			xuiC_TextInput.Text = "0";
		}
		else if (_text.Length > 1 && _text[0] == '0')
		{
			xuiC_TextInput.Text = _text.Substring(1);
		}
		int num;
		if (!int.TryParse(xuiC_TextInput.Text, out num) || num < 0)
		{
			xuiC_TextInput.Text = this.Prefab.DuplicateRepeatDistance.ToString(NumberFormatInfo.InvariantInfo);
			return;
		}
		if (this.Prefab.DuplicateRepeatDistance == num)
		{
			return;
		}
		this.Prefab.DuplicateRepeatDistance = num;
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06007F02 RID: 32514 RVA: 0x003035BB File Offset: 0x003017BB
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
		this.prefab = null;
	}

	// Token: 0x06007F03 RID: 32515 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007F04 RID: 32516 RVA: 0x003035E8 File Offset: 0x003017E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "fromprefabbrowser")
		{
			_value = (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.FileBrowserSelection).ToString();
			return true;
		}
		if (!(_bindingName == "title"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = Localization.Get("xuiPrefabProperties", false, null) + ": " + ((this.prefab != null) ? this.prefab.PrefabName : "-");
		return true;
	}

	// Token: 0x06007F05 RID: 32517 RVA: 0x00303664 File Offset: 0x00301864
	public static void Show(XUi _xui, XUiC_PrefabPropertiesEditor.EPropertiesFrom _from, PathAbstractions.AbstractedLocation _prefabLocation)
	{
		XUiC_PrefabPropertiesEditor childByType = ((XUiWindowGroup)_xui.playerUI.windowManager.GetWindow(XUiC_PrefabPropertiesEditor.ID)).Controller.GetChildByType<XUiC_PrefabPropertiesEditor>();
		childByType.propertiesFrom = _from;
		if (_from == XUiC_PrefabPropertiesEditor.EPropertiesFrom.FileBrowserSelection)
		{
			childByType.Prefab = new Prefab();
			childByType.Prefab.LoadXMLData(_prefabLocation);
		}
		else if (_from == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			childByType.Prefab = PrefabEditModeManager.Instance.VoxelPrefab;
		}
		_xui.playerUI.windowManager.Open(XUiC_PrefabPropertiesEditor.ID, true);
	}

	// Token: 0x04005DFB RID: 24059
	public static string ID = "";

	// Token: 0x04005DFC RID: 24060
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PrefabFeatureEditorList> featureLists = new List<XUiC_PrefabFeatureEditorList>();

	// Token: 0x04005DFD RID: 24061
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxDifficultyTier;

	// Token: 0x04005DFE RID: 24062
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtThemeRepeatDistance;

	// Token: 0x04005DFF RID: 24063
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtDuplicateRepeatDistance;

	// Token: 0x04005E00 RID: 24064
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabPropertiesEditor.EPropertiesFrom propertiesFrom;

	// Token: 0x04005E01 RID: 24065
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x02000FBA RID: 4026
	public enum EPropertiesFrom
	{
		// Token: 0x04005E03 RID: 24067
		LoadedPrefab,
		// Token: 0x04005E04 RID: 24068
		FileBrowserSelection
	}
}
