using System;
using UnityEngine.Scripting;

// Token: 0x02000801 RID: 2049
[Preserve]
public class NetPackageIdMapping : NetPackage
{
	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06003C4B RID: 15435 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x001826C2 File Offset: 0x001808C2
	public NetPackageIdMapping Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x001826D4 File Offset: 0x001808D4
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = _reader.ReadBytes(num);
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x00182701 File Offset: 0x00180901
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.name);
		_writer.Write(this.data.Length);
		_writer.Write(this.data);
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x00182730 File Offset: 0x00180930
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.IdMappingReceived(this.name, this.data);
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06003C50 RID: 15440 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400310B RID: 12555
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400310C RID: 12556
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
