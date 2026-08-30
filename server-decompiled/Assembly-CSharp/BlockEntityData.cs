using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003DC RID: 988
[Preserve]
public class BlockEntityData
{
	// Token: 0x06001DF5 RID: 7669 RVA: 0x0000640C File Offset: 0x0000460C
	public BlockEntityData()
	{
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000B606C File Offset: 0x000B426C
	public BlockEntityData(BlockValue _blockValue, Vector3i _pos)
	{
		this.pos = _pos;
		this.blockValue = _blockValue;
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x000B6084 File Offset: 0x000B4284
	[PublicizedFrom(EAccessModifier.Private)]
	public void getRenderers()
	{
		if (this.matPropBlock == null)
		{
			this.matPropBlock = new MaterialPropertyBlock();
		}
		if (this.renderers != null)
		{
			this.renderers.Clear();
		}
		else
		{
			this.renderers = new List<Renderer>();
		}
		this.transform.GetComponentsInChildren<Renderer>(true, this.renderers);
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x000B60D6 File Offset: 0x000B42D6
	public List<Renderer> GetRenderers()
	{
		return this.renderers;
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x000B60DE File Offset: 0x000B42DE
	public void Cleanup()
	{
		if (this.renderers != null)
		{
			this.renderers.Clear();
		}
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x000B60F4 File Offset: 0x000B42F4
	public void SetMaterialColor(string name, Color value)
	{
		this.getRenderers();
		if (this.renderers == null)
		{
			return;
		}
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		for (int i = 0; i < this.renderers.Count; i++)
		{
			if (this.renderers[i] != null)
			{
				this.renderers[i].GetPropertyBlock(this.matPropBlock);
				this.matPropBlock.SetColor(name, value);
				this.renderers[i].SetPropertyBlock(this.matPropBlock);
			}
		}
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x000B6180 File Offset: 0x000B4380
	public void SetMaterialValue(string name, float value)
	{
		this.getRenderers();
		if (this.renderers == null)
		{
			return;
		}
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		for (int i = 0; i < this.renderers.Count; i++)
		{
			if (this.renderers[i] != null)
			{
				this.renderers[i].GetPropertyBlock(this.matPropBlock);
				this.matPropBlock.SetFloat(name, value);
				this.renderers[i].SetPropertyBlock(this.matPropBlock);
			}
		}
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x000B620C File Offset: 0x000B440C
	public void SetMaterialColor(Color color)
	{
		this.getRenderers();
		for (int i = 0; i < this.renderers.Count; i++)
		{
			if (this.renderers[i] != null)
			{
				this.renderers[i].GetPropertyBlock(this.matPropBlock);
				this.matPropBlock.SetColor("_Color", color);
				this.renderers[i].SetPropertyBlock(this.matPropBlock);
			}
		}
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateTemperature()
	{
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x000B6288 File Offset: 0x000B4488
	public override string ToString()
	{
		string text = "EntityBlockCreationData ";
		BlockValue blockValue = this.blockValue;
		return text + blockValue.ToString();
	}

	// Token: 0x040013FF RID: 5119
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock matPropBlock;

	// Token: 0x04001400 RID: 5120
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> renderers;

	// Token: 0x04001401 RID: 5121
	public BlockValue blockValue;

	// Token: 0x04001402 RID: 5122
	public Vector3i pos;

	// Token: 0x04001403 RID: 5123
	public Transform transform;

	// Token: 0x04001404 RID: 5124
	public bool bHasTransform;

	// Token: 0x04001405 RID: 5125
	public bool bRenderingOn;

	// Token: 0x04001406 RID: 5126
	public bool bNeedsTemperature;
}
