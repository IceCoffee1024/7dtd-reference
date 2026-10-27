using System;
using Unity.Mathematics;

// Token: 0x02000CE4 RID: 3300
public struct GroundWaterBounds
{
	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x0600649E RID: 25758 RVA: 0x0027967D File Offset: 0x0027787D
	public bool IsGroundWater
	{
		get
		{
			return this.state > 0;
		}
	}

	// Token: 0x0600649F RID: 25759 RVA: 0x00279688 File Offset: 0x00277888
	public GroundWaterBounds(int _groundHeight, int _waterHeight)
	{
		this.state = 1;
		this.waterHeight = (byte)math.clamp(_waterHeight, 0, 255);
		this.bottom = (byte)math.clamp(_groundHeight, 0, (int)this.waterHeight);
	}

	// Token: 0x04004E8B RID: 20107
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;

	// Token: 0x04004E8C RID: 20108
	public byte waterHeight;

	// Token: 0x04004E8D RID: 20109
	public byte bottom;
}
