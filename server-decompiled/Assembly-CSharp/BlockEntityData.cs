using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003DF RID: 991
[Preserve]
public class BlockEntityData
{
	// Token: 0x06001DED RID: 7661 RVA: 0x0000640C File Offset: 0x0000460C
	public BlockEntityData()
	{
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x000B6278 File Offset: 0x000B4478
	public BlockEntityData(BlockValue _blockValue, Vector3i _pos)
	{
		this.pos = _pos;
		this.blockValue = _blockValue;
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000B6290 File Offset: 0x000B4490
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

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000B62E2 File Offset: 0x000B44E2
	public List<Renderer> GetRenderers()
	{
		return this.renderers;
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000B62EA File Offset: 0x000B44EA
	public void Cleanup()
	{
		if (this.renderers != null)
		{
			this.renderers.Clear();
		}
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000B6300 File Offset: 0x000B4500
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

	// Token: 0x06001DF3 RID: 7667 RVA: 0x000B638C File Offset: 0x000B458C
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

	// Token: 0x06001DF4 RID: 7668 RVA: 0x000B6418 File Offset: 0x000B4618
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

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateTemperature()
	{
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000B6494 File Offset: 0x000B4694
	public override string ToString()
	{
		string text = "EntityBlockCreationData ";
		BlockValue blockValue = this.blockValue;
		return text + blockValue.ToString();
	}

	// Token: 0x040013F8 RID: 5112
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock matPropBlock;

	// Token: 0x040013F9 RID: 5113
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> renderers;

	// Token: 0x040013FA RID: 5114
	public BlockValue blockValue;

	// Token: 0x040013FB RID: 5115
	public Vector3i pos;

	// Token: 0x040013FC RID: 5116
	public Transform transform;

	// Token: 0x040013FD RID: 5117
	public bool bHasTransform;

	// Token: 0x040013FE RID: 5118
	public bool bRenderingOn;

	// Token: 0x040013FF RID: 5119
	public bool bNeedsTemperature;
}
