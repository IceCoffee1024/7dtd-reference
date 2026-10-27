using System;

// Token: 0x02000B6D RID: 2925
public class LightingAround
{
	// Token: 0x0600581A RID: 22554 RVA: 0x0021D228 File Offset: 0x0021B428
	public LightingAround(byte _sun, byte _block, byte _stabilityMiddle)
	{
		Lighting lighting = new Lighting(_sun, _block, _stabilityMiddle);
		for (int i = 0; i < 9; i++)
		{
			this.lights[i] = lighting;
		}
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x0021D26C File Offset: 0x0021B46C
	public void SetStab(byte _stab)
	{
		for (int i = 0; i < 9; i++)
		{
			this.lights[i].stability = _stab;
		}
	}

	// Token: 0x17000972 RID: 2418
	public Lighting this[LightingAround.Pos _pos]
	{
		get
		{
			return this.lights[(int)_pos];
		}
		set
		{
			this.lights[(int)_pos] = value;
		}
	}

	// Token: 0x04004439 RID: 17465
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCount = 9;

	// Token: 0x0400443A RID: 17466
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting[] lights = new Lighting[9];

	// Token: 0x02000B6E RID: 2926
	public enum Pos
	{
		// Token: 0x0400443C RID: 17468
		Middle,
		// Token: 0x0400443D RID: 17469
		X0Y0Z0,
		// Token: 0x0400443E RID: 17470
		X1Y0Z0,
		// Token: 0x0400443F RID: 17471
		X1Y0Z1,
		// Token: 0x04004440 RID: 17472
		X0Y0Z1,
		// Token: 0x04004441 RID: 17473
		X0Y1Z0,
		// Token: 0x04004442 RID: 17474
		X1Y1Z0,
		// Token: 0x04004443 RID: 17475
		X1Y1Z1,
		// Token: 0x04004444 RID: 17476
		X0Y1Z1
	}
}
