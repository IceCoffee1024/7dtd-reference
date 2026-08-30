using System;
using UnityEngine.Scripting;

// Token: 0x02000E68 RID: 3688
[Preserve]
public class XUiC_WorldSelectionPopup : XUiController
{
	// Token: 0x17000CED RID: 3309
	// (get) Token: 0x060071A4 RID: 29092 RVA: 0x002B9330 File Offset: 0x002B7530
	// (set) Token: 0x060071A5 RID: 29093 RVA: 0x002B9338 File Offset: 0x002B7538
	[XuiXmlBinding("msgTitle")]
	public string MsgTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = string.Empty;

	// Token: 0x060071A6 RID: 29094 RVA: 0x002B9341 File Offset: 0x002B7541
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCancelButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.cancel();
	}

	// Token: 0x060071A7 RID: 29095 RVA: 0x002B9349 File Offset: 0x002B7549
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnConfirmButtonPressed(XUiController _sender, int _mouseButton)
	{
		this.confirm();
	}

	// Token: 0x17000CEE RID: 3310
	// (get) Token: 0x060071A8 RID: 29096 RVA: 0x002B9351 File Offset: 0x002B7551
	[XuiXmlBinding("isWorldSelected")]
	public bool IsWorldSelected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_WorldSelectionList xuiC_WorldSelectionList = this.worldList;
			return xuiC_WorldSelectionList != null && xuiC_WorldSelectionList.HasSelection;
		}
	}

	// Token: 0x060071A9 RID: 29097 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldSelectionList.Entry> _list, XUiC_WorldSelectionList.Entry _previousEntry, XUiC_WorldSelectionList.Entry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071AA RID: 29098 RVA: 0x002B9364 File Offset: 0x002B7564
	public override void Init()
	{
		base.Init();
		XUiC_WorldSelectionPopup.id = this.windowGroup.Id;
	}

	// Token: 0x060071AB RID: 29099 RVA: 0x002B937C File Offset: 0x002B757C
	[PublicizedFrom(EAccessModifier.Private)]
	public void open(string _titleCaptionKey, string _worldName, Action _onCancel, Action<PathAbstractions.AbstractedLocation> _onConfirm)
	{
		this.MsgTitle = _titleCaptionKey;
		this.onCancel = _onCancel;
		this.onConfirm = _onConfirm;
		this.worldList.SetFilter(_worldName);
		if (this.worldList.EntryCount == 1)
		{
			if (_onConfirm != null)
			{
				_onConfirm(this.worldList.GetEntry(0).Location);
			}
			return;
		}
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
	}

	// Token: 0x060071AC RID: 29100 RVA: 0x002B93F2 File Offset: 0x002B75F2
	public static void Open(XUi _xui, string _titleCaptionKey, string _worldName, Action _onCancel, Action<PathAbstractions.AbstractedLocation> _onConfirm)
	{
		((XUiC_WorldSelectionPopup)_xui.FindWindowGroupByName(XUiC_WorldSelectionPopup.id)).open(_titleCaptionKey, _worldName, _onCancel, _onConfirm);
	}

	// Token: 0x060071AD RID: 29101 RVA: 0x002B9410 File Offset: 0x002B7610
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false))
		{
			if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasReleased)
			{
				this.cancel();
				return;
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasPressed)
			{
				this.confirm();
				return;
			}
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
		}
	}

	// Token: 0x060071AE RID: 29102 RVA: 0x002B94A5 File Offset: 0x002B76A5
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancel()
	{
		Action action = this.onCancel;
		if (action != null)
		{
			action();
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x060071AF RID: 29103 RVA: 0x002B94D4 File Offset: 0x002B76D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void confirm()
	{
		if (!this.worldList.HasSelection)
		{
			return;
		}
		Action<PathAbstractions.AbstractedLocation> action = this.onConfirm;
		if (action != null)
		{
			action(this.worldList.SelectedEntryData.Location);
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x060071B0 RID: 29104 RVA: 0x002B952C File Offset: 0x002B772C
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onConfirm = null;
	}

	// Token: 0x0400559F RID: 21919
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040055A1 RID: 21921
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040055A2 RID: 21922
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x040055A3 RID: 21923
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x040055A4 RID: 21924
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<PathAbstractions.AbstractedLocation> onConfirm;

	// Token: 0x040055A5 RID: 21925
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldSelectionList worldList;
}
