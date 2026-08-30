using System;
using UnityEngine.Scripting;

// Token: 0x020007B4 RID: 1972
[Preserve]
public class NetPackageConfigFile : NetPackage
{
	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06003A7F RID: 14975 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0017AC16 File Offset: 0x00178E16
	public NetPackageConfigFile Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x0017AC28 File Offset: 0x00178E28
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = ((num >= 0) ? _reader.ReadBytes(num) : null);
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0017AC5C File Offset: 0x00178E5C
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

	// Token: 0x06003A83 RID: 14979 RVA: 0x0017AC9B File Offset: 0x00178E9B
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		WorldStaticData.ReceivedConfigFile(this.name, this.data);
	}

	// Token: 0x06003A84 RID: 14980 RVA: 0x0017ACAE File Offset: 0x00178EAE
	public override int GetLength()
	{
		int num = this.name.Length * 2;
		byte[] array = this.data;
		return num + ((array != null) ? array.Length : 0);
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x06003A85 RID: 14981 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04002F8F RID: 12175
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002F90 RID: 12176
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
