using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010E9 RID: 4329
[Preserve]
public class XUiC_WorldToolsWindow : XUiController
{
	// Token: 0x060089AE RID: 35246 RVA: 0x0034A048 File Offset: 0x00348248
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

	// Token: 0x060089AF RID: 35247 RVA: 0x0034A130 File Offset: 0x00348330
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

	// Token: 0x060089B0 RID: 35248 RVA: 0x0034A1B3 File Offset: 0x003483B3
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSelectionCaptions_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		SelectionBoxManager.Instance.CategoryDynamicPrefab.SetCaptionVisibility(_newValue);
	}

	// Token: 0x060089B1 RID: 35249 RVA: 0x0034A1C5 File Offset: 0x003483C5
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSideTransparency_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		SelectionBoxManager.Instance.AlphaMultiplier = (float)_newValue;
	}

	// Token: 0x060089B2 RID: 35250 RVA: 0x0034A1D3 File Offset: 0x003483D3
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxPrefabPreviewLimit_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		DynamicPrefabDecorator.PrefabPreviewLimit = (int)_newValue;
	}

	// Token: 0x060089B3 RID: 35251 RVA: 0x0034A1DC File Offset: 0x003483DC
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxBoxSideTransparency.Value = (double)SelectionBoxManager.Instance.AlphaMultiplier;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.btnLevelStartPoint.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04006662 RID: 26210
	public static string ID = "";

	// Token: 0x04006663 RID: 26211
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnLevelStartPoint;

	// Token: 0x04006664 RID: 26212
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxBoxSideTransparency;

	// Token: 0x04006665 RID: 26213
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxBoxSelectionCaptions;

	// Token: 0x04006666 RID: 26214
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxBoxPrefabPreviewLimit;
}
