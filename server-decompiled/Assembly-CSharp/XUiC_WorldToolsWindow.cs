using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010D8 RID: 4312
[Preserve]
public class XUiC_WorldToolsWindow : XUiController
{
	// Token: 0x06008960 RID: 35168 RVA: 0x00346298 File Offset: 0x00344498
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

	// Token: 0x06008961 RID: 35169 RVA: 0x00346380 File Offset: 0x00344580
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

	// Token: 0x06008962 RID: 35170 RVA: 0x00346403 File Offset: 0x00344603
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSelectionCaptions_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		SelectionBoxManager.Instance.CategoryDynamicPrefab.SetCaptionVisibility(_newValue);
	}

	// Token: 0x06008963 RID: 35171 RVA: 0x00346415 File Offset: 0x00344615
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSideTransparency_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		SelectionBoxManager.Instance.AlphaMultiplier = (float)_newValue;
	}

	// Token: 0x06008964 RID: 35172 RVA: 0x00346423 File Offset: 0x00344623
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxPrefabPreviewLimit_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		DynamicPrefabDecorator.PrefabPreviewLimit = (int)_newValue;
	}

	// Token: 0x06008965 RID: 35173 RVA: 0x0034642C File Offset: 0x0034462C
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxBoxSideTransparency.Value = (double)SelectionBoxManager.Instance.AlphaMultiplier;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.btnLevelStartPoint.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04006620 RID: 26144
	public static string ID = "";

	// Token: 0x04006621 RID: 26145
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnLevelStartPoint;

	// Token: 0x04006622 RID: 26146
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxBoxSideTransparency;

	// Token: 0x04006623 RID: 26147
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxBoxSelectionCaptions;

	// Token: 0x04006624 RID: 26148
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxBoxPrefabPreviewLimit;
}
