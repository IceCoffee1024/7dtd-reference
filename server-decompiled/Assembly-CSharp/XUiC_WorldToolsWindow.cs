using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010BF RID: 4287
[Preserve]
public class XUiC_WorldToolsWindow : XUiController
{
	// Token: 0x0600886E RID: 34926 RVA: 0x00341A74 File Offset: 0x0033FC74
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

	// Token: 0x0600886F RID: 34927 RVA: 0x00341B5C File Offset: 0x0033FD5C
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

	// Token: 0x06008870 RID: 34928 RVA: 0x00341BDF File Offset: 0x0033FDDF
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSelectionCaptions_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		SelectionBoxManager.Instance.CategoryDynamicPrefab.SetCaptionVisibility(_newValue);
	}

	// Token: 0x06008871 RID: 34929 RVA: 0x00341BF1 File Offset: 0x0033FDF1
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxSideTransparency_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		SelectionBoxManager.Instance.AlphaMultiplier = (float)_newValue;
	}

	// Token: 0x06008872 RID: 34930 RVA: 0x00341BFF File Offset: 0x0033FDFF
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxBoxPrefabPreviewLimit_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		DynamicPrefabDecorator.PrefabPreviewLimit = (int)_newValue;
	}

	// Token: 0x06008873 RID: 34931 RVA: 0x00341C08 File Offset: 0x0033FE08
	public override void OnOpen()
	{
		base.OnOpen();
		this.cbxBoxSideTransparency.Value = (double)SelectionBoxManager.Instance.AlphaMultiplier;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.btnLevelStartPoint.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04006559 RID: 25945
	public static string ID = "";

	// Token: 0x0400655A RID: 25946
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnLevelStartPoint;

	// Token: 0x0400655B RID: 25947
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxBoxSideTransparency;

	// Token: 0x0400655C RID: 25948
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxBoxSelectionCaptions;

	// Token: 0x0400655D RID: 25949
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxBoxPrefabPreviewLimit;
}
