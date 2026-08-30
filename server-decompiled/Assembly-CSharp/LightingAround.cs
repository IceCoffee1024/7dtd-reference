using System;

// Token: 0x02000B50 RID: 2896
public class LightingAround
{
	// Token: 0x060057DB RID: 22491 RVA: 0x00218A40 File Offset: 0x00216C40
	public LightingAround(byte _sun, byte _block, byte _stabilityMiddle)
	{
		Lighting lighting = new Lighting(_sun, _block, _stabilityMiddle);
		for (int i = 0; i < 9; i++)
		{
			this.lights[i] = lighting;
		}
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x00218A84 File Offset: 0x00216C84
	public void SetStab(byte _stab)
	{
		for (int i = 0; i < 9; i++)
		{
			this.lights[i].stability = _stab;
		}
	}

	// Token: 0x1700094A RID: 2378
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

	// Token: 0x040043D6 RID: 17366
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCount = 9;

	// Token: 0x040043D7 RID: 17367
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting[] lights = new Lighting[9];

	// Token: 0x02000B51 RID: 2897
	public enum Pos
	{
		// Token: 0x040043D9 RID: 17369
		Middle,
		// Token: 0x040043DA RID: 17370
		X0Y0Z0,
		// Token: 0x040043DB RID: 17371
		X1Y0Z0,
		// Token: 0x040043DC RID: 17372
		X1Y0Z1,
		// Token: 0x040043DD RID: 17373
		X0Y0Z1,
		// Token: 0x040043DE RID: 17374
		X0Y1Z0,
		// Token: 0x040043DF RID: 17375
		X1Y1Z0,
		// Token: 0x040043E0 RID: 17376
		X1Y1Z1,
		// Token: 0x040043E1 RID: 17377
		X0Y1Z1
	}
}
