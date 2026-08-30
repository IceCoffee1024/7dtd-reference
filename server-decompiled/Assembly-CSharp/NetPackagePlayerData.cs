using System;
using UnityEngine.Scripting;

// Token: 0x02000823 RID: 2083
[Preserve]
public class NetPackagePlayerData : NetPackage
{
	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06003D31 RID: 15665 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x001856D1 File Offset: 0x001838D1
	public NetPackagePlayerData Setup(EntityPlayer _player)
	{
		this.playerDataFile = new PlayerDataFile();
		if (_player != null)
		{
			this.playerDataFile.FromPlayer(_player);
		}
		return this;
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x001856F4 File Offset: 0x001838F4
	public override void read(PooledBinaryReader _reader)
	{
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader);
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x0018570D File Offset: 0x0018390D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.playerDataFile.WriteNetwork(_writer);
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x00185722 File Offset: 0x00183922
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.playerDataFile.id, false))
		{
			return;
		}
		_callbacks.SavePlayerData(base.Sender, this.playerDataFile);
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x00177A48 File Offset: 0x00175C48
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x040031B9 RID: 12729
	[PublicizedFrom(EAccessModifier.Protected)]
	public PlayerDataFile playerDataFile;
}
