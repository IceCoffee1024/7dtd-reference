using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101A RID: 4122
[Preserve]
public class XUiC_SaveDirtyPrefab : XUiController
{
	// Token: 0x0600825D RID: 33373 RVA: 0x00317BB4 File Offset: 0x00315DB4
	public override void Init()
	{
		base.Init();
		XUiC_SaveDirtyPrefab.ID = base.WindowGroup.Id;
		((XUiC_SimpleButton)base.GetChildById("btnSave")).OnPressed += this.BtnSave_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnDontSave")).OnPressed += this.BtnDontSave_OnPressed;
		this.txtSaveName = (XUiC_TextInput)base.GetChildById("txtSaveName");
		this.txtSaveName.OnChangeHandler += this.TxtSaveNameOnOnChangeHandler;
	}

	// Token: 0x0600825E RID: 33374 RVA: 0x00317C68 File Offset: 0x00315E68
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtSaveNameOnOnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (this.nameRequired)
		{
			this.nameInvalid = _text.Length <= 0 || _text.Contains(" ") || !GameUtils.ValidateGameName(_text);
			this.nameExists = !this.nameInvalid && Prefab.LocationForNewPrefab(_text, null).Exists();
		}
		else
		{
			this.nameInvalid = false;
			this.nameExists = false;
		}
		base.RefreshBindings();
	}

	// Token: 0x0600825F RID: 33375 RVA: 0x00317CDB File Offset: 0x00315EDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.Save);
	}

	// Token: 0x06008260 RID: 33376 RVA: 0x00317CE4 File Offset: 0x00315EE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.Cancel);
	}

	// Token: 0x06008261 RID: 33377 RVA: 0x00317CED File Offset: 0x00315EED
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDontSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.DontSave);
	}

	// Token: 0x06008262 RID: 33378 RVA: 0x00317CF8 File Offset: 0x00315EF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Save)
		{
			if (this.nameRequired)
			{
				string text = this.txtSaveName.Text;
				PrefabEditModeManager.Instance.VoxelPrefab.location = Prefab.LocationForNewPrefab(text, null);
			}
			if (PrefabEditModeManager.Instance.SaveVoxelPrefab())
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, string.Format(Localization.Get("xuiPrefabsPrefabSaved", false, null), PrefabEditModeManager.Instance.LoadedPrefab.Name), false, false, 0f);
			}
			else
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, Localization.Get("xuiPrefabsPrefabSavingError", false, null), false, false, 0f);
				_action = XUiC_SaveDirtyPrefab.ESelectedAction.Cancel;
			}
		}
		ThreadManager.StartCoroutine(this.delayCallback(_action));
	}

	// Token: 0x06008263 RID: 33379 RVA: 0x00317DD3 File Offset: 0x00315FD3
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayCallback(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		yield return new WaitForSeconds(0.1f);
		Action<XUiC_SaveDirtyPrefab.ESelectedAction> action = this.onCloseAction;
		if (action != null)
		{
			action(_action);
		}
		yield break;
	}

	// Token: 0x06008264 RID: 33380 RVA: 0x00317DE9 File Offset: 0x00315FE9
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtSaveName.Text = "";
		this.TxtSaveNameOnOnChangeHandler(this, "", true);
		this.IsDirty = true;
	}

	// Token: 0x06008265 RID: 33381 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06008266 RID: 33382 RVA: 0x00317E18 File Offset: 0x00316018
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		PathAbstractions.AbstractedLocation loadedPrefab = PrefabEditModeManager.Instance.LoadedPrefab;
		if (_bindingName == "is_save_new")
		{
			_value = (loadedPrefab.Type == PathAbstractions.EAbstractedLocationType.None).ToString();
			return true;
		}
		if (_bindingName == "current_prefab_name")
		{
			_value = ((loadedPrefab.Type != PathAbstractions.EAbstractedLocationType.None) ? loadedPrefab.Name : "");
			return true;
		}
		if (_bindingName == "request_name")
		{
			_value = this.nameRequired.ToString();
			return true;
		}
		if (_bindingName == "prefab_name_exists")
		{
			_value = this.nameExists.ToString();
			return true;
		}
		if (!(_bindingName == "prefab_name_invalid"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.nameInvalid.ToString();
		return true;
	}

	// Token: 0x06008267 RID: 33383 RVA: 0x00317ED8 File Offset: 0x003160D8
	public static void Show(XUi _xui, Action<XUiC_SaveDirtyPrefab.ESelectedAction> _onCloseAction, XUiC_SaveDirtyPrefab.EMode _mode = XUiC_SaveDirtyPrefab.EMode.AskSaveIfDirty)
	{
		PathAbstractions.AbstractedLocation loadedPrefab = PrefabEditModeManager.Instance.LoadedPrefab;
		if (_mode != XUiC_SaveDirtyPrefab.EMode.AskSaveIfDirty)
		{
			if (_mode == XUiC_SaveDirtyPrefab.EMode.ForceSave)
			{
				if (loadedPrefab.Type != PathAbstractions.EAbstractedLocationType.None && Prefab.CanSaveIn(loadedPrefab))
				{
					if (PrefabEditModeManager.Instance.SaveVoxelPrefab())
					{
						GameManager.ShowTooltip(_xui.playerUI.entityPlayer, string.Format(Localization.Get("xuiPrefabsPrefabSaved", false, null), loadedPrefab.Name), false, false, 0f);
						if (_onCloseAction != null)
						{
							_onCloseAction(XUiC_SaveDirtyPrefab.ESelectedAction.Save);
							return;
						}
					}
					else
					{
						GameManager.ShowTooltip(_xui.playerUI.entityPlayer, Localization.Get("xuiPrefabsPrefabSavingError", false, null), false, false, 0f);
						if (_onCloseAction != null)
						{
							_onCloseAction(XUiC_SaveDirtyPrefab.ESelectedAction.Cancel);
						}
					}
					return;
				}
			}
		}
		else if (PrefabEditModeManager.Instance.VoxelPrefab == null || !PrefabEditModeManager.Instance.NeedsSaving)
		{
			if (_onCloseAction != null)
			{
				_onCloseAction(XUiC_SaveDirtyPrefab.ESelectedAction.DontSave);
			}
			return;
		}
		XUiC_SaveDirtyPrefab childByType = ((XUiWindowGroup)_xui.playerUI.windowManager.GetWindow(XUiC_SaveDirtyPrefab.ID)).Controller.GetChildByType<XUiC_SaveDirtyPrefab>();
		childByType.nameRequired = loadedPrefab.Type == PathAbstractions.EAbstractedLocationType.None || !Prefab.CanSaveIn(loadedPrefab);
		childByType.onCloseAction = _onCloseAction;
		_xui.playerUI.windowManager.Open(XUiC_SaveDirtyPrefab.ID, true, true);
	}

	// Token: 0x04006087 RID: 24711
	public static string ID = "";

	// Token: 0x04006088 RID: 24712
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSaveName;

	// Token: 0x04006089 RID: 24713
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameRequired;

	// Token: 0x0400608A RID: 24714
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameInvalid;

	// Token: 0x0400608B RID: 24715
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameExists;

	// Token: 0x0400608C RID: 24716
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_SaveDirtyPrefab.ESelectedAction> onCloseAction;

	// Token: 0x0200101B RID: 4123
	public enum EMode
	{
		// Token: 0x0400608E RID: 24718
		AskSaveIfDirty,
		// Token: 0x0400608F RID: 24719
		ForceSave
	}

	// Token: 0x0200101C RID: 4124
	public enum ESelectedAction
	{
		// Token: 0x04006091 RID: 24721
		Save,
		// Token: 0x04006092 RID: 24722
		Cancel,
		// Token: 0x04006093 RID: 24723
		DontSave
	}
}
