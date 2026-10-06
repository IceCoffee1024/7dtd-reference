using System;
using UnityEngine.Scripting;

// Token: 0x02000821 RID: 2081
[Preserve]
public class NetPackagePlayerData : NetPackage
{
	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06003CF1 RID: 15601 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x00185299 File Offset: 0x00183499
	public NetPackagePlayerData Setup(EntityPlayer _player)
	{
		this.playerDataFile = new PlayerDataFile();
		if (_player != null)
		{
			this.playerDataFile.FromPlayer(_player, StreamModeWrite.Persistency);
		}
		return this;
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x001852BD File Offset: 0x001834BD
	public override void read(PooledBinaryReader _reader)
	{
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader, StreamModeRead.FromClient);
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x001852D7 File Offset: 0x001834D7
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.playerDataFile.WriteNetwork(_writer, StreamModeWrite.ToServer);
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x001852ED File Offset: 0x001834ED
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.playerDataFile.id, false))
		{
			return;
		}
		_callbacks.SavePlayerData(base.Sender, this.playerDataFile);
	}

	// Token: 0x0400319D RID: 12701
	[PublicizedFrom(EAccessModifier.Protected)]
	public PlayerDataFile playerDataFile;
}
