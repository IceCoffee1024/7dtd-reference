using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B25 RID: 6949
	public class Client : IDisposable
	{
		// Token: 0x0600D092 RID: 53394 RVA: 0x004C0D20 File Offset: 0x004BEF20
		public Client(int _entityId)
		{
			this.entityId = _entityId;
		}

		// Token: 0x0600D093 RID: 53395 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x0600D094 RID: 53396 RVA: 0x004C0D30 File Offset: 0x004BEF30
		public void Play(int playOnEntityId, string soundGroupName, float occlusion, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(playOnEntityId, soundGroupName, occlusion, true, false, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D095 RID: 53397 RVA: 0x004C0D74 File Offset: 0x004BEF74
		public void Play(Vector3 position, string soundGroupName, float occlusion, int entityId = -1, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, occlusion, true, entityId, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D096 RID: 53398 RVA: 0x004C0DB8 File Offset: 0x004BEFB8
		public void Stop(int stopOnEntityId, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(stopOnEntityId, soundGroupName, 0f, false, false, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D097 RID: 53399 RVA: 0x004C0E04 File Offset: 0x004BF004
		public void Stop(Vector3 position, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, 0f, false, -1, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x04009EE9 RID: 40681
		public int entityId;
	}
}
