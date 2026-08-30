using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007A6 RID: 1958
[Preserve]
public class NetPackageBlockLimitTracking : NetPackage
{
	// Token: 0x06003A2D RID: 14893 RVA: 0x00179CC3 File Offset: 0x00177EC3
	public NetPackageBlockLimitTracking()
	{
		this.amounts = new List<int>();
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x00179CD6 File Offset: 0x00177ED6
	public NetPackageBlockLimitTracking Setup(List<int> _amounts)
	{
		this.amounts = _amounts;
		return this;
	}

	// Token: 0x06003A2F RID: 14895 RVA: 0x00179CE0 File Offset: 0x00177EE0
	public override void read(PooledBinaryReader _br)
	{
		this.amounts.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.amounts.Add(_br.ReadInt32());
		}
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x00179D1C File Offset: 0x00177F1C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.amounts.Count);
		for (int i = 0; i < this.amounts.Count; i++)
		{
			_bw.Write(this.amounts[i]);
		}
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x00179D69 File Offset: 0x00177F69
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Warning("Server should not receive a NetPackageBlockLimitTracking");
			return;
		}
		BlockLimitTracker.instance.UpdateClientAmounts(this.amounts);
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x00179D92 File Offset: 0x00177F92
	public override int GetLength()
	{
		return 4 + this.amounts.Count * 4;
	}

	// Token: 0x04002F5C RID: 12124
	public List<int> amounts;
}
