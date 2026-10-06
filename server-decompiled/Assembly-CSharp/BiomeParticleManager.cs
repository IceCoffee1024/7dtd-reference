using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001349 RID: 4937
public class BiomeParticleManager
{
	// Token: 0x06009BB8 RID: 39864 RVA: 0x000027FC File Offset: 0x000009FC
	public static void RegisterEffect(string biomeName, string prefabName, float chunkMargin)
	{
	}

	// Token: 0x06009BB9 RID: 39865 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static List<GameObject> SpawnParticles(Chunk chunk, Transform _parent)
	{
		return null;
	}

	// Token: 0x04007544 RID: 30020
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, DictionaryList<string, BiomeParticleManager.ParticleEffectData>> effects;

	// Token: 0x04007545 RID: 30021
	public static bool RegistrationCompleted;

	// Token: 0x0200134A RID: 4938
	public struct ParticleEffectData
	{
		// Token: 0x04007546 RID: 30022
		public string biomeName;

		// Token: 0x04007547 RID: 30023
		public string prefabName;

		// Token: 0x04007548 RID: 30024
		public float chunkMargin;
	}
}
