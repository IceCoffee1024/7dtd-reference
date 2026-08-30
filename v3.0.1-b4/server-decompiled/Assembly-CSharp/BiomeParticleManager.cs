using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200130F RID: 4879
public class BiomeParticleManager
{
	// Token: 0x06009A11 RID: 39441 RVA: 0x000027FC File Offset: 0x000009FC
	public static void RegisterEffect(string biomeName, string prefabName, float chunkMargin)
	{
	}

	// Token: 0x06009A12 RID: 39442 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public static List<GameObject> SpawnParticles(Chunk chunk, Transform _parent)
	{
		return null;
	}

	// Token: 0x040073F1 RID: 29681
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, DictionaryList<string, BiomeParticleManager.ParticleEffectData>> effects;

	// Token: 0x040073F2 RID: 29682
	public static bool RegistrationCompleted;

	// Token: 0x02001310 RID: 4880
	public struct ParticleEffectData
	{
		// Token: 0x040073F3 RID: 29683
		public string biomeName;

		// Token: 0x040073F4 RID: 29684
		public string prefabName;

		// Token: 0x040073F5 RID: 29685
		public float chunkMargin;
	}
}
