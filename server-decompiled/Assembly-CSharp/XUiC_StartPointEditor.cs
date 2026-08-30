using System;
using UnityEngine.Scripting;

// Token: 0x0200107E RID: 4222
[Preserve]
public class XUiC_StartPointEditor : XUiController
{
	// Token: 0x0600862B RID: 34347 RVA: 0x003324BC File Offset: 0x003306BC
	public override void Init()
	{
		base.Init();
		XUiC_StartPointEditor.ID = base.WindowGroup.Id;
		this.cbxHeading = base.GetChildById("cbxHeading").GetChildByType<XUiC_ComboBoxInt>();
		this.cbxHeading.OnValueChanged += this.CbxHeading_OnValueChanged;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnOk")).OnPressed += this.BtnOk_OnPressed;
	}

	// Token: 0x0600862C RID: 34348 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600862D RID: 34349 RVA: 0x0033254E File Offset: 0x0033074E
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.headingOnOpen = this.cbxHeading.Value;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600862E RID: 34350 RVA: 0x00332580 File Offset: 0x00330780
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxHeading_OnValueChanged(XUiController _sender, long _oldvalue, long _newvalue)
	{
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)_newvalue);
		base.RefreshBindings();
	}

	// Token: 0x0600862F RID: 34351 RVA: 0x003325B4 File Offset: 0x003307B4
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

	// Token: 0x06008630 RID: 34352 RVA: 0x00332638 File Offset: 0x00330838
	public override void OnClose()
	{
		base.OnClose();
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)this.headingOnOpen);
		this.spawnPoint = null;
		this.selectionBox = null;
	}

	// Token: 0x06008631 RID: 34353 RVA: 0x00332680 File Offset: 0x00330880
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

	// Token: 0x040063A8 RID: 25512
	public static string ID = "";

	// Token: 0x040063A9 RID: 25513
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxHeading;

	// Token: 0x040063AA RID: 25514
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPoint spawnPoint;

	// Token: 0x040063AB RID: 25515
	[PublicizedFrom(EAccessModifier.Private)]
	public SelectionBox selectionBox;

	// Token: 0x040063AC RID: 25516
	[PublicizedFrom(EAccessModifier.Private)]
	public long headingOnOpen;
}
