using System;

// Token: 0x02001393 RID: 5011
public class BoundaryProjectorTreasure : BoundaryProjector
{
	// Token: 0x170012E0 RID: 4832
	// (get) Token: 0x06009D62 RID: 40290 RVA: 0x003B746C File Offset: 0x003B566C
	// (set) Token: 0x06009D63 RID: 40291 RVA: 0x003B7474 File Offset: 0x003B5674
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

	// Token: 0x170012E1 RID: 4833
	// (get) Token: 0x06009D64 RID: 40292 RVA: 0x003B748C File Offset: 0x003B568C
	public float CurrentRadius
	{
		get
		{
			return this.ProjectorList[0].Projector.orthographicSize;
		}
	}

	// Token: 0x06009D65 RID: 40293 RVA: 0x003B74A4 File Offset: 0x003B56A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupProjectors()
	{
		base.SetAlpha(0, 1f);
		base.SetAutoRotate(0, true, 2f);
	}

	// Token: 0x06009D66 RID: 40294 RVA: 0x003B74BF File Offset: 0x003B56BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleWithinRadiusChange()
	{
		base.SetGlow(0, this.withinRadius);
	}

	// Token: 0x040076D4 RID: 30420
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool withinRadius;
}
