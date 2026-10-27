using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000FF1 RID: 4081
[Preserve]
public class XUiC_PrefabPropertiesEditor : XUiController
{
	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x06008047 RID: 32839 RVA: 0x0030C3FA File Offset: 0x0030A5FA
	// (set) Token: 0x06008048 RID: 32840 RVA: 0x0030C404 File Offset: 0x0030A604
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

	// Token: 0x06008049 RID: 32841 RVA: 0x0030C44C File Offset: 0x0030A64C
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

	// Token: 0x0600804A RID: 32842 RVA: 0x0030C5A8 File Offset: 0x0030A7A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void featureChangedCallback(XUiC_PrefabFeatureEditorList _list, string _featureName, bool _selected)
	{
		if (this.propertiesFrom == XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x0600804B RID: 32843 RVA: 0x0030C5BD File Offset: 0x0030A7BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOpenInEditor_OnOnPressed(XUiController _sender, int _mouseButton)
	{
		Process.Start(this.prefab.location.FullPathNoExtension + ".xml");
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600804C RID: 32844 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600804D RID: 32845 RVA: 0x0030C5FC File Offset: 0x0030A7FC
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

	// Token: 0x0600804E RID: 32846 RVA: 0x0030C65C File Offset: 0x0030A85C
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

	// Token: 0x0600804F RID: 32847 RVA: 0x0030C6EC File Offset: 0x0030A8EC
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

	// Token: 0x06008050 RID: 32848 RVA: 0x0030C72C File Offset: 0x0030A92C
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

	// Token: 0x06008051 RID: 32849 RVA: 0x0030C7D8 File Offset: 0x0030A9D8
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

	// Token: 0x06008052 RID: 32850 RVA: 0x0030C883 File Offset: 0x0030AA83
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
		this.prefab = null;
	}

	// Token: 0x06008053 RID: 32851 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06008054 RID: 32852 RVA: 0x0030C8B0 File Offset: 0x0030AAB0
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

	// Token: 0x06008055 RID: 32853 RVA: 0x0030C92C File Offset: 0x0030AB2C
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

	// Token: 0x04005F26 RID: 24358
	public static string ID = "";

	// Token: 0x04005F27 RID: 24359
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PrefabFeatureEditorList> featureLists = new List<XUiC_PrefabFeatureEditorList>();

	// Token: 0x04005F28 RID: 24360
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxDifficultyTier;

	// Token: 0x04005F29 RID: 24361
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtThemeRepeatDistance;

	// Token: 0x04005F2A RID: 24362
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtDuplicateRepeatDistance;

	// Token: 0x04005F2B RID: 24363
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabPropertiesEditor.EPropertiesFrom propertiesFrom;

	// Token: 0x04005F2C RID: 24364
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x02000FF2 RID: 4082
	public enum EPropertiesFrom
	{
		// Token: 0x04005F2E RID: 24366
		LoadedPrefab,
		// Token: 0x04005F2F RID: 24367
		FileBrowserSelection
	}
}
