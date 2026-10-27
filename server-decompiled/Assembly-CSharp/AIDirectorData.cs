using System;
using System.Collections.Generic;

// Token: 0x02000416 RID: 1046
public abstract class AIDirectorData
{
	// Token: 0x06002030 RID: 8240 RVA: 0x000C32BE File Offset: 0x000C14BE
	public static void InitStatic()
	{
		AIDirectorData.noisySounds = new CaseInsensitiveStringDictionary<AIDirectorData.Noise>();
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000C32CA File Offset: 0x000C14CA
	public static void Cleanup()
	{
		if (AIDirectorData.noisySounds != null)
		{
			AIDirectorData.noisySounds.Clear();
		}
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000C32DD File Offset: 0x000C14DD
	public static void AddNoisySound(string _name, AIDirectorData.Noise _noise)
	{
		AIDirectorData.noisySounds.Add(_name, _noise);
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000C32EB File Offset: 0x000C14EB
	public static bool FindNoise(string name, out AIDirectorData.Noise noise)
	{
		if (name == null)
		{
			noise = default(AIDirectorData.Noise);
			return false;
		}
		return AIDirectorData.noisySounds.TryGetValue(name, out noise);
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AIDirectorData()
	{
	}

	// Token: 0x04001610 RID: 5648
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, AIDirectorData.Noise> noisySounds;

	// Token: 0x02000417 RID: 1047
	public struct Noise
	{
		// Token: 0x06002035 RID: 8245 RVA: 0x000C3305 File Offset: 0x000C1505
		public Noise(string _source, float _volume, float _duration, float _muffledWhenCrouched, float _heatMapStrength, ulong _heatMapWorldTimeToLive)
		{
			this.volume = _volume;
			this.duration = _duration;
			this.muffledWhenCrouched = _muffledWhenCrouched;
			this.heatMapStrength = _heatMapStrength;
			this.heatMapWorldTimeToLive = _heatMapWorldTimeToLive;
		}

		// Token: 0x04001611 RID: 5649
		public float volume;

		// Token: 0x04001612 RID: 5650
		public float duration;

		// Token: 0x04001613 RID: 5651
		public float muffledWhenCrouched;

		// Token: 0x04001614 RID: 5652
		public float heatMapStrength;

		// Token: 0x04001615 RID: 5653
		public ulong heatMapWorldTimeToLive;
	}
}
