using System;
using UnityEngine.Scripting;

// Token: 0x020007F3 RID: 2035
[Preserve]
public class NetPackageIdMapping : NetPackage
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x0017FDE2 File Offset: 0x0017DFE2
	public NetPackageIdMapping Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0017FDF4 File Offset: 0x0017DFF4
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = _reader.ReadBytes(num);
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0017FE21 File Offset: 0x0017E021
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.name);
		_writer.Write(this.data.Length);
		_writer.Write(this.data);
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0017FE50 File Offset: 0x0017E050
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.IdMappingReceived(this.name, this.data);
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x0017FE68 File Offset: 0x0017E068
	public override int GetLength()
	{
		return this.name.Length * 2 + this.data.Length;
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003093 RID: 12435
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003094 RID: 12436
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
