using System;
using UnityEngine.Scripting;

// Token: 0x0200108E RID: 4238
[Preserve]
public class XUiC_StartPointEditor : XUiController
{
	// Token: 0x0600866D RID: 34413 RVA: 0x00335DF4 File Offset: 0x00333FF4
	public override void Init()
	{
		base.Init();
		XUiC_StartPointEditor.ID = base.WindowGroup.Id;
		this.cbxHeading = base.GetChildById("cbxHeading").GetChildByType<XUiC_ComboBoxInt>();
		this.cbxHeading.OnValueChanged += this.CbxHeading_OnValueChanged;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnOk")).OnPressed += this.BtnOk_OnPressed;
	}

	// Token: 0x0600866E RID: 34414 RVA: 0x002AAE45 File Offset: 0x002A9045
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600866F RID: 34415 RVA: 0x00335E86 File Offset: 0x00334086
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.headingOnOpen = this.cbxHeading.Value;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06008670 RID: 34416 RVA: 0x00335EB8 File Offset: 0x003340B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxHeading_OnValueChanged(XUiController _sender, long _oldvalue, long _newvalue)
	{
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)_newvalue);
		base.RefreshBindings();
	}

	// Token: 0x06008671 RID: 34417 RVA: 0x00335EEC File Offset: 0x003340EC
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

	// Token: 0x06008672 RID: 34418 RVA: 0x00335F70 File Offset: 0x00334170
	public override void OnClose()
	{
		base.OnClose();
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)this.headingOnOpen);
		this.spawnPoint = null;
		this.selectionBox = null;
	}

	// Token: 0x06008673 RID: 34419 RVA: 0x00335FB8 File Offset: 0x003341B8
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

	// Token: 0x040063E8 RID: 25576
	public static string ID = "";

	// Token: 0x040063E9 RID: 25577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxHeading;

	// Token: 0x040063EA RID: 25578
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPoint spawnPoint;

	// Token: 0x040063EB RID: 25579
	[PublicizedFrom(EAccessModifier.Private)]
	public SelectionBox selectionBox;

	// Token: 0x040063EC RID: 25580
	[PublicizedFrom(EAccessModifier.Private)]
	public long headingOnOpen;
}
