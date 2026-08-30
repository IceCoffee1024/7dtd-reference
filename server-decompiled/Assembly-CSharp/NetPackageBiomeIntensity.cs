using System;
using UnityEngine.Scripting;

// Token: 0x020007AF RID: 1967
[Preserve]
public class NetPackageBiomeIntensity : NetPackage
{
	// Token: 0x06003A7F RID: 14975 RVA: 0x0017BEF9 File Offset: 0x0017A0F9
	public NetPackageBiomeIntensity Setup(BiomeIntensity _bi)
	{
		this.bi = _bi;
		return this;
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0017BF03 File Offset: 0x0017A103
	public override void read(PooledBinaryReader _reader)
	{
		this.bi = BiomeIntensity.Default;
		this.bi.Read(_reader);
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x0017BF1C File Offset: 0x0017A11C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.bi.Write(_writer);
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0017BF31 File Offset: 0x0017A131
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.LocalPlayerBiomeIntensityStandingOn = this.bi;
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04002FD4 RID: 12244
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity bi;
}
