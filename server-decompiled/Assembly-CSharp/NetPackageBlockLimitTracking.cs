using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B0 RID: 1968
[Preserve]
public class NetPackageBlockLimitTracking : NetPackage
{
	// Token: 0x06003A85 RID: 14981 RVA: 0x0017BF43 File Offset: 0x0017A143
	public NetPackageBlockLimitTracking()
	{
		this.amounts = new List<int>();
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x0017BF56 File Offset: 0x0017A156
	public NetPackageBlockLimitTracking Setup(List<int> _amounts)
	{
		this.amounts = _amounts;
		return this;
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0017BF60 File Offset: 0x0017A160
	public override void read(PooledBinaryReader _br)
	{
		this.amounts.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.amounts.Add(_br.ReadInt32());
		}
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0017BF9C File Offset: 0x0017A19C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.amounts.Count);
		for (int i = 0; i < this.amounts.Count; i++)
		{
			_bw.Write(this.amounts[i]);
		}
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x0017BFE9 File Offset: 0x0017A1E9
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("Server should not receive a NetPackageBlockLimitTracking");
			return;
		}
		BlockLimitTracker.instance.UpdateClientAmounts(this.amounts);
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x0017C012 File Offset: 0x0017A212
	public override int GetLength()
	{
		return 4 + this.amounts.Count * 4;
	}

	// Token: 0x04002FD5 RID: 12245
	public List<int> amounts;
}
