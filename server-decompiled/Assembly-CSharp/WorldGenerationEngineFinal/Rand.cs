using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001750 RID: 5968
	public class Rand
	{
		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x0600B9A1 RID: 47521 RVA: 0x0044A99F File Offset: 0x00448B9F
		public static Rand Instance
		{
			get
			{
				if (Rand.instance == null)
				{
					Rand.instance = new Rand();
				}
				return Rand.instance;
			}
		}

		// Token: 0x0600B9A2 RID: 47522 RVA: 0x0044A9B7 File Offset: 0x00448BB7
		public Rand()
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
		}

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x0044A9CF File Offset: 0x00448BCF
		public Rand(int seed)
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
			this.SetSeed(seed);
		}

		// Token: 0x0600B9A4 RID: 47524 RVA: 0x0044A9EE File Offset: 0x00448BEE
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
			Rand.instance = null;
		}

		// Token: 0x0600B9A5 RID: 47525 RVA: 0x0044AA06 File Offset: 0x00448C06
		public void Free()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
		}

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x0044AA18 File Offset: 0x00448C18
		public void SetSeed(int seed)
		{
			this.gameRandom.SetSeed(seed);
		}

		// Token: 0x0600B9A7 RID: 47527 RVA: 0x0044AA26 File Offset: 0x00448C26
		public float Float()
		{
			return this.gameRandom.RandomFloat;
		}

		// Token: 0x0600B9A8 RID: 47528 RVA: 0x0044AA33 File Offset: 0x00448C33
		public int Int()
		{
			return this.gameRandom.RandomInt;
		}

		// Token: 0x0600B9A9 RID: 47529 RVA: 0x0044AA40 File Offset: 0x00448C40
		public int Range(int min, int max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600B9AA RID: 47530 RVA: 0x0044AA4F File Offset: 0x00448C4F
		public int Range(int max)
		{
			return this.gameRandom.RandomRange(max);
		}

		// Token: 0x0600B9AB RID: 47531 RVA: 0x0044AA5D File Offset: 0x00448C5D
		public float Range(float min, float max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600B9AC RID: 47532 RVA: 0x0044AA6C File Offset: 0x00448C6C
		public int Angle()
		{
			return this.gameRandom.RandomRange(360);
		}

		// Token: 0x0600B9AD RID: 47533 RVA: 0x0044AA7E File Offset: 0x00448C7E
		public Vector2 RandomOnUnitCircle()
		{
			return this.gameRandom.RandomOnUnitCircle;
		}

		// Token: 0x0600B9AE RID: 47534 RVA: 0x0044AA8B File Offset: 0x00448C8B
		public int PeekSample()
		{
			return this.gameRandom.PeekSample();
		}

		// Token: 0x04008A7D RID: 35453
		[PublicizedFrom(EAccessModifier.Private)]
		public static Rand instance;

		// Token: 0x04008A7E RID: 35454
		public GameRandom gameRandom;
	}
}
