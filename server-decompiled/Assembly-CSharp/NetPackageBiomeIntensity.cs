using System;
using UnityEngine.Scripting;

// Token: 0x020007B2 RID: 1970
[Preserve]
public class NetPackageBiomeIntensity : NetPackage
{
	// Token: 0x06003AB7 RID: 15031 RVA: 0x0017C599 File Offset: 0x0017A799
	public NetPackageBiomeIntensity Setup(BiomeIntensity _bi)
	{
		this.bi = _bi;
		return this;
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x0017C5A3 File Offset: 0x0017A7A3
	public override void read(PooledBinaryReader _reader)
	{
		this.bi = BiomeIntensity.Default;
		this.bi.Read(_reader);
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x0017C5BC File Offset: 0x0017A7BC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.bi.Write(_writer);
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x0017C5D1 File Offset: 0x0017A7D1
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.LocalPlayerBiomeIntensityStandingOn = this.bi;
	}

	// Token: 0x04002FCE RID: 12238
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeIntensity bi;
}
