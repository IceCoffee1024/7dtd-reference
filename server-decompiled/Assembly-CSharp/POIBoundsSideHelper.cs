using System;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class POIBoundsSideHelper : MonoBehaviour
{
	// Token: 0x06002C66 RID: 11366 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x0011A4B2 File Offset: 0x001186B2
	public void Setup()
	{
		this.MeshRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x0011A4C0 File Offset: 0x001186C0
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

	// Token: 0x06002C69 RID: 11369 RVA: 0x0011A606 File Offset: 0x00118806
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.AddSideEntered(this);
		}
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x0011A623 File Offset: 0x00118823
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.RemoveSideEntered(this);
		}
	}

	// Token: 0x04002248 RID: 8776
	public POIBoundsSideHelper.SideTypes SideType;

	// Token: 0x04002249 RID: 8777
	public POIBoundsHelper Owner;

	// Token: 0x0400224A RID: 8778
	public MeshRenderer MeshRenderer;

	// Token: 0x02000556 RID: 1366
	public enum SideTypes
	{
		// Token: 0x0400224C RID: 8780
		PositiveX,
		// Token: 0x0400224D RID: 8781
		NegativeX,
		// Token: 0x0400224E RID: 8782
		PositiveZ,
		// Token: 0x0400224F RID: 8783
		NegativeZ
	}
}
