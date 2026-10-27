using System;
using UnityEngine.Scripting;

// Token: 0x0200109E RID: 4254
[Preserve]
public class XUiC_StartPointEditor : XUiController
{
	// Token: 0x060086C0 RID: 34496 RVA: 0x00337E78 File Offset: 0x00336078
	public override void Init()
	{
		base.Init();
		XUiC_StartPointEditor.ID = base.WindowGroup.Id;
		this.cbxHeading = base.GetChildById("cbxHeading").GetChildByType<XUiC_ComboBoxInt>();
		this.cbxHeading.OnValueChanged += this.CbxHeading_OnValueChanged;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnOk")).OnPressed += this.BtnOk_OnPressed;
	}

	// Token: 0x060086C1 RID: 34497 RVA: 0x002AC36D File Offset: 0x002AA56D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x060086C2 RID: 34498 RVA: 0x00337F0A File Offset: 0x0033610A
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.headingOnOpen = this.cbxHeading.Value;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x060086C3 RID: 34499 RVA: 0x00337F3C File Offset: 0x0033613C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxHeading_OnValueChanged(XUiController _sender, long _oldvalue, long _newvalue)
	{
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)_newvalue);
		base.RefreshBindings();
	}

	// Token: 0x060086C4 RID: 34500 RVA: 0x00337F70 File Offset: 0x00336170
	public override void OnOpen()
	{
		base.OnOpen();
		this.selectionBox = SelectionBoxManager.Instance.Selection;
		if (this.selectionBox == null)
		{
			return;
		}
		this.spawnPoint = GameManager.Instance.GetSpawnPointList().Find(Vector3i.Parse(this.selectionBox.name));
		this.cbxHeading.Value = (this.headingOnOpen = (long)this.spawnPoint.spawnPosition.heading);
		base.RefreshBindings();
	}

	// Token: 0x060086C5 RID: 34501 RVA: 0x00337FF4 File Offset: 0x003361F4
	public override void OnClose()
	{
		base.OnClose();
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)this.headingOnOpen);
		this.spawnPoint = null;
		this.selectionBox = null;
	}

	// Token: 0x060086C6 RID: 34502 RVA: 0x0033803C File Offset: 0x0033623C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "position")
		{
			SpawnPoint spawnPoint = this.spawnPoint;
			_value = ((spawnPoint != null) ? spawnPoint.spawnPosition.position.ToCultureInvariantString() : null) ?? "";
			return true;
		}
		if (!(_bindingName == "cardinal"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		SpawnPoint spawnPoint2 = this.spawnPoint;
		_value = GameUtils.GetClosestDirection((spawnPoint2 != null) ? spawnPoint2.spawnPosition.heading : 0f, false).ToStringCached<GameUtils.DirEightWay>();
		return true;
	}

	// Token: 0x04006427 RID: 25639
	public static string ID = "";

	// Token: 0x04006428 RID: 25640
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxHeading;

	// Token: 0x04006429 RID: 25641
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPoint spawnPoint;

	// Token: 0x0400642A RID: 25642
	[PublicizedFrom(EAccessModifier.Private)]
	public SelectionBox selectionBox;

	// Token: 0x0400642B RID: 25643
	[PublicizedFrom(EAccessModifier.Private)]
	public long headingOnOpen;
}
