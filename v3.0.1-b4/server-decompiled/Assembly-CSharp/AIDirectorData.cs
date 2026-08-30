using System;
using System.Collections.Generic;

// Token: 0x0200040E RID: 1038
public abstract class AIDirectorData
{
	// Token: 0x0600201F RID: 8223 RVA: 0x000C26C2 File Offset: 0x000C08C2
	public static void InitStatic()
	{
		AIDirectorData.noisySounds = new CaseInsensitiveStringDictionary<AIDirectorData.Noise>();
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000C26CE File Offset: 0x000C08CE
	public static void Cleanup()
	{
		if (AIDirectorData.noisySounds != null)
		{
			AIDirectorData.noisySounds.Clear();
		}
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000C26E1 File Offset: 0x000C08E1
	public static void AddNoisySound(string _name, AIDirectorData.Noise _noise)
	{
		AIDirectorData.noisySounds.Add(_name, _noise);
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000C26EF File Offset: 0x000C08EF
	public static bool FindNoise(string name, out AIDirectorData.Noise noise)
	{
		if (name == null)
		{
			noise = default(AIDirectorData.Noise);
			return false;
		}
		return AIDirectorData.noisySounds.TryGetValue(name, out noise);
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AIDirectorData()
	{
	}

	// Token: 0x040015F4 RID: 5620
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, AIDirectorData.Noise> noisySounds;

	// Token: 0x0200040F RID: 1039
	public struct Noise
	{
		// Token: 0x06002024 RID: 8228 RVA: 0x000C2709 File Offset: 0x000C0909
		public Noise(string _source, float _volume, float _duration, float _muffledWhenCrouched, float _heatMapStrength, ulong _heatMapWorldTimeToLive)
		{
			this.volume = _volume;
			this.duration = _duration;
			this.muffledWhenCrouched = _muffledWhenCrouched;
			this.heatMapStrength = _heatMapStrength;
			this.heatMapWorldTimeToLive = _heatMapWorldTimeToLive;
		}

		// Token: 0x040015F5 RID: 5621
		public float volume;

		// Token: 0x040015F6 RID: 5622
		public float duration;

		// Token: 0x040015F7 RID: 5623
		public float muffledWhenCrouched;

		// Token: 0x040015F8 RID: 5624
		public float heatMapStrength;

		// Token: 0x040015F9 RID: 5625
		public ulong heatMapWorldTimeToLive;
	}
}
