using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010F9 RID: 4345
[Preserve]
public class XUiC_WorldToolsWindow : XUiController
{
	// Token: 0x06008A01 RID: 35329 RVA: 0x0034C0CC File Offset: 0x0034A2CC
	public override void Init()
	{
		base.Init();
		XUiC_WorldToolsWindow.ID = base.WindowGroup.Id;
		this.btnLevelStartPoint = base.GetChildById("btnLevelStartPoint");
		this.btnLevelStartPoint.GetChildById("clickable").OnPress += this.BtnLevelStartPoint_Controller_OnPress;
		this.cbxBoxSideTransparency = base.GetChildById("cbxBoxSideTransparency").GetChildByType<XUiC_ComboBoxFloat>();
		this.cbxBoxSideTransparency.OnValueChanged += this.CbxBoxSideTransparency_OnValueChanged;
		this.cbxBoxSelectionCaptions = base.GetChildById("cbxBoxSelectionCaptions").GetChildByType<XUiC_ComboBoxBool>();
		this.cbxBoxSelectionCaptions.OnValueChanged += this.CbxBoxSelectionCaptions_OnValueChanged;
		this.cbxBoxSelectionCaptions.Value = true;
		this.cbxBoxPrefabPreviewLimit = base.GetChildById("cbxBoxPrefabPreviewLimit").GetChildByType<XUiC_ComboBoxInt>();
		this.cbxBoxPrefabPreviewLimit.OnValueChanged += this.CbxBoxPrefabPreviewLimit_OnValueChanged;
	}

	// Token: 0x06008A02 RID: 35330 RVA: 0x0034C1B4 File Offset: 0x0034A3B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLevelStartPoint_Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		Vector3 raycastHitPoint = XUiC_LevelTools3Window.getRaycastHitPoint(100f, 0f);
		if (!raycastHitPoint.Equals(Vector3.zero))
		{
			Vector3i vector3i = World.worldToBlockPos(raycastHitPoint);
			GameManager.Instance.GetSpawnPointList().Add(new SpawnPoint(vector3i));
			SelectionCategory categoryStartPoint = SelectionBoxManager.Instance.CategoryStartPoint;
			Vector3i vector3i2 = vector3i;
			SelectionBox selectionBox = categoryStartPoint.AddBox(vector3i2.ToString() ?? "", vector3i, Vector3i.one, true, false);
			SelectionBoxManager.Instance.SetActive(selectionBox, true);
		}
	}

	// Token: 0x06008A03 RID: 35331 RVA: 0x0034C237 File Offset: 0x0034A437
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSelectionCaptions_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		SelectionBoxManager.Instance.CategoryDynamicPrefab.SetCaptionVisibility(_newValue);
	}

	// Token: 0x06008A04 RID: 35332 RVA: 0x0034C249 File Offset: 0x0034A449
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSideTransparency_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		SelectionBoxManager.Instance.AlphaMultiplier = (float)_newValue;
	}

	// Token: 0x06008A05 RID: 35333 RVA: 0x0034C257 File Offset: 0x0034A457
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxPrefabPreviewLimit_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		DynamicPrefabDecorator.PrefabPreviewLimit = (int)_newValue;
	}

	// Token: 0x06008A06 RID: 35334 RVA: 0x0034C260 File Offset: 0x0034A460
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxBoxSideTransparency.Value = (double)SelectionBoxManager.Instance.AlphaMultiplier;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.btnLevelStartPoint.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x040066A1 RID: 26273
	public static string ID = "";

	// Token: 0x040066A2 RID: 26274
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnLevelStartPoint;

	// Token: 0x040066A3 RID: 26275
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxBoxSideTransparency;

	// Token: 0x040066A4 RID: 26276
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxBoxSelectionCaptions;

	// Token: 0x040066A5 RID: 26277
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxBoxPrefabPreviewLimit;
}
