using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001039 RID: 4153
[Preserve]
public class XUiC_SaveDirtyPrefab : XUiController
{
	// Token: 0x060082BE RID: 33470 RVA: 0x0031C8FC File Offset: 0x0031AAFC
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

	// Token: 0x060082BF RID: 33471 RVA: 0x0031C9B0 File Offset: 0x0031ABB0
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

	// Token: 0x060082C0 RID: 33472 RVA: 0x0031CA23 File Offset: 0x0031AC23
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.Save);
	}

	// Token: 0x060082C1 RID: 33473 RVA: 0x0031CA2C File Offset: 0x0031AC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.Cancel);
	}

	// Token: 0x060082C2 RID: 33474 RVA: 0x0031CA35 File Offset: 0x0031AC35
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDontSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CloseWith(XUiC_SaveDirtyPrefab.ESelectedAction.DontSave);
	}

	// Token: 0x060082C3 RID: 33475 RVA: 0x0031CA40 File Offset: 0x0031AC40
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

	// Token: 0x060082C4 RID: 33476 RVA: 0x0031CB1B File Offset: 0x0031AD1B
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

	// Token: 0x060082C5 RID: 33477 RVA: 0x0031CB31 File Offset: 0x0031AD31
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtSaveName.Text = "";
		this.TxtSaveNameOnOnChangeHandler(this, "", true);
		this.IsDirty = true;
	}

	// Token: 0x060082C6 RID: 33478 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060082C7 RID: 33479 RVA: 0x0031CB60 File Offset: 0x0031AD60
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

	// Token: 0x060082C8 RID: 33480 RVA: 0x0031CC20 File Offset: 0x0031AE20
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

	// Token: 0x040060EE RID: 24814
	public static string ID = "";

	// Token: 0x040060EF RID: 24815
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSaveName;

	// Token: 0x040060F0 RID: 24816
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameRequired;

	// Token: 0x040060F1 RID: 24817
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameInvalid;

	// Token: 0x040060F2 RID: 24818
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameExists;

	// Token: 0x040060F3 RID: 24819
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_SaveDirtyPrefab.ESelectedAction> onCloseAction;

	// Token: 0x0200103A RID: 4154
	public enum EMode
	{
		// Token: 0x040060F5 RID: 24821
		AskSaveIfDirty,
		// Token: 0x040060F6 RID: 24822
		ForceSave
	}

	// Token: 0x0200103B RID: 4155
	public enum ESelectedAction
	{
		// Token: 0x040060F8 RID: 24824
		Save,
		// Token: 0x040060F9 RID: 24825
		Cancel,
		// Token: 0x040060FA RID: 24826
		DontSave
	}
}
