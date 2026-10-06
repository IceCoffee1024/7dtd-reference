using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007CA RID: 1994
[Preserve]
public class NetPackageDecoUpdate : NetPackage
{
	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x06003B36 RID: 15158 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x0017E428 File Offset: 0x0017C628
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

	// Token: 0x06003B38 RID: 15160 RVA: 0x0017E4B4 File Offset: 0x0017C6B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoUpdate()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x0017E4EC File Offset: 0x0017C6EC
	public override void read(PooledBinaryReader _br)
	{
		this.firstPackage = _br.ReadBoolean();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x0017E520 File Offset: 0x0017C720
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.firstPackage);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x0017E558 File Offset: 0x0017C758
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

	// Token: 0x04003037 RID: 12343
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);

	// Token: 0x04003038 RID: 12344
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstPackage = true;

	// Token: 0x04003039 RID: 12345
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decoSize = 16;

	// Token: 0x0400303A RID: 12346
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decosPerPackage = 32768;
}
