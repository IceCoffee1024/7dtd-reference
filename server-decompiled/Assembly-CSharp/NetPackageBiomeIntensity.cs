using System;
using UnityEngine.Scripting;

// Token: 0x020007BA RID: 1978
[Preserve]
public class NetPackageBiomeIntensity : NetPackage
{
	// Token: 0x06003ACE RID: 15054 RVA: 0x0017D531 File Offset: 0x0017B731
	public NetPackageBiomeIntensity Setup(BiomeIntensity _bi)
	{
		this.bi = _bi;
		return this;
	}

	// Token: 0x06003ACF RID: 15055 RVA: 0x0017D53B File Offset: 0x0017B73B
	public override void read(PooledBinaryReader _reader)
	{
		this.bi = BiomeIntensity.Default;
		this.bi.Read(_reader);
	}

	// Token: 0x06003AD0 RID: 15056 RVA: 0x0017D554 File Offset: 0x0017B754
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.bi.Write(_writer);
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0017D569 File Offset: 0x0017B769
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.LocalPlayerBiomeIntensityStandingOn = this.bi;
	}

	// Token: 0x04002FEA RID: 12266
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity bi;
}
