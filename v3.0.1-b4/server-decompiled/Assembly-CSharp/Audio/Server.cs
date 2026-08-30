using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001AFD RID: 6909
	public class Server : IDisposable
	{
		// Token: 0x0600CF01 RID: 52993 RVA: 0x004BA5F8 File Offset: 0x004B87F8
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

		// Token: 0x0600CF02 RID: 52994 RVA: 0x004BA6A4 File Offset: 0x004B88A4
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

		// Token: 0x0600CF03 RID: 52995 RVA: 0x004BA738 File Offset: 0x004B8938
		public void Stop(int playOnEntityId, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(playOnEntityId, soundGroupName);
			}
		}

		// Token: 0x0600CF04 RID: 52996 RVA: 0x004BA794 File Offset: 0x004B8994
		public void Stop(Vector3 position, string soundGroupName)
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Stop(position, soundGroupName);
			}
		}

		// Token: 0x0600CF05 RID: 52997 RVA: 0x004BA7F0 File Offset: 0x004B89F0
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer)
		{
			this.m_localPlayer = localPlayer;
		}

		// Token: 0x0600CF06 RID: 52998 RVA: 0x004BA7FC File Offset: 0x004B89FC
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

		// Token: 0x0600CF07 RID: 52999 RVA: 0x004BA888 File Offset: 0x004B8A88
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			Client client;
			if (this.m_players.TryGetValue(entity.entityId, out client))
			{
				this.m_players.Remove(entity.entityId);
				client.Dispose();
			}
		}

		// Token: 0x0600CF08 RID: 53000 RVA: 0x004BA8C4 File Offset: 0x004B8AC4
		public void Dispose()
		{
			foreach (KeyValuePair<int, Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_players = null;
			this.m_localPlayer = null;
		}

		// Token: 0x04009DB6 RID: 40374
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal m_localPlayer;

		// Token: 0x04009DB7 RID: 40375
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Client> m_players = new Dictionary<int, Client>();
	}
}
