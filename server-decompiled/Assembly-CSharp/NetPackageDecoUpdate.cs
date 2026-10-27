using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007D2 RID: 2002
[Preserve]
public class NetPackageDecoUpdate : NetPackage
{
	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06003B4D RID: 15181 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x0017F3C0 File Offset: 0x0017D5C0
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

	// Token: 0x06003B4F RID: 15183 RVA: 0x0017F44C File Offset: 0x0017D64C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoUpdate()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0017F484 File Offset: 0x0017D684
	public override void read(PooledBinaryReader _br)
	{
		this.firstPackage = _br.ReadBoolean();
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x0017F4B8 File Offset: 0x0017D6B8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.firstPackage);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x0017F4F0 File Offset: 0x0017D6F0
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

	// Token: 0x04003053 RID: 12371
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);

	// Token: 0x04003054 RID: 12372
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstPackage = true;

	// Token: 0x04003055 RID: 12373
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decoSize = 16;

	// Token: 0x04003056 RID: 12374
	[PublicizedFrom(EAccessModifier.Private)]
	public const int decosPerPackage = 32768;
}
