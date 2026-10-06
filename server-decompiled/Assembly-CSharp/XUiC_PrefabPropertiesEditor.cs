using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000FE1 RID: 4065
[Preserve]
public class XUiC_PrefabPropertiesEditor : XUiController
{
	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x06007FF8 RID: 32760 RVA: 0x0030A8DE File Offset: 0x00308ADE
	// (set) Token: 0x06007FF9 RID: 32761 RVA: 0x0030A8E8 File Offset: 0x00308AE8
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

	// Token: 0x06007FFA RID: 32762 RVA: 0x0030A930 File Offset: 0x00308B30
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

	// Token: 0x06007FFB RID: 32763 RVA: 0x0030AA8C File Offset: 0x00308C8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void featureChangedCallback(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected)
	{
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06007FFC RID: 32764 RVA: 0x0030AAA1 File Offset: 0x00308CA1
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOpenInEditor_OnOnPressed(XUiController _sender, int _mouseButton)
	{
		Process.Start(this.prefab.location.FullPathNoExtension + ".xml");
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007FFD RID: 32765 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007FFE RID: 32766 RVA: 0x0030AAE0 File Offset: 0x00308CE0
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

	// Token: 0x06007FFF RID: 32767 RVA: 0x0030AB40 File Offset: 0x00308D40
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

	// Token: 0x06008000 RID: 32768 RVA: 0x0030ABD0 File Offset: 0x00308DD0
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

	// Token: 0x06008001 RID: 32769 RVA: 0x0030AC10 File Offset: 0x00308E10
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

	// Token: 0x06008002 RID: 32770 RVA: 0x0030ACBC File Offset: 0x00308EBC
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

	// Token: 0x06008003 RID: 32771 RVA: 0x0030AD67 File Offset: 0x00308F67
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
		this.prefab = null;
	}

	// Token: 0x06008004 RID: 32772 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06008005 RID: 32773 RVA: 0x0030AD94 File Offset: 0x00308F94
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

	// Token: 0x06008006 RID: 32774 RVA: 0x0030AE10 File Offset: 0x00309010
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

	// Token: 0x04005EEB RID: 24299
	public static string ID = "";

	// Token: 0x04005EEC RID: 24300
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PrefabFeatureEditorList> featureLists = new List<XUiC_PrefabFeatureEditorList>();

	// Token: 0x04005EED RID: 24301
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxDifficultyTier;

	// Token: 0x04005EEE RID: 24302
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtThemeRepeatDistance;

	// Token: 0x04005EEF RID: 24303
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtDuplicateRepeatDistance;

	// Token: 0x04005EF0 RID: 24304
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabPropertiesEditor.EPropertiesFrom propertiesFrom;

	// Token: 0x04005EF1 RID: 24305
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x02000FE2 RID: 4066
	public enum EPropertiesFrom
	{
		// Token: 0x04005EF3 RID: 24307
		LoadedPrefab,
		// Token: 0x04005EF4 RID: 24308
		FileBrowserSelection
	}
}
