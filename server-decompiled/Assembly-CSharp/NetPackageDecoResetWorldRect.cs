using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007C9 RID: 1993
[Preserve]
public class NetPackageDecoResetWorldRect : NetPackage
{
	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06003B2F RID: 15151 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0017E250 File Offset: 0x0017C450
	public NetPackageDecoResetWorldRect Setup(Rect _worldRect)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			pooledBinaryWriter.Write((int)_worldRect.x);
			pooledBinaryWriter.Write((int)_worldRect.y);
			pooledBinaryWriter.Write((int)_worldRect.width);
			pooledBinaryWriter.Write((int)_worldRect.height);
		}
		return this;
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0017E2CC File Offset: 0x0017C4CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldRect()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x0017E304 File Offset: 0x0017C504
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x0017E32C File Offset: 0x0017C52C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x0017E358 File Offset: 0x0017C558
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.ms;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.ms);
				this.ms.Position = 0L;
				int num = pooledBinaryReader.ReadInt32();
				int num2 = pooledBinaryReader.ReadInt32();
				int num3 = pooledBinaryReader.ReadInt32();
				int num4 = pooledBinaryReader.ReadInt32();
				Rect rect = new Rect((float)num, (float)num2, (float)num3, (float)num4);
				DecoManager.Instance.ResetDecosInWorldRect(rect);
			}
		}
	}

	// Token: 0x04003036 RID: 12342
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
