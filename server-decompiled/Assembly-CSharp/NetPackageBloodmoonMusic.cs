using System;
using UnityEngine.Scripting;

// Token: 0x020007B2 RID: 1970
[Preserve]
public class NetPackageBloodmoonMusic : NetPackage
{
	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06003A92 RID: 14994 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x0017C0CF File Offset: 0x0017A2CF
	public NetPackageBloodmoonMusic Setup(bool _isBloodmoonMusicEligible)
	{
		this.IsBloodMoonMusicEligible = _isBloodmoonMusicEligible;
		return this;
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x0017C0D9 File Offset: 0x0017A2D9
	public override void read(PooledBinaryReader _reader)
	{
		this.IsBloodMoonMusicEligible = _reader.ReadBoolean();
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x0017C0E7 File Offset: 0x0017A2E7
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.IsBloodMoonMusicEligible);
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x0017C0FC File Offset: 0x0017A2FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (GameManager.Instance.World != null && GameManager.Instance.World.dmsConductor != null)
		{
			GameManager.Instance.World.dmsConductor.IsBloodmoonMusicEligible = this.IsBloodMoonMusicEligible;
		}
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x00020021 File Offset: 0x0001E221
	public override int GetLength()
	{
		return 1;
	}

	// Token: 0x04002FD8 RID: 12248
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsBloodMoonMusicEligible;
}
