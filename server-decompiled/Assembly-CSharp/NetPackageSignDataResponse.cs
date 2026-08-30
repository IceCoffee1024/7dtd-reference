using System;
using UnityEngine.Scripting;

// Token: 0x02000844 RID: 2116
[Preserve]
public class NetPackageSignDataResponse : NetPackage
{
	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x00187FA6 File Offset: 0x001861A6
	public NetPackageSignDataResponse Setup(byte[] _data, bool _isLastBatch)
	{
		this.data = _data;
		this.isLastBatch = _isLastBatch;
		return this;
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00187FB8 File Offset: 0x001861B8
	public override void read(PooledBinaryReader _reader)
	{
		this.isLastBatch = _reader.ReadBoolean();
		int num = _reader.ReadInt32();
		this.data = ((num > 0) ? _reader.ReadBytes(num) : null);
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x00187FEC File Offset: 0x001861EC
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

	// Token: 0x06003DE7 RID: 15847 RVA: 0x00188037 File Offset: 0x00186237
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.ProcessSignDataBatchReceived(this.data, this.isLastBatch);
	}

	// Token: 0x06003DE8 RID: 15848 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003201 RID: 12801
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04003202 RID: 12802
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLastBatch;
}
