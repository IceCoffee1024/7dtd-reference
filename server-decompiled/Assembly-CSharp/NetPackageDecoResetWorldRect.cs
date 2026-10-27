using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D1 RID: 2001
[Preserve]
public class NetPackageDecoResetWorldRect : NetPackage
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06003B46 RID: 15174 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B47 RID: 15175 RVA: 0x0017F1E8 File Offset: 0x0017D3E8
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

	// Token: 0x06003B48 RID: 15176 RVA: 0x0017F264 File Offset: 0x0017D464
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldRect()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x0017F29C File Offset: 0x0017D49C
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x0017F2C4 File Offset: 0x0017D4C4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x0017F2F0 File Offset: 0x0017D4F0
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

	// Token: 0x04003052 RID: 12370
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
