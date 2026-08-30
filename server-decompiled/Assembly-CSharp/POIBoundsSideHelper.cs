using System;
using UnityEngine;

// Token: 0x02000546 RID: 1350
public class POIBoundsSideHelper : MonoBehaviour
{
	// Token: 0x06002C3E RID: 11326 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x001180E2 File Offset: 0x001162E2
	public void Setup()
	{
		this.MeshRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x001180F0 File Offset: 0x001162F0
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

	// Token: 0x06002C41 RID: 11329 RVA: 0x00118236 File Offset: 0x00116436
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.AddSideEntered(this);
		}
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x00118253 File Offset: 0x00116453
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.RemoveSideEntered(this);
		}
	}

	// Token: 0x040021F7 RID: 8695
	public POIBoundsSideHelper.SideTypes SideType;

	// Token: 0x040021F8 RID: 8696
	public POIBoundsHelper Owner;

	// Token: 0x040021F9 RID: 8697
	public MeshRenderer MeshRenderer;

	// Token: 0x02000547 RID: 1351
	public enum SideTypes
	{
		// Token: 0x040021FB RID: 8699
		PositiveX,
		// Token: 0x040021FC RID: 8700
		NegativeX,
		// Token: 0x040021FD RID: 8701
		PositiveZ,
		// Token: 0x040021FE RID: 8702
		NegativeZ
	}
}
