using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007C7 RID: 1991
[Preserve]
public class NetPackageDecoUpdate : NetPackage
{
	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06003B12 RID: 15122 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B13 RID: 15123 RVA: 0x0017DE3C File Offset: 0x0017C03C
	public NetPackageDecoUpdate Setup(List<DecoObject> _decoList, ref int _currentIndex)
	{
		this.firstPackage = _currentIndex == 0;
		int num = Math.Min(32768, _decoList.Count - _currentIndex);
		int num2 = _currentIndex + num;
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			pooledBinaryWriter.Write(num);
			for (int i = _currentIndex; i < num2; i++)
			{
				_decoList[i].Write(pooledBinaryWriter, null);
			}
		}
		_currentIndex = num2;
		return this;
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x0017DEC8 File Offset: 0x0017C0C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoUpdate()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x0017DF00 File Offset: 0x0017C100
	public override void read(PooledBinaryReader _br)
	{
		this.firstPackage = _br.ReadBoolean();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x0017DF34 File Offset: 0x0017C134
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.firstPackage);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B17 RID: 15127 RVA: 0x0017DF6C File Offset: 0x0017C16C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.ms;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.ms);
				this.ms.Position = 0L;
				DecoManager.Instance.Read(pooledBinaryReader, int.MaxValue, this.firstPackage);
			}
		}
	}

	// Token: 0x06003B18 RID: 15128 RVA: 0x0017DFF8 File Offset: 0x0017C1F8
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x0400303D RID: 12349
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);

	// Token: 0x0400303E RID: 12350
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstPackage = true;

	// Token: 0x0400303F RID: 12351
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decoSize = 16;

	// Token: 0x04003040 RID: 12352
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decosPerPackage = 32768;
}
