using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016FE RID: 5886
	public class Rand
	{
		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x0600B6C7 RID: 46791 RVA: 0x00440707 File Offset: 0x0043E907
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

		// Token: 0x0600B6C8 RID: 46792 RVA: 0x0044071F File Offset: 0x0043E91F
		public Rand()
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
		}

		// Token: 0x0600B6C9 RID: 46793 RVA: 0x00440737 File Offset: 0x0043E937
		public Rand(int seed)
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
			this.SetSeed(seed);
		}

		// Token: 0x0600B6CA RID: 46794 RVA: 0x00440756 File Offset: 0x0043E956
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
			Rand.instance = null;
		}

		// Token: 0x0600B6CB RID: 46795 RVA: 0x0044076E File Offset: 0x0043E96E
		public void Free()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
		}

		// Token: 0x0600B6CC RID: 46796 RVA: 0x00440780 File Offset: 0x0043E980
		public void SetSeed(int seed)
		{
			this.gameRandom.SetSeed(seed);
		}

		// Token: 0x0600B6CD RID: 46797 RVA: 0x0044078E File Offset: 0x0043E98E
		public float Float()
		{
			return this.gameRandom.RandomFloat;
		}

		// Token: 0x0600B6CE RID: 46798 RVA: 0x0044079B File Offset: 0x0043E99B
		public int Int()
		{
			return this.gameRandom.RandomInt;
		}

		// Token: 0x0600B6CF RID: 46799 RVA: 0x004407A8 File Offset: 0x0043E9A8
		public int Range(int min, int max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600B6D0 RID: 46800 RVA: 0x004407B7 File Offset: 0x0043E9B7
		public int Range(int max)
		{
			return this.gameRandom.RandomRange(max);
		}

		// Token: 0x0600B6D1 RID: 46801 RVA: 0x004407C5 File Offset: 0x0043E9C5
		public float Range(float min, float max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600B6D2 RID: 46802 RVA: 0x004407D4 File Offset: 0x0043E9D4
		public int Angle()
		{
			return this.gameRandom.RandomRange(360);
		}

		// Token: 0x0600B6D3 RID: 46803 RVA: 0x004407E6 File Offset: 0x0043E9E6
		public Vector2 RandomOnUnitCircle()
		{
			return this.gameRandom.RandomOnUnitCircle;
		}

		// Token: 0x0600B6D4 RID: 46804 RVA: 0x004407F3 File Offset: 0x0043E9F3
		public int PeekSample()
		{
			return this.gameRandom.PeekSample();
		}

		// Token: 0x0400889C RID: 34972
		[PublicizedFrom(EAccessModifier.Private)]
		public static Rand instance;

		// Token: 0x0400889D RID: 34973
		public GameRandom gameRandom;
	}
}
