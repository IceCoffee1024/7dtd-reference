using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B82 RID: 7042
	public class Client : IDisposable
	{
		// Token: 0x0600D2FD RID: 54013 RVA: 0x004C8D14 File Offset: 0x004C6F14
		public Client(int _entityId)
		{
			this.entityId = _entityId;
		}

		// Token: 0x0600D2FE RID: 54014 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x0600D2FF RID: 54015 RVA: 0x004C8D24 File Offset: 0x004C6F24
		public void Play(int playOnEntityId, string soundGroupName, float occlusion, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(playOnEntityId, soundGroupName, occlusion, true, false, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D300 RID: 54016 RVA: 0x004C8D68 File Offset: 0x004C6F68
		public void Play(Vector3 position, string soundGroupName, float occlusion, int entityId = -1, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, occlusion, true, entityId, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D301 RID: 54017 RVA: 0x004C8DAC File Offset: 0x004C6FAC
		public void Stop(int stopOnEntityId, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(stopOnEntityId, soundGroupName, 0f, false, false, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600D302 RID: 54018 RVA: 0x004C8DF8 File Offset: 0x004C6FF8
		public void Stop(Vector3 position, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, 0f, false, -1, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0400A098 RID: 41112
		public int entityId;
	}
}
