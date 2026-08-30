using System;
using UnityEngine.Scripting;

// Token: 0x0200084E RID: 2126
[Preserve]
public class NetPackageShowToolbeltMessage : NetPackage
{
	// Token: 0x06003E3A RID: 15930 RVA: 0x00189F29 File Offset: 0x00188129
	public NetPackageShowToolbeltMessage Setup(string _toolbeltMessage, string _sound)
	{
		this.toolbeltMessage = _toolbeltMessage;
		this.sound = _sound;
		return this;
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x00189F3A File Offset: 0x0018813A
	public override void read(PooledBinaryReader _br)
	{
		this.toolbeltMessage = _br.ReadString();
		this.sound = _br.ReadString();
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x00189F54 File Offset: 0x00188154
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.toolbeltMessage);
		_bw.Write(this.sound);
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x00189F75 File Offset: 0x00188175
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.ShowTooltip(_world.GetLocalPlayers()[0], this.toolbeltMessage, this.sound, null, null, false, false, 0f);
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06003E3E RID: 15934 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x00177BC4 File Offset: 0x00175DC4
	public override int GetLength()
	{
		return 80;
	}

	// Token: 0x0400327E RID: 12926
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolbeltMessage;

	// Token: 0x0400327F RID: 12927
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound = "";
}
