using System;

// Token: 0x02001347 RID: 4935
public class BoundaryProjectorTreasure : BoundaryProjector
{
	// Token: 0x17001256 RID: 4694
	// (get) Token: 0x06009B63 RID: 39779 RVA: 0x003AB244 File Offset: 0x003A9444
	// (set) Token: 0x06009B64 RID: 39780 RVA: 0x003AB24C File Offset: 0x003A944C
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

	// Token: 0x17001257 RID: 4695
	// (get) Token: 0x06009B65 RID: 39781 RVA: 0x003AB264 File Offset: 0x003A9464
	public float CurrentRadius
	{
		get
		{
			return this.ProjectorList[0].Projector.orthographicSize;
		}
	}

	// Token: 0x06009B66 RID: 39782 RVA: 0x003AB27C File Offset: 0x003A947C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupProjectors()
	{
		base.SetAlpha(0, 1f);
		base.SetAutoRotate(0, true, 2f);
	}

	// Token: 0x06009B67 RID: 39783 RVA: 0x003AB297 File Offset: 0x003A9497
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleWithinRadiusChange()
	{
		base.SetGlow(0, this.withinRadius);
	}

	// Token: 0x04007540 RID: 30016
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool withinRadius;
}
