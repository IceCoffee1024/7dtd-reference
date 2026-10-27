using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200135B RID: 4955
public class BiomeParticleManager
{
	// Token: 0x06009C10 RID: 39952 RVA: 0x000027FC File Offset: 0x000009FC
	public static void RegisterEffect(string biomeName, string prefabName, float chunkMargin)
	{
	}

	// Token: 0x06009C11 RID: 39953 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static List<GameObject> SpawnParticles(Chunk chunk, Transform _parent)
	{
		return null;
	}

	// Token: 0x04007585 RID: 30085
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, DictionaryList<string, BiomeParticleManager.ParticleEffectData>> effects;

	// Token: 0x04007586 RID: 30086
	public static bool RegistrationCompleted;

	// Token: 0x0200135C RID: 4956
	public struct ParticleEffectData
	{
		// Token: 0x04007587 RID: 30087
		public string biomeName;

		// Token: 0x04007588 RID: 30088
		public string prefabName;

		// Token: 0x04007589 RID: 30089
		public float chunkMargin;
	}
}
