using System;
using UnityEngine.Scripting;

// Token: 0x02000850 RID: 2128
[Preserve]
public class NetPackageSignDataResponse : NetPackage
{
	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06003E48 RID: 15944 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x00189FC6 File Offset: 0x001881C6
	public NetPackageSignDataResponse Setup(byte[] _data, bool _isLastBatch)
	{
		this.data = _data;
		this.isLastBatch = _isLastBatch;
		return this;
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x00189FD8 File Offset: 0x001881D8
	public override void read(PooledBinaryReader _reader)
	{
		this.isLastBatch = _reader.ReadBoolean();
		int num = _reader.ReadInt32();
		this.data = ((num > 0) ? _reader.ReadBytes(num) : null);
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x0018A00C File Offset: 0x0018820C
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

	// Token: 0x06003E4C RID: 15948 RVA: 0x0018A057 File Offset: 0x00188257
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.ProcessSignDataBatchReceived(this.data, this.isLastBatch);
	}

	// Token: 0x06003E4D RID: 15949 RVA: 0x0018A06F File Offset: 0x0018826F
	public override int GetLength()
	{
		int num = 7;
		byte[] array = this.data;
		return num + ((array != null) ? array.Length : 0);
	}

	// Token: 0x04003280 RID: 12928
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04003281 RID: 12929
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLastBatch;
}
