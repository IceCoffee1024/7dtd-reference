using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BB RID: 1979
[Preserve]
public class NetPackageBlockLimitTracking : NetPackage
{
	// Token: 0x06003AD3 RID: 15059 RVA: 0x0017D57B File Offset: 0x0017B77B
	public NetPackageBlockLimitTracking()
	{
		this.amounts = new List<int>();
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x0017D58E File Offset: 0x0017B78E
	public NetPackageBlockLimitTracking Setup(List<int> _amounts)
	{
		this.amounts = _amounts;
		return this;
	}

	// Token: 0x06003AD5 RID: 15061 RVA: 0x0017D598 File Offset: 0x0017B798
	public override void read(PooledBinaryReader _br)
	{
		this.amounts.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.amounts.Add(_br.ReadInt32());
		}
	}

	// Token: 0x06003AD6 RID: 15062 RVA: 0x0017D5D4 File Offset: 0x0017B7D4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.amounts.Count);
		for (int i = 0; i < this.amounts.Count; i++)
		{
			_bw.Write(this.amounts[i]);
		}
	}

	// Token: 0x06003AD7 RID: 15063 RVA: 0x0017D621 File Offset: 0x0017B821
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("Server should not receive a NetPackageBlockLimitTracking");
			return;
		}
		BlockLimitTracker.instance.UpdateClientAmounts(this.amounts);
	}

	// Token: 0x04002FEB RID: 12267
	public List<int> amounts;
}
