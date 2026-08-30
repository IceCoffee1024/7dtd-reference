using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BC RID: 1980
[Preserve]
public class NetPackageClientInfo : NetPackage
{
	// Token: 0x06003AC8 RID: 15048 RVA: 0x0017CB7C File Offset: 0x0017AD7C
	public NetPackageClientInfo Setup(WorldBase _world, IList<ClientInfo> _clients)
	{
		this.playerIds.Clear();
		this.pingTimes.Clear();
		this.admins.Clear();
		if (!GameManager.IsDedicatedServer)
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				this.addPlayerEntry(primaryPlayer, null);
			}
		}
		for (int i = 0; i < _clients.Count; i++)
		{
			int entityId = _clients[i].entityId;
			if (entityId != -1)
			{
				EntityAlive entityAlive = (EntityAlive)_world.GetEntity(entityId);
				this.addPlayerEntry(entityAlive, _clients[i]);
			}
		}
		return this;
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x0017CC10 File Offset: 0x0017AE10
	[PublicizedFrom(EAccessModifier.Private)]
	public void addPlayerEntry(EntityAlive _ea, ClientInfo _clientInfo)
	{
		if (_ea == null)
		{
			return;
		}
		EntityPlayer entityPlayer = _ea as EntityPlayer;
		_ea.pingToServer = ((_clientInfo != null) ? _clientInfo.ping : (-1));
		this.playerIds.Add(_ea.entityId);
		this.pingTimes.Add(_ea.pingToServer);
		this.admins.Add(entityPlayer != null && entityPlayer.IsAdmin);
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x0017CC80 File Offset: 0x0017AE80
	public override void read(PooledBinaryReader _reader)
	{
		this.playerIds.Clear();
		this.pingTimes.Clear();
		this.admins.Clear();
		int num = (int)_reader.ReadUInt16();
		for (int i = 0; i < num; i++)
		{
			this.playerIds.Add(_reader.ReadInt32());
			this.pingTimes.Add((int)_reader.ReadInt16());
			this.admins.Add(_reader.ReadBoolean());
		}
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x0017CCF4 File Offset: 0x0017AEF4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((ushort)this.playerIds.Count);
		for (int i = 0; i < this.playerIds.Count; i++)
		{
			_writer.Write(this.playerIds[i]);
			_writer.Write((short)this.pingTimes[i]);
			_writer.Write(this.admins[i]);
		}
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x0017CD68 File Offset: 0x0017AF68
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		for (int i = 0; i < this.playerIds.Count; i++)
		{
			EntityAlive entityAlive = (EntityAlive)_world.GetEntity(this.playerIds[i]);
			if (entityAlive != null)
			{
				entityAlive.pingToServer = this.pingTimes[i];
				EntityPlayer entityPlayer = entityAlive as EntityPlayer;
				if (entityPlayer != null)
				{
					entityPlayer.IsAdmin = this.admins[i];
				}
			}
		}
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x0017CB1E File Offset: 0x0017AD1E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x06003ACE RID: 15054 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003004 RID: 12292
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> playerIds = new List<int>();

	// Token: 0x04003005 RID: 12293
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> pingTimes = new List<int>();

	// Token: 0x04003006 RID: 12294
	[PublicizedFrom(EAccessModifier.Private)]
	public List<bool> admins = new List<bool>();
}
