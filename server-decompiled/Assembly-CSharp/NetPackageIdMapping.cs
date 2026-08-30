using System;
using UnityEngine.Scripting;

// Token: 0x020007FE RID: 2046
[Preserve]
public class NetPackageIdMapping : NetPackage
{
	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06003C51 RID: 15441 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x001821BA File Offset: 0x001803BA
	public NetPackageIdMapping Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x001821CC File Offset: 0x001803CC
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = _reader.ReadBytes(num);
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x001821F9 File Offset: 0x001803F9
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.name);
		_writer.Write(this.data.Length);
		_writer.Write(this.data);
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x00182228 File Offset: 0x00180428
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.IdMappingReceived(this.name, this.data);
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x00182240 File Offset: 0x00180440
	public override int GetLength()
	{
		return this.name.Length * 2 + this.data.Length;
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06003C57 RID: 15447 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003112 RID: 12562
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003113 RID: 12563
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
