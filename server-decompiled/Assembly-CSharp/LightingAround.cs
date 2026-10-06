using System;

// Token: 0x02000B62 RID: 2914
public class LightingAround
{
	// Token: 0x060057F5 RID: 22517 RVA: 0x0021BCCC File Offset: 0x00219ECC
	public LightingAround(byte _sun, byte _block, byte _stabilityMiddle)
	{
		Lighting lighting = new Lighting(_sun, _block, _stabilityMiddle);
		for (int i = 0; i < 9; i++)
		{
			this.lights[i] = lighting;
		}
	}

	// Token: 0x060057F6 RID: 22518 RVA: 0x0021BD10 File Offset: 0x00219F10
	public void SetStab(byte _stab)
	{
		for (int i = 0; i < 9; i++)
		{
			this.lights[i].stability = _stab;
		}
	}

	// Token: 0x1700096C RID: 2412
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

	// Token: 0x04004414 RID: 17428
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCount = 9;

	// Token: 0x04004415 RID: 17429
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting[] lights = new Lighting[9];

	// Token: 0x02000B63 RID: 2915
	public enum Pos
	{
		// Token: 0x04004417 RID: 17431
		Middle,
		// Token: 0x04004418 RID: 17432
		X0Y0Z0,
		// Token: 0x04004419 RID: 17433
		X1Y0Z0,
		// Token: 0x0400441A RID: 17434
		X1Y0Z1,
		// Token: 0x0400441B RID: 17435
		X0Y0Z1,
		// Token: 0x0400441C RID: 17436
		X0Y1Z0,
		// Token: 0x0400441D RID: 17437
		X1Y1Z0,
		// Token: 0x0400441E RID: 17438
		X1Y1Z1,
		// Token: 0x0400441F RID: 17439
		X0Y1Z1
	}
}
