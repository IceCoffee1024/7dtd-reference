using System;

// Token: 0x02000B3E RID: 2878
public class LightingAround
{
	// Token: 0x06005753 RID: 22355 RVA: 0x00215D24 File Offset: 0x00213F24
	public LightingAround(byte _sun, byte _block, byte _stabilityMiddle)
	{
		Lighting lighting = new Lighting(_sun, _block, _stabilityMiddle);
		for (int i = 0; i < 9; i++)
		{
			this.lights[i] = lighting;
		}
	}

	// Token: 0x06005754 RID: 22356 RVA: 0x00215D68 File Offset: 0x00213F68
	public void SetStab(byte _stab)
	{
		for (int i = 0; i < 9; i++)
		{
			this.lights[i].stability = _stab;
		}
	}

	// Token: 0x17000938 RID: 2360
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

	// Token: 0x0400433B RID: 17211
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCount = 9;

	// Token: 0x0400433C RID: 17212
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting[] lights = new Lighting[9];

	// Token: 0x02000B3F RID: 2879
	public enum Pos
	{
		// Token: 0x0400433E RID: 17214
		Middle,
		// Token: 0x0400433F RID: 17215
		X0Y0Z0,
		// Token: 0x04004340 RID: 17216
		X1Y0Z0,
		// Token: 0x04004341 RID: 17217
		X1Y0Z1,
		// Token: 0x04004342 RID: 17218
		X0Y0Z1,
		// Token: 0x04004343 RID: 17219
		X0Y1Z0,
		// Token: 0x04004344 RID: 17220
		X1Y1Z0,
		// Token: 0x04004345 RID: 17221
		X1Y1Z1,
		// Token: 0x04004346 RID: 17222
		X0Y1Z1
	}
}
