using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class DismemberedPart
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x00029663 File Offset: 0x00027863
	public void SetDetachedTransform(Transform _detach, Transform _group)
	{
		this.detachT = _detach;
		this.detachRenderer = _group.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00029678 File Offset: 0x00027878
	public void FadeDetached(float value)
	{
		if (this.detachRenderer)
		{
			if (!this.fadeOutInit)
			{
				this.detachMats.Clear();
				this.detachMats.AddRange(this.detachRenderer.materials);
				this.fadeOutInit = true;
			}
			for (int i = 0; i < this.detachMats.Count; i++)
			{
				Material material = this.detachMats[i];
				if (material.HasProperty("_Fade"))
				{
					material.SetFloat("_Fade", value);
				}
			}
			this.detachRenderer.SetMaterials(this.detachMats);
		}
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00029710 File Offset: 0x00027910
	public DismemberedPart(DismemberedPartData _data, uint _bodyDamageFlag, EnumDamageTypes _damageType)
	{
		this.Data = _data;
		this.bodyDamageFlag = _bodyDamageFlag;
		this.damageType = _damageType;
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00029764 File Offset: 0x00027964
	public string prefabPath
	{
		get
		{
			return this.Data.prefabPath;
		}
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00029774 File Offset: 0x00027974
	public void Update()
	{
		this.elapsedTime += Time.deltaTime;
		if (this.pivotT && this.overrideHeadSize != 1f)
		{
			if (!this.startValuesSet)
			{
				this.startingHeadSize = this.overrideHeadSize;
				this.startingScale = this.pivotT.localScale;
				this.startValuesSet = true;
			}
			float num = this.elapsedTime / this.overrideHeadDismemberScaleTime;
			this.overrideHeadSize = Mathf.Lerp(this.startingHeadSize, 1f, num);
			this.pivotT.localScale = Vector3.Lerp(this.startingScale, Vector3.one, num);
		}
		if (!this.ReadyForCleanup)
		{
			if (this.elapsedTime > this.lifeTime - 0.5f)
			{
				this.fadeTime += Time.deltaTime;
				this.FadeDetached(Mathf.Lerp(1f, 0f, this.fadeTime / 0.5f));
			}
			if (this.elapsedTime >= this.lifeTime)
			{
				this.ReadyForCleanup = true;
			}
		}
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0002987E File Offset: 0x00027A7E
	public void Hide()
	{
		if (this.prefabT)
		{
			this.prefabT.gameObject.SetActive(false);
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002989E File Offset: 0x00027A9E
	public void CleanupDetached()
	{
		if (this.detachT)
		{
			global::UnityEngine.Object.Destroy(this.detachT.gameObject);
			this.detachT = null;
		}
	}

	// Token: 0x0400064F RID: 1615
	public DismemberedPartData Data;

	// Token: 0x04000650 RID: 1616
	public uint bodyDamageFlag;

	// Token: 0x04000651 RID: 1617
	public EnumDamageTypes damageType;

	// Token: 0x04000652 RID: 1618
	public Transform prefabT;

	// Token: 0x04000653 RID: 1619
	public Transform detachT;

	// Token: 0x04000654 RID: 1620
	public bool ReadyForCleanup;

	// Token: 0x04000655 RID: 1621
	public float lifeTime = 10f;

	// Token: 0x04000656 RID: 1622
	[PublicizedFrom(EAccessModifier.Private)]
	public float elapsedTime;

	// Token: 0x04000657 RID: 1623
	public Transform targetT;

	// Token: 0x04000658 RID: 1624
	public Transform pivotT;

	// Token: 0x04000659 RID: 1625
	public float overrideHeadSize = 1f;

	// Token: 0x0400065A RID: 1626
	public float overrideHeadDismemberScaleTime = 1f;

	// Token: 0x0400065B RID: 1627
	[PublicizedFrom(EAccessModifier.Private)]
	public bool startValuesSet;

	// Token: 0x0400065C RID: 1628
	[PublicizedFrom(EAccessModifier.Private)]
	public float startingHeadSize;

	// Token: 0x0400065D RID: 1629
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startingScale;

	// Token: 0x0400065E RID: 1630
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshRenderer detachRenderer;

	// Token: 0x0400065F RID: 1631
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Material> detachMats = new List<Material>();

	// Token: 0x04000660 RID: 1632
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cFadeTime = 0.5f;

	// Token: 0x04000661 RID: 1633
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeTime;

	// Token: 0x04000662 RID: 1634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fadeOutInit;
}
