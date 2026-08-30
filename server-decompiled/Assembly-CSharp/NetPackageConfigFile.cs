using System;
using UnityEngine.Scripting;

// Token: 0x020007BE RID: 1982
[Preserve]
public class NetPackageConfigFile : NetPackage
{
	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AD8 RID: 15064 RVA: 0x0017CE96 File Offset: 0x0017B096
	public NetPackageConfigFile Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003AD9 RID: 15065 RVA: 0x0017CEA8 File Offset: 0x0017B0A8
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = ((num >= 0) ? _reader.ReadBytes(num) : null);
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x0017CEDC File Offset: 0x0017B0DC
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

	// Token: 0x06003ADB RID: 15067 RVA: 0x0017CF1B File Offset: 0x0017B11B
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		WorldStaticData.ReceivedConfigFile(this.name, this.data);
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x0017CF2E File Offset: 0x0017B12E
	public override int GetLength()
	{
		int num = this.name.Length * 2;
		byte[] array = this.data;
		return num + ((array != null) ? array.Length : 0);
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06003ADD RID: 15069 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003008 RID: 12296
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003009 RID: 12297
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
