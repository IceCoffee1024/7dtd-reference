using System;
using UnityEngine.Scripting;

// Token: 0x0200084F RID: 2127
[Preserve]
public class NetPackageShowToolbeltMessage : NetPackage
{
	// Token: 0x06003DE0 RID: 15840 RVA: 0x00189AC9 File Offset: 0x00187CC9
	public NetPackageShowToolbeltMessage Setup(string _toolbeltMessage, string _sound)
	{
		this.toolbeltMessage = _toolbeltMessage;
		this.sound = _sound;
		return this;
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x00189ADA File Offset: 0x00187CDA
	public override void read(PooledBinaryReader _br)
	{
		this.toolbeltMessage = _br.ReadString();
		this.sound = _br.ReadString();
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x00189AF4 File Offset: 0x00187CF4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.toolbeltMessage);
		_bw.Write(this.sound);
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x00189B15 File Offset: 0x00187D15
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.ShowTooltip(_world.GetLocalPlayers()[0], this.toolbeltMessage, this.sound, null, null, false, false, 0f);
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003266 RID: 12902
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolbeltMessage;

	// Token: 0x04003267 RID: 12903
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound = "";
}
