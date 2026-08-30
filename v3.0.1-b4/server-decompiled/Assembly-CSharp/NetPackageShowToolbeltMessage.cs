using System;
using UnityEngine.Scripting;

// Token: 0x02000842 RID: 2114
[Preserve]
public class NetPackageShowToolbeltMessage : NetPackage
{
	// Token: 0x06003DD5 RID: 15829 RVA: 0x00187F09 File Offset: 0x00186109
	public NetPackageShowToolbeltMessage Setup(string _toolbeltMessage, string _sound)
	{
		this.toolbeltMessage = _toolbeltMessage;
		this.sound = _sound;
		return this;
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x00187F1A File Offset: 0x0018611A
	public override void read(PooledBinaryReader _br)
	{
		this.toolbeltMessage = _br.ReadString();
		this.sound = _br.ReadString();
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00187F34 File Offset: 0x00186134
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.toolbeltMessage);
		_bw.Write(this.sound);
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00187F55 File Offset: 0x00186155
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.ShowTooltip(_world.GetLocalPlayers()[0], this.toolbeltMessage, this.sound, null, null, false, false, 0f);
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x00175944 File Offset: 0x00173B44
	public override int GetLength()
	{
		return 80;
	}

	// Token: 0x040031FF RID: 12799
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolbeltMessage;

	// Token: 0x04003200 RID: 12800
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound = "";
}
