using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007C6 RID: 1990
[Preserve]
public class NetPackageDecoResetWorldRect : NetPackage
{
	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06003B0A RID: 15114 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x0017DC58 File Offset: 0x0017BE58
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

	// Token: 0x06003B0C RID: 15116 RVA: 0x0017DCD4 File Offset: 0x0017BED4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldRect()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x0017DD0C File Offset: 0x0017BF0C
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x0017DD34 File Offset: 0x0017BF34
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B0F RID: 15119 RVA: 0x0017DD60 File Offset: 0x0017BF60
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

	// Token: 0x06003B10 RID: 15120 RVA: 0x0017DE14 File Offset: 0x0017C014
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x0400303C RID: 12348
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
