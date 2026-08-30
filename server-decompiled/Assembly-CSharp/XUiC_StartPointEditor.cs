using System;
using UnityEngine.Scripting;

// Token: 0x02001065 RID: 4197
[Preserve]
public class XUiC_StartPointEditor : XUiController
{
	// Token: 0x06008539 RID: 34105 RVA: 0x0032DC94 File Offset: 0x0032BE94
	public override void Init()
	{
		base.Init();
		XUiC_StartPointEditor.ID = base.WindowGroup.Id;
		this.cbxHeading = base.GetChildById("cbxHeading").GetChildByType<XUiC_ComboBoxInt>();
		this.cbxHeading.OnValueChanged += this.CbxHeading_OnValueChanged;
		((XUiC_SimpleButton)base.GetChildById("btnCancel")).OnPressed += this.BtnCancel_OnPressed;
		((XUiC_SimpleButton)base.GetChildById("btnOk")).OnPressed += this.BtnOk_OnPressed;
	}

	// Token: 0x0600853A RID: 34106 RVA: 0x002A4621 File Offset: 0x002A2821
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600853B RID: 34107 RVA: 0x0032DD26 File Offset: 0x0032BF26
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.headingOnOpen = this.cbxHeading.Value;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600853C RID: 34108 RVA: 0x0032DD58 File Offset: 0x0032BF58
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxHeading_OnValueChanged(XUiController _sender, long _oldvalue, long _newvalue)
	{
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)_newvalue);
		base.RefreshBindings();
	}

	// Token: 0x0600853D RID: 34109 RVA: 0x0032DD8C File Offset: 0x0032BF8C
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

	// Token: 0x0600853E RID: 34110 RVA: 0x0032DE10 File Offset: 0x0032C010
	public override void OnClose()
	{
		base.OnClose();
		this.spawnPoint.spawnPosition.heading = (this.selectionBox.FacingDirection = (float)this.headingOnOpen);
		this.spawnPoint = null;
		this.selectionBox = null;
	}

	// Token: 0x0600853F RID: 34111 RVA: 0x0032DE58 File Offset: 0x0032C058
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

	// Token: 0x040062E1 RID: 25313
	public static string ID = "";

	// Token: 0x040062E2 RID: 25314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxHeading;

	// Token: 0x040062E3 RID: 25315
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPoint spawnPoint;

	// Token: 0x040062E4 RID: 25316
	[PublicizedFrom(EAccessModifier.Private)]
	public SelectionBox selectionBox;

	// Token: 0x040062E5 RID: 25317
	[PublicizedFrom(EAccessModifier.Private)]
	public long headingOnOpen;
}
