using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001329 RID: 4905
public class BiomeParticleManager
{
	// Token: 0x06009B0C RID: 39692 RVA: 0x000027FC File Offset: 0x000009FC
	public static void RegisterEffect(string biomeName, string prefabName, float chunkMargin)
	{
	}

	// Token: 0x06009B0D RID: 39693 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public static List<GameObject> SpawnParticles(Chunk chunk, Transform _parent)
	{
		return null;
	}

	// Token: 0x040074C9 RID: 29897
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, DictionaryList<string, BiomeParticleManager.ParticleEffectData>> effects;

	// Token: 0x040074CA RID: 29898
	public static bool RegistrationCompleted;

	// Token: 0x0200132A RID: 4906
	public struct ParticleEffectData
	{
		// Token: 0x040074CB RID: 29899
		public string biomeName;

		// Token: 0x040074CC RID: 29900
		public string prefabName;

		// Token: 0x040074CD RID: 29901
		public float chunkMargin;
	}
}
