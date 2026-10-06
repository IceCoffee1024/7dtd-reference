using System;
using UnityEngine.Scripting;

// Token: 0x020007B5 RID: 1973
[Preserve]
public class NetPackageBloodmoonMusic : NetPackage
{
	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x0017C75B File Offset: 0x0017A95B
	public NetPackageBloodmoonMusic Setup(bool _isBloodmoonMusicEligible)
	{
		this.IsBloodMoonMusicEligible = _isBloodmoonMusicEligible;
		return this;
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x0017C765 File Offset: 0x0017A965
	public override void read(PooledBinaryReader _reader)
	{
		this.IsBloodMoonMusicEligible = _reader.ReadBoolean();
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x0017C773 File Offset: 0x0017A973
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.IsBloodMoonMusicEligible);
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x0017C788 File Offset: 0x0017A988
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (GameManager.Instance.World != null && GameManager.Instance.World.dmsConductor != null)
		{
			GameManager.Instance.World.dmsConductor.IsBloodmoonMusicEligible = this.IsBloodMoonMusicEligible;
		}
	}

	// Token: 0x04002FD2 RID: 12242
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsBloodMoonMusicEligible;
}
