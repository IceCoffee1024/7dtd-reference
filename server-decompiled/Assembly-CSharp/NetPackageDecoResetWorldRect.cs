using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007BB RID: 1979
[Preserve]
public class NetPackageDecoResetWorldRect : NetPackage
{
	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06003AAB RID: 15019 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x0017B880 File Offset: 0x00179A80
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

	// Token: 0x06003AAD RID: 15021 RVA: 0x0017B8FC File Offset: 0x00179AFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldRect()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x0017B934 File Offset: 0x00179B34
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x0017B95C File Offset: 0x00179B5C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x0017B988 File Offset: 0x00179B88
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

	// Token: 0x06003AB1 RID: 15025 RVA: 0x0017BA3C File Offset: 0x00179C3C
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x04002FBD RID: 12221
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
