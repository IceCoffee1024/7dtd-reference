using System;

// Token: 0x02001381 RID: 4993
public class BoundaryProjectorTreasure : BoundaryProjector
{
	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x06009D0A RID: 40202 RVA: 0x003B5068 File Offset: 0x003B3268
	// (set) Token: 0x06009D0B RID: 40203 RVA: 0x003B5070 File Offset: 0x003B3270
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

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x06009D0C RID: 40204 RVA: 0x003B5088 File Offset: 0x003B3288
	public float CurrentRadius
	{
		get
		{
			return this.ProjectorList[0].Projector.orthographicSize;
		}
	}

	// Token: 0x06009D0D RID: 40205 RVA: 0x003B50A0 File Offset: 0x003B32A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupProjectors()
	{
		base.SetAlpha(0, 1f);
		base.SetAutoRotate(0, true, 2f);
	}

	// Token: 0x06009D0E RID: 40206 RVA: 0x003B50BB File Offset: 0x003B32BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleWithinRadiusChange()
	{
		base.SetGlow(0, this.withinRadius);
	}

	// Token: 0x04007693 RID: 30355
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool withinRadius;
}
