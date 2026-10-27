using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001781 RID: 6017
	public class Rand
	{
		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x0600BAB5 RID: 47797 RVA: 0x0044E34B File Offset: 0x0044C54B
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

		// Token: 0x0600BAB6 RID: 47798 RVA: 0x0044E363 File Offset: 0x0044C563
		public Rand()
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
		}

		// Token: 0x0600BAB7 RID: 47799 RVA: 0x0044E37B File Offset: 0x0044C57B
		public Rand(int seed)
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
			this.SetSeed(seed);
		}

		// Token: 0x0600BAB8 RID: 47800 RVA: 0x0044E39A File Offset: 0x0044C59A
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
			Rand.instance = null;
		}

		// Token: 0x0600BAB9 RID: 47801 RVA: 0x0044E3B2 File Offset: 0x0044C5B2
		public void Free()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
		}

		// Token: 0x0600BABA RID: 47802 RVA: 0x0044E3C4 File Offset: 0x0044C5C4
		public void SetSeed(int seed)
		{
			this.gameRandom.SetSeed(seed);
		}

		// Token: 0x0600BABB RID: 47803 RVA: 0x0044E3D2 File Offset: 0x0044C5D2
		public float Float()
		{
			return this.gameRandom.RandomFloat;
		}

		// Token: 0x0600BABC RID: 47804 RVA: 0x0044E3DF File Offset: 0x0044C5DF
		public int Int()
		{
			return this.gameRandom.RandomInt;
		}

		// Token: 0x0600BABD RID: 47805 RVA: 0x0044E3EC File Offset: 0x0044C5EC
		public int Range(int min, int max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600BABE RID: 47806 RVA: 0x0044E3FB File Offset: 0x0044C5FB
		public int Range(int max)
		{
			return this.gameRandom.RandomRange(max);
		}

		// Token: 0x0600BABF RID: 47807 RVA: 0x0044E409 File Offset: 0x0044C609
		public float Range(float min, float max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600BAC0 RID: 47808 RVA: 0x0044E418 File Offset: 0x0044C618
		public int Angle()
		{
			return this.gameRandom.RandomRange(360);
		}

		// Token: 0x0600BAC1 RID: 47809 RVA: 0x0044E42A File Offset: 0x0044C62A
		public Vector2 RandomOnUnitCircle()
		{
			return this.gameRandom.RandomOnUnitCircle;
		}

		// Token: 0x0600BAC2 RID: 47810 RVA: 0x0044E437 File Offset: 0x0044C637
		public int PeekSample()
		{
			return this.gameRandom.PeekSample();
		}

		// Token: 0x04008B66 RID: 35686
		[PublicizedFrom(EAccessModifier.Private)]
		public static Rand instance;

		// Token: 0x04008B67 RID: 35687
		public GameRandom gameRandom;
	}
}
