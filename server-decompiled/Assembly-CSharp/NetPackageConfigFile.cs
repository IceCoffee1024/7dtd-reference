using System;
using UnityEngine.Scripting;

// Token: 0x020007C1 RID: 1985
[Preserve]
public class NetPackageConfigFile : NetPackage
{
	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06003B03 RID: 15107 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x0017D4D2 File Offset: 0x0017B6D2
	public NetPackageConfigFile Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003B05 RID: 15109 RVA: 0x0017D4E4 File Offset: 0x0017B6E4
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = ((num >= 0) ? _reader.ReadBytes(num) : null);
	}

	// Token: 0x06003B06 RID: 15110 RVA: 0x0017D518 File Offset: 0x0017B718
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

	// Token: 0x06003B07 RID: 15111 RVA: 0x0017D557 File Offset: 0x0017B757
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		WorldStaticData.ReceivedConfigFile(this.name, this.data);
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06003B08 RID: 15112 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003002 RID: 12290
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003003 RID: 12291
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
