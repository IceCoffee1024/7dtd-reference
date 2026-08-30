using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000600 RID: 1536
[Preserve]
public class ItemClassTorch : ItemClass
{
	// Token: 0x06003222 RID: 12834 RVA: 0x00145AF8 File Offset: 0x00143CF8
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

	// Token: 0x06003223 RID: 12835 RVA: 0x00145B38 File Offset: 0x00143D38
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

	// Token: 0x06003224 RID: 12836 RVA: 0x00145B76 File Offset: 0x00143D76
	public override BlockValue OnConvertToBlockValue(ItemValue _itemValue, BlockValue _blueprintBlockValue)
	{
		_blueprintBlockValue.meta = (byte)((int)_itemValue.UseTimes & 15);
		_blueprintBlockValue.meta2 = (byte)(((int)_itemValue.UseTimes >> 8) & 15);
		return _blueprintBlockValue;
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x00010E62 File Offset: 0x0000F062
	public override RenderCubeType GetFocusType(ItemInventoryData _data)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x00145BA0 File Offset: 0x00143DA0
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
