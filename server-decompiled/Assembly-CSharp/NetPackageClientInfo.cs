using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BF RID: 1983
[Preserve]
public class NetPackageClientInfo : NetPackage
{
	// Token: 0x06003AF6 RID: 15094 RVA: 0x0017D1B8 File Offset: 0x0017B3B8
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

	// Token: 0x06003AF7 RID: 15095 RVA: 0x0017D24C File Offset: 0x0017B44C
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

	// Token: 0x06003AF8 RID: 15096 RVA: 0x0017D2BC File Offset: 0x0017B4BC
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

	// Token: 0x06003AF9 RID: 15097 RVA: 0x0017D330 File Offset: 0x0017B530
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

	// Token: 0x06003AFA RID: 15098 RVA: 0x0017D3A4 File Offset: 0x0017B5A4
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

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06003AFB RID: 15099 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04002FFE RID: 12286
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> playerIds = new List<int>();

	// Token: 0x04002FFF RID: 12287
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> pingTimes = new List<int>();

	// Token: 0x04003000 RID: 12288
	[PublicizedFrom(EAccessModifier.Private)]
	public List<bool> admins = new List<bool>();
}
