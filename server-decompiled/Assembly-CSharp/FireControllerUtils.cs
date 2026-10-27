using System;

// Token: 0x020003D4 RID: 980
public static class FireControllerUtils
{
	// Token: 0x06001D73 RID: 7539 RVA: 0x000B2194 File Offset: 0x000B0394
	public static void SpawnParticleEffect(ParticleEffect _pe, int _entityId)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (!GameManager.IsDedicatedServer)
			{
				GameManager.Instance.SpawnParticleEffectClient(_pe, _entityId, false, true);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(_pe, _entityId, false, true), false, -1, _entityId, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(_pe, _entityId, false, true), false);
	}
}
