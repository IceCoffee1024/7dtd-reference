using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B3 RID: 1971
[Preserve]
public class NetPackageBlockLimitTracking : NetPackage
{
	// Token: 0x06003ABC RID: 15036 RVA: 0x0017C5E3 File Offset: 0x0017A7E3
	public NetPackageBlockLimitTracking()
	{
		this.amounts = new List<int>();
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x0017C5F6 File Offset: 0x0017A7F6
	public NetPackageBlockLimitTracking Setup(List<int> _amounts)
	{
		this.amounts = _amounts;
		return this;
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x0017C600 File Offset: 0x0017A800
	public override void read(PooledBinaryReader _br)
	{
		this.amounts.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.amounts.Add(_br.ReadInt32());
		}
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x0017C63C File Offset: 0x0017A83C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.amounts.Count);
		for (int i = 0; i < this.amounts.Count; i++)
		{
			_bw.Write(this.amounts[i]);
		}
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x0017C689 File Offset: 0x0017A889
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("Server should not receive a NetPackageBlockLimitTracking");
			return;
		}
		BlockLimitTracker.instance.UpdateClientAmounts(this.amounts);
	}

	// Token: 0x04002FCF RID: 12239
	public List<int> amounts;
}
