using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001029 RID: 4137
[Preserve]
public class XUiC_SaveDirtyPrefab : XUiController
{
	// Token: 0x0600826C RID: 33388 RVA: 0x0031ABF8 File Offset: 0x00318DF8
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

	// Token: 0x0600826D RID: 33389 RVA: 0x0031ACAC File Offset: 0x00318EAC
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

	// Token: 0x0600826E RID: 33390 RVA: 0x0031AD1F File Offset: 0x00318F1F
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.Save);
	}

	// Token: 0x0600826F RID: 33391 RVA: 0x0031AD28 File Offset: 0x00318F28
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.Cancel);
	}

	// Token: 0x06008270 RID: 33392 RVA: 0x0031AD31 File Offset: 0x00318F31
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDontSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.DontSave);
	}

	// Token: 0x06008271 RID: 33393 RVA: 0x0031AD3C File Offset: 0x00318F3C
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

	// Token: 0x06008272 RID: 33394 RVA: 0x0031AE17 File Offset: 0x00319017
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

	// Token: 0x06008273 RID: 33395 RVA: 0x0031AE2D File Offset: 0x0031902D
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtSaveName.Text = "";
		this.TxtSaveNameOnOnChangeHandler(this, "", true);
		this.IsDirty = true;
	}

	// Token: 0x06008274 RID: 33396 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06008275 RID: 33397 RVA: 0x0031AE5C File Offset: 0x0031905C
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

	// Token: 0x06008276 RID: 33398 RVA: 0x0031AF1C File Offset: 0x0031911C
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

	// Token: 0x040060B0 RID: 24752
	public static string ID = "";

	// Token: 0x040060B1 RID: 24753
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSaveName;

	// Token: 0x040060B2 RID: 24754
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameRequired;

	// Token: 0x040060B3 RID: 24755
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameInvalid;

	// Token: 0x040060B4 RID: 24756
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameExists;

	// Token: 0x040060B5 RID: 24757
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_SaveDirtyPrefab.ESelectedAction> onCloseAction;

	// Token: 0x0200102A RID: 4138
	public enum EMode
	{
		// Token: 0x040060B7 RID: 24759
		AskSaveIfDirty,
		// Token: 0x040060B8 RID: 24760
		ForceSave
	}

	// Token: 0x0200102B RID: 4139
	public enum ESelectedAction
	{
		// Token: 0x040060BA RID: 24762
		Save,
		// Token: 0x040060BB RID: 24763
		Cancel,
		// Token: 0x040060BC RID: 24764
		DontSave
	}
}
