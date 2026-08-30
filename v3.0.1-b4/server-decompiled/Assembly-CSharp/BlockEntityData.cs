using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003D8 RID: 984
[Preserve]
public class BlockEntityData
{
	// Token: 0x06001DD8 RID: 7640 RVA: 0x0000640C File Offset: 0x0000460C
	public BlockEntityData()
	{
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000B5928 File Offset: 0x000B3B28
	public BlockEntityData(BlockValue _blockValue, Vector3i _pos)
	{
		this.pos = _pos;
		this.blockValue = _blockValue;
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000B5940 File Offset: 0x000B3B40
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

	// Token: 0x06001DDB RID: 7643 RVA: 0x000B5992 File Offset: 0x000B3B92
	public List<Renderer> GetRenderers()
	{
		return this.renderers;
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000B599A File Offset: 0x000B3B9A
	public void Cleanup()
	{
		if (this.renderers != null)
		{
			this.renderers.Clear();
		}
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x000B59B0 File Offset: 0x000B3BB0
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

	// Token: 0x06001DDE RID: 7646 RVA: 0x000B5A3C File Offset: 0x000B3C3C
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

	// Token: 0x06001DDF RID: 7647 RVA: 0x000B5AC8 File Offset: 0x000B3CC8
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

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateTemperature()
	{
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000B5B44 File Offset: 0x000B3D44
	public override string ToString()
	{
		string text = "EntityBlockCreationData ";
		BlockValue blockValue = this.blockValue;
		return text + blockValue.ToString();
	}

	// Token: 0x040013EA RID: 5098
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock matPropBlock;

	// Token: 0x040013EB RID: 5099
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> renderers;

	// Token: 0x040013EC RID: 5100
	public BlockValue blockValue;

	// Token: 0x040013ED RID: 5101
	public Vector3i pos;

	// Token: 0x040013EE RID: 5102
	public Transform transform;

	// Token: 0x040013EF RID: 5103
	public bool bHasTransform;

	// Token: 0x040013F0 RID: 5104
	public bool bRenderingOn;

	// Token: 0x040013F1 RID: 5105
	public bool bNeedsTemperature;
}
