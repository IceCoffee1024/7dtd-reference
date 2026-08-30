using System;
using Unity.Mathematics;

// Token: 0x02000CB9 RID: 3257
public struct GroundWaterBounds
{
	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x060063FA RID: 25594 RVA: 0x00271E55 File Offset: 0x00270055
	public bool IsGroundWater
	{
		get
		{
			return this.state > 0;
		}
	}

	// Token: 0x060063FB RID: 25595 RVA: 0x00271E60 File Offset: 0x00270060
	public GroundWaterBounds(int _groundHeight, int _waterHeight)
	{
		this.state = 1;
		this.waterHeight = (byte)math.clamp(_waterHeight, 0, 255);
		this.bottom = (byte)math.clamp(_groundHeight, 0, (int)this.waterHeight);
	}

	// Token: 0x04004D92 RID: 19858
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;

	// Token: 0x04004D93 RID: 19859
	public byte waterHeight;

	// Token: 0x04004D94 RID: 19860
	public byte bottom;
}
