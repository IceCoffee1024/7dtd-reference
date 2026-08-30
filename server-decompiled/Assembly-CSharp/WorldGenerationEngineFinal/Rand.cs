using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001724 RID: 5924
	public class Rand
	{
		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x0600B84A RID: 47178 RVA: 0x0044647F File Offset: 0x0044467F
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

		// Token: 0x0600B84B RID: 47179 RVA: 0x00446497 File Offset: 0x00444697
		public Rand()
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
		}

		// Token: 0x0600B84C RID: 47180 RVA: 0x004464AF File Offset: 0x004446AF
		public Rand(int seed)
		{
			this.gameRandom = GameRandomManager.Instance.CreateGameRandom();
			this.SetSeed(seed);
		}

		// Token: 0x0600B84D RID: 47181 RVA: 0x004464CE File Offset: 0x004446CE
		public void Cleanup()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
			Rand.instance = null;
		}

		// Token: 0x0600B84E RID: 47182 RVA: 0x004464E6 File Offset: 0x004446E6
		public void Free()
		{
			GameRandomManager.Instance.FreeGameRandom(this.gameRandom);
		}

		// Token: 0x0600B84F RID: 47183 RVA: 0x004464F8 File Offset: 0x004446F8
		public void SetSeed(int seed)
		{
			this.gameRandom.SetSeed(seed);
		}

		// Token: 0x0600B850 RID: 47184 RVA: 0x00446506 File Offset: 0x00444706
		public float Float()
		{
			return this.gameRandom.RandomFloat;
		}

		// Token: 0x0600B851 RID: 47185 RVA: 0x00446513 File Offset: 0x00444713
		public int Int()
		{
			return this.gameRandom.RandomInt;
		}

		// Token: 0x0600B852 RID: 47186 RVA: 0x00446520 File Offset: 0x00444720
		public int Range(int min, int max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600B853 RID: 47187 RVA: 0x0044652F File Offset: 0x0044472F
		public int Range(int max)
		{
			return this.gameRandom.RandomRange(max);
		}

		// Token: 0x0600B854 RID: 47188 RVA: 0x0044653D File Offset: 0x0044473D
		public float Range(float min, float max)
		{
			return this.gameRandom.RandomRange(min, max);
		}

		// Token: 0x0600B855 RID: 47189 RVA: 0x0044654C File Offset: 0x0044474C
		public int Angle()
		{
			return this.gameRandom.RandomRange(360);
		}

		// Token: 0x0600B856 RID: 47190 RVA: 0x0044655E File Offset: 0x0044475E
		public Vector2 RandomOnUnitCircle()
		{
			return this.gameRandom.RandomOnUnitCircle;
		}

		// Token: 0x0600B857 RID: 47191 RVA: 0x0044656B File Offset: 0x0044476B
		public int PeekSample()
		{
			return this.gameRandom.PeekSample();
		}

		// Token: 0x040089B8 RID: 35256
		[PublicizedFrom(EAccessModifier.Private)]
		public static Rand instance;

		// Token: 0x040089B9 RID: 35257
		public GameRandom gameRandom;
	}
}
