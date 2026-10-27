using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B81 RID: 7041
	public class Server : IDisposable
	{
		// Token: 0x0600D2F5 RID: 54005 RVA: 0x004C89E0 File Offset: 0x004C6BE0
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

		// Token: 0x0600D2F6 RID: 54006 RVA: 0x004C8A8C File Offset: 0x004C6C8C
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

		// Token: 0x0600D2F7 RID: 54007 RVA: 0x004C8B20 File Offset: 0x004C6D20
		public void Stop(int playOnEntityId, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(playOnEntityId, soundGroupName);
			}
		}

		// Token: 0x0600D2F8 RID: 54008 RVA: 0x004C8B7C File Offset: 0x004C6D7C
		public void Stop(Vector3 position, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(position, soundGroupName);
			}
		}

		// Token: 0x0600D2F9 RID: 54009 RVA: 0x004C8BD8 File Offset: 0x004C6DD8
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer)
		{
			this.m_localPlayer = localPlayer;
		}

		// Token: 0x0600D2FA RID: 54010 RVA: 0x004C8BE4 File Offset: 0x004C6DE4
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

		// Token: 0x0600D2FB RID: 54011 RVA: 0x004C8C70 File Offset: 0x004C6E70
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			Client client;
			if (this.m_players.TryGetValue(entity.entityId, out client))
			{
				this.m_players.Remove(entity.entityId);
				client.Dispose();
			}
		}

		// Token: 0x0600D2FC RID: 54012 RVA: 0x004C8CAC File Offset: 0x004C6EAC
		public void Dispose()
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_players = null;
			this.m_localPlayer = null;
		}

		// Token: 0x0400A096 RID: 41110
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal m_localPlayer;

		// Token: 0x0400A097 RID: 41111
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Client> m_players = new Dictionary<int, Client>();
	}
}
