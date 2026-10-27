using System;
using UnityEngine.Scripting;

// Token: 0x0200085B RID: 2139
[Preserve]
public class NetPackageSignDataResponse : NetPackage
{
	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06003E0E RID: 15886 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06003E0F RID: 15887 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x0018AC0E File Offset: 0x00188E0E
	public NetPackageSignDataResponse Setup(byte[] _data, bool _isLastBatch)
	{
		this.data = _data;
		this.isLastBatch = _isLastBatch;
		return this;
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x0018AC20 File Offset: 0x00188E20
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

	// Token: 0x06003E12 RID: 15890 RVA: 0x0018AC6C File Offset: 0x00188E6C
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

	// Token: 0x06003E13 RID: 15891 RVA: 0x0018ACB7 File Offset: 0x00188EB7
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.ProcessSignDataBatchReceived(this.data, this.isLastBatch);
	}

	// Token: 0x04003288 RID: 12936
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04003289 RID: 12937
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLastBatch;
}
