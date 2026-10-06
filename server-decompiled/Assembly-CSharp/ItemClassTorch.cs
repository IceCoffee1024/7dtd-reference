using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000602 RID: 1538
[Preserve]
public class ItemClassTorch : ItemClass
{
	// Token: 0x06003265 RID: 12901 RVA: 0x00146484 File Offset: 0x00144684
	public override void StartHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		base.StartHolding(_data, _modelTransform);
		if (_modelTransform == null)
		{
			return;
		}
		OnActivateItemGameObjectReference component = _modelTransform.GetComponent<OnActivateItemGameObjectReference>();
		if (component != null && !component.IsActivated())
		{
			component.ActivateItem(true);
		}
	}

	// Token: 0x06003266 RID: 12902 RVA: 0x001464C4 File Offset: 0x001446C4
	public override void StopHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		base.StopHolding(_data, _modelTransform);
		if (_modelTransform == null)
		{
			return;
		}
		OnActivateItemGameObjectReference component = _modelTransform.GetComponent<OnActivateItemGameObjectReference>();
		if (component != null && component.IsActivated())
		{
			component.ActivateItem(false);
		}
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x00146502 File Offset: 0x00144702
	public override BlockValue OnConvertToBlockValue(ItemValue _itemValue, BlockValue _blueprintBlockValue)
	{
		_blueprintBlockValue.meta = (byte)((int)_itemValue.UseTimes & 15);
		_blueprintBlockValue.meta2 = (byte)(((int)_itemValue.UseTimes >> 8) & 15);
		return _blueprintBlockValue;
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x00010E62 File Offset: 0x0000F062
	public override RenderCubeType GetFocusType(ItemInventoryData _data)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x0014652C File Offset: 0x0014472C
	public override void OnHoldingUpdate(ItemInventoryData _data)
	{
		base.OnHoldingUpdate(_data);
		if (_data.model == null)
		{
			return;
		}
		bool flag = _data.world.IsWaterInBounds(BoundsUtils.ExpandBounds(new Bounds(_data.model.position + new Vector3(0f, 1.2f, 0f), Vector3.one), -0.1f, -0.4f, -0.1f));
		OnActivateItemGameObjectReference component = _data.model.GetComponent<OnActivateItemGameObjectReference>();
		if (component != null)
		{
			component.ActivateItem(!flag);
			if (flag)
			{
				base.StopHoldingAudio(_data);
			}
		}
		MeshRenderer meshRenderer = _data.model.gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer != null)
		{
			meshRenderer.material.SetTextureScale("_TorchLit", flag ? Vector2.zero : Vector2.one);
			return;
		}
		Transform transform = _data.model.Find("candle");
		if (transform != null)
		{
			meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.material.SetFloat("_Is_Lit", (float)(flag ? 0 : 1));
			}
		}
		transform = _data.model.Find("candle_lod");
		if (transform != null)
		{
			meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.material.SetFloat("_Is_Lit", (float)(flag ? 0 : 1));
			}
		}
		transform = _data.model.Find("candle_lod2");
		if (transform != null)
		{
			meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.material.SetFloat("_Is_Lit", (float)(flag ? 0 : 1));
			}
		}
		Transform transform2 = _data.model.FindInChilds("close", false);
		if (transform2 != null)
		{
			transform2.gameObject.SetActive(!flag);
		}
		transform2 = _data.model.FindInChilds("distance", false);
		if (transform2 != null)
		{
			transform2.gameObject.SetActive(!flag);
		}
		Transform transform3 = _data.model.FindInChilds("LightMover", false);
		if (transform3 != null)
		{
			transform3.gameObject.SetActive(!flag);
		}
	}
}
