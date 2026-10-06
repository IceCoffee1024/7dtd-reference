using System;
using Unity.Mathematics;

// Token: 0x02000CD9 RID: 3289
public struct GroundWaterBounds
{
	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x06006479 RID: 25721 RVA: 0x00278155 File Offset: 0x00276355
	public bool IsGroundWater
	{
		get
		{
			return this.state > 0;
		}
	}

	// Token: 0x0600647A RID: 25722 RVA: 0x00278160 File Offset: 0x00276360
	public GroundWaterBounds(int _groundHeight, int _waterHeight)
	{
		this.state = 1;
		this.waterHeight = (byte)math.clamp(_waterHeight, 0, 255);
		this.bottom = (byte)math.clamp(_groundHeight, 0, (int)this.waterHeight);
	}

	// Token: 0x04004E66 RID: 20070
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;

	// Token: 0x04004E67 RID: 20071
	public byte waterHeight;

	// Token: 0x04004E68 RID: 20072
	public byte bottom;
}
