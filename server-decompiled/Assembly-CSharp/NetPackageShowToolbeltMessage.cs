using System;
using UnityEngine.Scripting;

// Token: 0x02000859 RID: 2137
[Preserve]
public class NetPackageShowToolbeltMessage : NetPackage
{
	// Token: 0x06003E03 RID: 15875 RVA: 0x0018AB71 File Offset: 0x00188D71
	public NetPackageShowToolbeltMessage Setup(string _toolbeltMessage, string _sound)
	{
		this.toolbeltMessage = _toolbeltMessage;
		this.sound = _sound;
		return this;
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x0018AB82 File Offset: 0x00188D82
	public override void read(PooledBinaryReader _br)
	{
		this.toolbeltMessage = _br.ReadString();
		this.sound = _br.ReadString();
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x0018AB9C File Offset: 0x00188D9C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.toolbeltMessage);
		_bw.Write(this.sound);
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x0018ABBD File Offset: 0x00188DBD
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.ShowTooltip(_world.GetLocalPlayers()[0], this.toolbeltMessage, this.sound, null, null, false, false, 0f);
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06003E07 RID: 15879 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003286 RID: 12934
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolbeltMessage;

	// Token: 0x04003287 RID: 12935
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound = "";
}
