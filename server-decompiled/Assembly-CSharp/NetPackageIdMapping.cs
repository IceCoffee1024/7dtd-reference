using System;
using UnityEngine.Scripting;

// Token: 0x0200080B RID: 2059
[Preserve]
public class NetPackageIdMapping : NetPackage
{
	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06003C6E RID: 15470 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x001837C6 File Offset: 0x001819C6
	public NetPackageIdMapping Setup(string _name, byte[] _data)
	{
		this.name = _name;
		this.data = _data;
		return this;
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x001837D8 File Offset: 0x001819D8
	public override void read(PooledBinaryReader _reader)
	{
		this.name = _reader.ReadString();
		int num = _reader.ReadInt32();
		this.data = _reader.ReadBytes(num);
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x00183805 File Offset: 0x00181A05
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.name);
		_writer.Write(this.data.Length);
		_writer.Write(this.data);
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00183834 File Offset: 0x00181A34
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.IdMappingReceived(this.name, this.data);
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06003C73 RID: 15475 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400312B RID: 12587
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400312C RID: 12588
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
