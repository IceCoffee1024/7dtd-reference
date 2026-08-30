using System;
using UnityEngine.Scripting;

// Token: 0x02000818 RID: 2072
[Preserve]
public class NetPackagePlayerData : NetPackage
{
	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x001832F9 File Offset: 0x001814F9
	public NetPackagePlayerData Setup(EntityPlayer _player)
	{
		this.playerDataFile = new PlayerDataFile();
		if (_player != null)
		{
			this.playerDataFile.FromPlayer(_player);
		}
		return this;
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x0018331C File Offset: 0x0018151C
	public override void read(PooledBinaryReader _reader)
	{
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader);
	}

	// Token: 0x06003CD5 RID: 15573 RVA: 0x00183335 File Offset: 0x00181535
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.playerDataFile.WriteNetwork(_writer);
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x0018334A File Offset: 0x0018154A
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.playerDataFile.id, false))
		{
			return;
		}
		_callbacks.SavePlayerData(base.Sender, this.playerDataFile);
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x001757C8 File Offset: 0x001739C8
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x0400313A RID: 12602
	[PublicizedFrom(EAccessModifier.Protected)]
	public PlayerDataFile playerDataFile;
}
