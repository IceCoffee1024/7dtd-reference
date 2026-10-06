using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B50 RID: 6992
	public class Server : IDisposable
	{
		// Token: 0x0600D1E1 RID: 53729 RVA: 0x004C4EB8 File Offset: 0x004C30B8
		public void Play(Entity playOnEntity, string soundGroupName, float _occlusion, bool signalOnly = false, float volumeScale = 1f)
		{
			if (GameManager.IsDedicatedServer && playOnEntity != null)
			{
				Manager.ConvertName(ref soundGroupName, playOnEntity);
				Manager.SignalAI(playOnEntity, playOnEntity.GetPosition(), soundGroupName, volumeScale);
			}
			if (!signalOnly)
			{
				foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
				{
					if (Manager.IgnoresDistanceCheck(soundGroupName) || Entity.CheckDistance(playOnEntity, keyValuePair.Value.entityId))
					{
						keyValuePair.Value.Play(playOnEntity.entityId, soundGroupName, _occlusion, 1f);
					}
				}
			}
		}

		// Token: 0x0600D1E2 RID: 53730 RVA: 0x004C4F64 File Offset: 0x004C3164
		public void Play(Vector3 position, string soundGroupName, float _occlusion, int entityId = -1, float volumeScale = 1f)
		{
			if (GameManager.IsDedicatedServer)
			{
				Manager.ConvertName(ref soundGroupName, null);
				Manager.SignalAI(null, position, soundGroupName, volumeScale);
			}
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				if (Manager.IgnoresDistanceCheck(soundGroupName) || Entity.CheckDistance(position, keyValuePair.Value.entityId))
				{
					keyValuePair.Value.Play(position, soundGroupName, _occlusion, entityId, volumeScale);
				}
			}
		}

		// Token: 0x0600D1E3 RID: 53731 RVA: 0x004C4FF8 File Offset: 0x004C31F8
		public void Stop(int playOnEntityId, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(playOnEntityId, soundGroupName);
			}
		}

		// Token: 0x0600D1E4 RID: 53732 RVA: 0x004C5054 File Offset: 0x004C3254
		public void Stop(Vector3 position, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(position, soundGroupName);
			}
		}

		// Token: 0x0600D1E5 RID: 53733 RVA: 0x004C50B0 File Offset: 0x004C32B0
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer)
		{
			this.m_localPlayer = localPlayer;
		}

		// Token: 0x0600D1E6 RID: 53734 RVA: 0x004C50BC File Offset: 0x004C32BC
		public void EntityAddedToWorld(Entity entity, World world)
		{
			if (entity is EntityPlayer && (this.m_localPlayer == null || entity.entityId != this.m_localPlayer.entityId))
			{
				Client client;
				if (this.m_players.TryGetValue(entity.entityId, out client))
				{
					Log.Warning("[AudioLog] AudioManagerServer: consistency error, client id '" + entity.entityId.ToString() + "' already exists, but is being added again!");
					return;
				}
				client = new Client(entity.entityId);
				this.m_players[entity.entityId] = client;
			}
		}

		// Token: 0x0600D1E7 RID: 53735 RVA: 0x004C5148 File Offset: 0x004C3348
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			Client client;
			if (this.m_players.TryGetValue(entity.entityId, out client))
			{
				this.m_players.Remove(entity.entityId);
				client.Dispose();
			}
		}

		// Token: 0x0600D1E8 RID: 53736 RVA: 0x004C5184 File Offset: 0x004C3384
		public void Dispose()
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_players = null;
			this.m_localPlayer = null;
		}

		// Token: 0x04009FAD RID: 40877
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal m_localPlayer;

		// Token: 0x04009FAE RID: 40878
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Client> m_players = new Dictionary<int, Client>();
	}
}
