using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B51 RID: 6993
	public class Client : IDisposable
	{
		// Token: 0x0600D1E9 RID: 53737 RVA: 0x004C51EC File Offset: 0x004C33EC
		public Client(int _entityId)
		{
			this.entityId = _entityId;
		}

		// Token: 0x0600D1EA RID: 53738 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x0600D1EB RID: 53739 RVA: 0x004C51FC File Offset: 0x004C33FC
		public void Play(int playOnEntityId, string soundGroupName, float occlusion, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(playOnEntityId, soundGroupName, occlusion, true, false, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D1EC RID: 53740 RVA: 0x004C5240 File Offset: 0x004C3440
		public void Play(Vector3 position, string soundGroupName, float occlusion, int entityId = -1, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, occlusion, true, entityId, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D1ED RID: 53741 RVA: 0x004C5284 File Offset: 0x004C3484
		public void Stop(int stopOnEntityId, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(stopOnEntityId, soundGroupName, 0f, false, false, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D1EE RID: 53742 RVA: 0x004C52D0 File Offset: 0x004C34D0
		public void Stop(Vector3 position, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, 0f, false, -1, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x04009FAF RID: 40879
		public int entityId;
	}
}
