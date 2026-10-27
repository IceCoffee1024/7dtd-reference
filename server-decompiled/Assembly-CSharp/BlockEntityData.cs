using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003E1 RID: 993
[Preserve]
public class BlockEntityData
{
	// Token: 0x06001DF1 RID: 7665 RVA: 0x0000640C File Offset: 0x0000460C
	public BlockEntityData()
	{
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000B6488 File Offset: 0x000B4688
	public BlockEntityData(BlockValue _blockValue, Vector3i _pos)
	{
		this.pos = _pos;
		this.blockValue = _blockValue;
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x000B64A0 File Offset: 0x000B46A0
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

	// Token: 0x06001DF4 RID: 7668 RVA: 0x000B64F2 File Offset: 0x000B46F2
	public List<Renderer> GetRenderers()
	{
		return this.renderers;
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000B64FA File Offset: 0x000B46FA
	public void Cleanup()
	{
		if (this.renderers != null)
		{
			this.renderers.Clear();
		}
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000B6510 File Offset: 0x000B4710
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

	// Token: 0x06001DF7 RID: 7671 RVA: 0x000B659C File Offset: 0x000B479C
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

	// Token: 0x06001DF8 RID: 7672 RVA: 0x000B6628 File Offset: 0x000B4828
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

	// Token: 0x06001DF9 RID: 7673 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateTemperature()
	{
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x000B66A4 File Offset: 0x000B48A4
	public override string ToString()
	{
		string text = "EntityBlockCreationData ";
		BlockValue blockValue = this.blockValue;
		return text + blockValue.ToString();
	}

	// Token: 0x04001408 RID: 5128
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock matPropBlock;

	// Token: 0x04001409 RID: 5129
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> renderers;

	// Token: 0x0400140A RID: 5130
	public BlockValue blockValue;

	// Token: 0x0400140B RID: 5131
	public Vector3i pos;

	// Token: 0x0400140C RID: 5132
	public Transform transform;

	// Token: 0x0400140D RID: 5133
	public bool bHasTransform;

	// Token: 0x0400140E RID: 5134
	public bool bRenderingOn;

	// Token: 0x0400140F RID: 5135
	public bool bNeedsTemperature;
}
