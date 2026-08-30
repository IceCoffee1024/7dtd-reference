using System;

// Token: 0x020003CB RID: 971
public static class FireControllerUtils
{
	// Token: 0x06001D5A RID: 7514 RVA: 0x000B1634 File Offset: 0x000AF834
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
