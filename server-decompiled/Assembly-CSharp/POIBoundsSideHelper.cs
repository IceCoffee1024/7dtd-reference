using System;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class POIBoundsSideHelper : MonoBehaviour
{
	// Token: 0x06002C5E RID: 11358 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x00119F26 File Offset: 0x00118126
	public void Setup()
	{
		this.MeshRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x00119F34 File Offset: 0x00118134
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

	// Token: 0x06002C61 RID: 11361 RVA: 0x0011A07A File Offset: 0x0011827A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.AddSideEntered(this);
		}
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x0011A097 File Offset: 0x00118297
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 24)
		{
			this.Owner.RemoveSideEntered(this);
		}
	}

	// Token: 0x04002231 RID: 8753
	public POIBoundsSideHelper.SideTypes SideType;

	// Token: 0x04002232 RID: 8754
	public POIBoundsHelper Owner;

	// Token: 0x04002233 RID: 8755
	public MeshRenderer MeshRenderer;

	// Token: 0x02000552 RID: 1362
	public enum SideTypes
	{
		// Token: 0x04002235 RID: 8757
		PositiveX,
		// Token: 0x04002236 RID: 8758
		NegativeX,
		// Token: 0x04002237 RID: 8759
		PositiveZ,
		// Token: 0x04002238 RID: 8760
		NegativeZ
	}
}
