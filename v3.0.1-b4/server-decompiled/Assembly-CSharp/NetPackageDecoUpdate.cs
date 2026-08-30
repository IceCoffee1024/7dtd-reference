using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BC RID: 1980
[Preserve]
public class NetPackageDecoUpdate : NetPackage
{
	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x0017BA64 File Offset: 0x00179C64
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

	// Token: 0x06003AB5 RID: 15029 RVA: 0x0017BAF0 File Offset: 0x00179CF0
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoUpdate()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x0017BB28 File Offset: 0x00179D28
	public override void read(PooledBinaryReader _br)
	{
		this.firstPackage = _br.ReadBoolean();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003AB7 RID: 15031 RVA: 0x0017BB5C File Offset: 0x00179D5C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.firstPackage);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x0017BB94 File Offset: 0x00179D94
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

	// Token: 0x06003AB9 RID: 15033 RVA: 0x0017BC20 File Offset: 0x00179E20
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x04002FBE RID: 12222
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);

	// Token: 0x04002FBF RID: 12223
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstPackage = true;

	// Token: 0x04002FC0 RID: 12224
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decoSize = 16;

	// Token: 0x04002FC1 RID: 12225
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decosPerPackage = 32768;
}
