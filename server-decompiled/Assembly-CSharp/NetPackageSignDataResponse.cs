using System;
using UnityEngine.Scripting;

// Token: 0x02000851 RID: 2129
[Preserve]
public class NetPackageSignDataResponse : NetPackage
{
	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06003DEB RID: 15851 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06003DEC RID: 15852 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x00189B66 File Offset: 0x00187D66
	public NetPackageSignDataResponse Setup(byte[] _data, bool _isLastBatch)
	{
		this.data = _data;
		this.isLastBatch = _isLastBatch;
		return this;
	}

	// Token: 0x06003DEE RID: 15854 RVA: 0x00189B78 File Offset: 0x00187D78
	public override void read(PooledBinaryReader _reader)
	{
		this.isLastBatch = _reader.ReadBoolean();
		int num = _reader.ReadInt32();
		if (num > 0)
		{
			this.data = new byte[num];
			_reader.ReadExact(this.data);
			return;
		}
		this.data = null;
	}

	// Token: 0x06003DEF RID: 15855 RVA: 0x00189BC4 File Offset: 0x00187DC4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.isLastBatch);
		int num = ((this.data != null) ? this.data.Length : 0);
		_writer.Write(num);
		if (num > 0)
		{
			_writer.Write(this.data);
		}
	}

	// Token: 0x06003DF0 RID: 15856 RVA: 0x00189C0F File Offset: 0x00187E0F
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.ProcessSignDataBatchReceived(this.data, this.isLastBatch);
	}

	// Token: 0x04003268 RID: 12904
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04003269 RID: 12905
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLastBatch;
}
