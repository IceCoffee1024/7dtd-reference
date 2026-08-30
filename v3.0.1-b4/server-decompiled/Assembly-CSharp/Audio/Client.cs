using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001AFE RID: 6910
	public class Client : IDisposable
	{
		// Token: 0x0600CF09 RID: 53001 RVA: 0x004BA92C File Offset: 0x004B8B2C
		public Client(int _entityId)
		{
			this.entityId = _entityId;
		}

		// Token: 0x0600CF0A RID: 53002 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x0600CF0B RID: 53003 RVA: 0x004BA93C File Offset: 0x004B8B3C
		public void Play(int playOnEntityId, string soundGroupName, float occlusion, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(playOnEntityId, soundGroupName, occlusion, true, false, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600CF0C RID: 53004 RVA: 0x004BA980 File Offset: 0x004B8B80
		public void Play(Vector3 position, string soundGroupName, float occlusion, int entityId = -1, float volumeScale = 1f)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, occlusion, true, entityId, volumeScale);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600CF0D RID: 53005 RVA: 0x004BA9C4 File Offset: 0x004B8BC4
		public void Stop(int stopOnEntityId, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(stopOnEntityId, soundGroupName, 0f, false, false, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0600CF0E RID: 53006 RVA: 0x004BAA10 File Offset: 0x004B8C10
		public void Stop(Vector3 position, string soundGroupName)
		{
			NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, 0f, false, -1, 1f);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageAudio, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x04009DB8 RID: 40376
		public int entityId;
	}
}
