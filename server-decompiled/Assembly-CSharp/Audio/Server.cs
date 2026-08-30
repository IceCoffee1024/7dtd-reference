using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B24 RID: 6948
	public class Server : IDisposable
	{
		// Token: 0x0600D08A RID: 53386 RVA: 0x004C09EC File Offset: 0x004BEBEC
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

		// Token: 0x0600D08B RID: 53387 RVA: 0x004C0A98 File Offset: 0x004BEC98
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

		// Token: 0x0600D08C RID: 53388 RVA: 0x004C0B2C File Offset: 0x004BED2C
		public void Stop(int playOnEntityId, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(playOnEntityId, soundGroupName);
			}
		}

		// Token: 0x0600D08D RID: 53389 RVA: 0x004C0B88 File Offset: 0x004BED88
		public void Stop(Vector3 position, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(position, soundGroupName);
			}
		}

		// Token: 0x0600D08E RID: 53390 RVA: 0x004C0BE4 File Offset: 0x004BEDE4
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer)
		{
			this.m_localPlayer = localPlayer;
		}

		// Token: 0x0600D08F RID: 53391 RVA: 0x004C0BF0 File Offset: 0x004BEDF0
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

		// Token: 0x0600D090 RID: 53392 RVA: 0x004C0C7C File Offset: 0x004BEE7C
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			Client client;
			if (this.m_players.TryGetValue(entity.entityId, out client))
			{
				this.m_players.Remove(entity.entityId);
				client.Dispose();
			}
		}

		// Token: 0x0600D091 RID: 53393 RVA: 0x004C0CB8 File Offset: 0x004BEEB8
		public void Dispose()
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_players = null;
			this.m_localPlayer = null;
		}

		// Token: 0x04009EE7 RID: 40679
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal m_localPlayer;

		// Token: 0x04009EE8 RID: 40680
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Client> m_players = new Dictionary<int, Client>();
	}
}
