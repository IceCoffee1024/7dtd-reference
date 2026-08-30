using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000FD2 RID: 4050
[Preserve]
public class XUiC_PrefabPropertiesEditor : XUiController
{
	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x06007FE9 RID: 32745 RVA: 0x00307906 File Offset: 0x00305B06
	// (set) Token: 0x06007FEA RID: 32746 RVA: 0x00307910 File Offset: 0x00305B10
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

	// Token: 0x06007FEB RID: 32747 RVA: 0x00307958 File Offset: 0x00305B58
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

	// Token: 0x06007FEC RID: 32748 RVA: 0x00307AB4 File Offset: 0x00305CB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void featureChangedCallback(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected)
	{
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06007FED RID: 32749 RVA: 0x00307AC9 File Offset: 0x00305CC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOpenInEditor_OnOnPressed(XUiController _sender, int _mouseButton)
	{
		Process.Start(this.prefab.location.FullPathNoExtension + ".xml");
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007FEE RID: 32750 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007FEF RID: 32751 RVA: 0x00307B08 File Offset: 0x00305D08
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

	// Token: 0x06007FF0 RID: 32752 RVA: 0x00307B68 File Offset: 0x00305D68
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

	// Token: 0x06007FF1 RID: 32753 RVA: 0x00307BF8 File Offset: 0x00305DF8
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

	// Token: 0x06007FF2 RID: 32754 RVA: 0x00307C38 File Offset: 0x00305E38
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

	// Token: 0x06007FF3 RID: 32755 RVA: 0x00307CE4 File Offset: 0x00305EE4
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

	// Token: 0x06007FF4 RID: 32756 RVA: 0x00307D8F File Offset: 0x00305F8F
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
		this.prefab = null;
	}

	// Token: 0x06007FF5 RID: 32757 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007FF6 RID: 32758 RVA: 0x00307DBC File Offset: 0x00305FBC
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

	// Token: 0x06007FF7 RID: 32759 RVA: 0x00307E38 File Offset: 0x00306038
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

	// Token: 0x04005EC2 RID: 24258
	public static string ID = "";

	// Token: 0x04005EC3 RID: 24259
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PrefabFeatureEditorList> featureLists = new List<XUiC_PrefabFeatureEditorList>();

	// Token: 0x04005EC4 RID: 24260
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxDifficultyTier;

	// Token: 0x04005EC5 RID: 24261
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtThemeRepeatDistance;

	// Token: 0x04005EC6 RID: 24262
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtDuplicateRepeatDistance;

	// Token: 0x04005EC7 RID: 24263
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabPropertiesEditor.EPropertiesFrom propertiesFrom;

	// Token: 0x04005EC8 RID: 24264
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x02000FD3 RID: 4051
	public enum EPropertiesFrom
	{
		// Token: 0x04005ECA RID: 24266
		LoadedPrefab,
		// Token: 0x04005ECB RID: 24267
		FileBrowserSelection
	}
}
