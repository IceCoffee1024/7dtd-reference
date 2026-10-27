using System;
using UnityEngine.Scripting;

// Token: 0x020007C9 RID: 1993
[Preserve]
public class NetPackageConfigFile : NetPackage
{
	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06003B1A RID: 15130 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x0017E46A File Offset: 0x0017C66A
	public NetPackageConfigFile Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x0017E47C File Offset: 0x0017C67C
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = ((num >= 0) ? _reader.ReadBytes(num) : null);
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x0017E4B0 File Offset: 0x0017C6B0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.name);
		if (this.data != null)
		{
			_writer.Write(this.data.Length);
			_writer.Write(this.data);
			return;
		}
		_writer.Write(-1);
	}

	// Token: 0x06003B1E RID: 15134 RVA: 0x0017E4EF File Offset: 0x0017C6EF
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		WorldStaticData.ReceivedConfigFile(this.name, this.data);
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06003B1F RID: 15135 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400301E RID: 12318
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400301F RID: 12319
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
