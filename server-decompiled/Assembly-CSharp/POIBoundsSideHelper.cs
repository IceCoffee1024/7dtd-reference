using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class POIBoundsSideHelper : MonoBehaviour
{
	// Token: 0x06002C68 RID: 11368 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x00119156 File Offset: 0x00117356
	public void Setup()
	{
		this.MeshRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x00119164 File Offset: 0x00117364
	public void SetSize(Vector3 size)
	{
		switch (this.SideType)
		{
		case POIBoundsSideHelper.SideTypes.PositiveX:
			base.transform.localPosition = new Vector3(size.z * 0.5f, 0f, 0f);
			base.transform.localScale = new Vector3(size.x, 100f, 6f);
			return;
		case POIBoundsSideHelper.SideTypes.NegativeX:
			base.transform.localPosition = new Vector3(size.z * -0.5f, 0f, 0f);
			base.transform.localScale = new Vector3(size.x, 100f, 6f);
			return;
		case POIBoundsSideHelper.SideTypes.PositiveZ:
			base.transform.localPosition = new Vector3(0f, 0f, size.x * 0.5f);
			base.transform.localScale = new Vector3(size.z, 100f, 6f);
			return;
		case POIBoundsSideHelper.SideTypes.NegativeZ:
			base.transform.localPosition = new Vector3(0f, 0f, size.x * -0.5f);
			base.transform.localScale = new Vector3(size.z, 100f, 6f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x001192AA File Offset: 0x001174AA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.AddSideEntered(this);
		}
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x001192C7 File Offset: 0x001174C7
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.RemoveSideEntered(this);
		}
	}

	// Token: 0x04002229 RID: 8745
	public POIBoundsSideHelper.SideTypes SideType;

	// Token: 0x0400222A RID: 8746
	public POIBoundsHelper Owner;

	// Token: 0x0400222B RID: 8747
	public MeshRenderer MeshRenderer;

	// Token: 0x0200054C RID: 1356
	public enum SideTypes
	{
		// Token: 0x0400222D RID: 8749
		PositiveX,
		// Token: 0x0400222E RID: 8750
		NegativeX,
		// Token: 0x0400222F RID: 8751
		PositiveZ,
		// Token: 0x04002230 RID: 8752
		NegativeZ
	}
}
