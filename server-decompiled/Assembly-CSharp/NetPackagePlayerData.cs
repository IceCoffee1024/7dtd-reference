using System;
using UnityEngine.Scripting;

// Token: 0x0200082B RID: 2091
[Preserve]
public class NetPackagePlayerData : NetPackage
{
	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06003D14 RID: 15636 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x0018639D File Offset: 0x0018459D
	public NetPackagePlayerData Setup(EntityPlayer _player)
	{
		this.playerDataFile = new PlayerDataFile();
		if (_player != null)
		{
			this.playerDataFile.FromPlayer(_player, StreamModeWrite.Persistency);
		}
		return this;
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x001863C1 File Offset: 0x001845C1
	public override void read(PooledBinaryReader _reader)
	{
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader, StreamModeRead.FromClient);
	}

	// Token: 0x06003D17 RID: 15639 RVA: 0x001863DB File Offset: 0x001845DB
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.playerDataFile.WriteNetwork(_writer, StreamModeWrite.ToServer);
	}

	// Token: 0x06003D18 RID: 15640 RVA: 0x001863F1 File Offset: 0x001845F1
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.playerDataFile.id, false))
		{
			return;
		}
		_callbacks.SavePlayerData(base.Sender, this.playerDataFile);
	}

	// Token: 0x040031BD RID: 12733
	[PublicizedFrom(EAccessModifier.Protected)]
	public PlayerDataFile playerDataFile;
}
