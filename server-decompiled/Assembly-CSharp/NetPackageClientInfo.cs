using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B2 RID: 1970
[Preserve]
public class NetPackageClientInfo : NetPackage
{
	// Token: 0x06003A70 RID: 14960 RVA: 0x0017A8FC File Offset: 0x00178AFC
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

	// Token: 0x06003A71 RID: 14961 RVA: 0x0017A990 File Offset: 0x00178B90
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

	// Token: 0x06003A72 RID: 14962 RVA: 0x0017AA00 File Offset: 0x00178C00
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

	// Token: 0x06003A73 RID: 14963 RVA: 0x0017AA74 File Offset: 0x00178C74
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

	// Token: 0x06003A74 RID: 14964 RVA: 0x0017AAE8 File Offset: 0x00178CE8
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

	// Token: 0x06003A75 RID: 14965 RVA: 0x0017A89E File Offset: 0x00178A9E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06003A76 RID: 14966 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04002F8B RID: 12171
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> playerIds = new List<int>();

	// Token: 0x04002F8C RID: 12172
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> pingTimes = new List<int>();

	// Token: 0x04002F8D RID: 12173
	[PublicizedFrom(EAccessModifier.Private)]
	public List<bool> admins = new List<bool>();
}
