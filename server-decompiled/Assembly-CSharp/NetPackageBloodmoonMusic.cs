using System;
using UnityEngine.Scripting;

// Token: 0x020007A8 RID: 1960
[Preserve]
public class NetPackageBloodmoonMusic : NetPackage
{
	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x06003A3A RID: 14906 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x00179E4F File Offset: 0x0017804F
	public NetPackageBloodmoonMusic Setup(bool _isBloodmoonMusicEligible)
	{
		this.IsBloodMoonMusicEligible = _isBloodmoonMusicEligible;
		return this;
	}

	// Token: 0x06003A3C RID: 14908 RVA: 0x00179E59 File Offset: 0x00178059
	public override void read(PooledBinaryReader _reader)
	{
		this.IsBloodMoonMusicEligible = _reader.ReadBoolean();
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x00179E67 File Offset: 0x00178067
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.IsBloodMoonMusicEligible);
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x00179E7C File Offset: 0x0017807C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (GameManager.Instance.World != null && GameManager.Instance.World.dmsConductor != null)
		{
			GameManager.Instance.World.dmsConductor.IsBloodmoonMusicEligible = this.IsBloodMoonMusicEligible;
		}
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetLength()
	{
		return 1;
	}

	// Token: 0x04002F5F RID: 12127
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsBloodMoonMusicEligible;
}
