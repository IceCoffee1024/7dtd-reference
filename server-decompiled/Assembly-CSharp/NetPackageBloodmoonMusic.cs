using System;
using UnityEngine.Scripting;

// Token: 0x020007BD RID: 1981
[Preserve]
public class NetPackageBloodmoonMusic : NetPackage
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06003ADE RID: 15070 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x0017D6F3 File Offset: 0x0017B8F3
	public NetPackageBloodmoonMusic Setup(bool _isBloodmoonMusicEligible)
	{
		this.IsBloodMoonMusicEligible = _isBloodmoonMusicEligible;
		return this;
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0017D6FD File Offset: 0x0017B8FD
	public override void read(PooledBinaryReader _reader)
	{
		this.IsBloodMoonMusicEligible = _reader.ReadBoolean();
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x0017D70B File Offset: 0x0017B90B
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.IsBloodMoonMusicEligible);
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x0017D720 File Offset: 0x0017B920
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (GameManager.Instance.World != null && GameManager.Instance.World.dmsConductor != null)
		{
			GameManager.Instance.World.dmsConductor.IsBloodmoonMusicEligible = this.IsBloodMoonMusicEligible;
		}
	}

	// Token: 0x04002FEE RID: 12270
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsBloodMoonMusicEligible;
}
