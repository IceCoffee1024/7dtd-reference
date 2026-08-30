using System;

// Token: 0x02001361 RID: 4961
public class BoundaryProjectorTreasure : BoundaryProjector
{
	// Token: 0x1700128F RID: 4751
	// (get) Token: 0x06009C5E RID: 40030 RVA: 0x003B04EC File Offset: 0x003AE6EC
	// (set) Token: 0x06009C5F RID: 40031 RVA: 0x003B04F4 File Offset: 0x003AE6F4
	public bool WithinRadius
	{
		get
		{
			return this.withinRadius;
		}
		set
		{
			if (this.withinRadius != value)
			{
				this.withinRadius = value;
				this.HandleWithinRadiusChange();
			}
		}
	}

	// Token: 0x17001290 RID: 4752
	// (get) Token: 0x06009C60 RID: 40032 RVA: 0x003B050C File Offset: 0x003AE70C
	public float CurrentRadius
	{
		get
		{
			return this.ProjectorList[0].Projector.orthographicSize;
		}
	}

	// Token: 0x06009C61 RID: 40033 RVA: 0x003B0524 File Offset: 0x003AE724
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupProjectors()
	{
		base.SetAlpha(0, 1f);
		base.SetAutoRotate(0, true, 2f);
	}

	// Token: 0x06009C62 RID: 40034 RVA: 0x003B053F File Offset: 0x003AE73F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleWithinRadiusChange()
	{
		base.SetGlow(0, this.withinRadius);
	}

	// Token: 0x04007618 RID: 30232
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool withinRadius;
}
