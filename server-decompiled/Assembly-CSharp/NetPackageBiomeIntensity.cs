using System;
using UnityEngine.Scripting;

// Token: 0x020007A5 RID: 1957
[Preserve]
public class NetPackageBiomeIntensity : NetPackage
{
	// Token: 0x06003A27 RID: 14887 RVA: 0x00179C79 File Offset: 0x00177E79
	public NetPackageBiomeIntensity Setup(BiomeIntensity _bi)
	{
		this.bi = _bi;
		return this;
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x00179C83 File Offset: 0x00177E83
	public override void read(PooledBinaryReader _reader)
	{
		this.bi = BiomeIntensity.Default;
		this.bi.Read(_reader);
	}

	// Token: 0x06003A29 RID: 14889 RVA: 0x00179C9C File Offset: 0x00177E9C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.bi.Write(_writer);
	}

	// Token: 0x06003A2A RID: 14890 RVA: 0x00179CB1 File Offset: 0x00177EB1
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.LocalPlayerBiomeIntensityStandingOn = this.bi;
	}

	// Token: 0x06003A2B RID: 14891 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04002F5B RID: 12123
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity bi;
}
