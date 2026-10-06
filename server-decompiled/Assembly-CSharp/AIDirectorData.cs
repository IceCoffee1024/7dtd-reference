using System;
using System.Collections.Generic;

// Token: 0x02000414 RID: 1044
public abstract class AIDirectorData
{
	// Token: 0x0600202B RID: 8235 RVA: 0x000C2F12 File Offset: 0x000C1112
	public static void InitStatic()
	{
		AIDirectorData.noisySounds = new CaseInsensitiveStringDictionary<AIDirectorData.Noise>();
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x000C2F1E File Offset: 0x000C111E
	public static void Cleanup()
	{
		if (AIDirectorData.noisySounds != null)
		{
			AIDirectorData.noisySounds.Clear();
		}
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x000C2F31 File Offset: 0x000C1131
	public static void AddNoisySound(string _name, AIDirectorData.Noise _noise)
	{
		AIDirectorData.noisySounds.Add(_name, _noise);
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x000C2F3F File Offset: 0x000C113F
	public static bool FindNoise(string name, out AIDirectorData.Noise noise)
	{
		if (name == null)
		{
			noise = default(AIDirectorData.Noise);
			return false;
		}
		return AIDirectorData.noisySounds.TryGetValue(name, out noise);
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AIDirectorData()
	{
	}

	// Token: 0x04001600 RID: 5632
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, AIDirectorData.Noise> noisySounds;

	// Token: 0x02000415 RID: 1045
	public struct Noise
	{
		// Token: 0x06002030 RID: 8240 RVA: 0x000C2F59 File Offset: 0x000C1159
		public Noise(string _source, float _volume, float _duration, float _muffledWhenCrouched, float _heatMapStrength, ulong _heatMapWorldTimeToLive)
		{
			this.volume = _volume;
			this.duration = _duration;
			this.muffledWhenCrouched = _muffledWhenCrouched;
			this.heatMapStrength = _heatMapStrength;
			this.heatMapWorldTimeToLive = _heatMapWorldTimeToLive;
		}

		// Token: 0x04001601 RID: 5633
		public float volume;

		// Token: 0x04001602 RID: 5634
		public float duration;

		// Token: 0x04001603 RID: 5635
		public float muffledWhenCrouched;

		// Token: 0x04001604 RID: 5636
		public float heatMapStrength;

		// Token: 0x04001605 RID: 5637
		public ulong heatMapWorldTimeToLive;
	}
}
