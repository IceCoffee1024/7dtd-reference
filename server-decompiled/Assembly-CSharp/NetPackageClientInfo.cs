using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007C7 RID: 1991
[Preserve]
public class NetPackageClientInfo : NetPackage
{
	// Token: 0x06003B0D RID: 15117 RVA: 0x0017E150 File Offset: 0x0017C350
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

	// Token: 0x06003B0E RID: 15118 RVA: 0x0017E1E4 File Offset: 0x0017C3E4
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

	// Token: 0x06003B0F RID: 15119 RVA: 0x0017E254 File Offset: 0x0017C454
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

	// Token: 0x06003B10 RID: 15120 RVA: 0x0017E2C8 File Offset: 0x0017C4C8
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

	// Token: 0x06003B11 RID: 15121 RVA: 0x0017E33C File Offset: 0x0017C53C
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

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06003B12 RID: 15122 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400301A RID: 12314
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> playerIds = new List<int>();

	// Token: 0x0400301B RID: 12315
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> pingTimes = new List<int>();

	// Token: 0x0400301C RID: 12316
	[PublicizedFrom(EAccessModifier.Private)]
	public List<bool> admins = new List<bool>();
}
