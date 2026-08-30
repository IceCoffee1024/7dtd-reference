using System;
using UnityEngine.Scripting;

// Token: 0x02000866 RID: 2150
[Preserve]
public class NetPackageWallVolumeRemove : NetPackage
{
	// Token: 0x06003ECB RID: 16075 RVA: 0x0018BA60 File Offset: 0x00189C60
	public NetPackageWallVolumeRemove Setup(int _index)
	{
		this.index = _index;
		return this;
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06003ECC RID: 16076 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0018BA6A File Offset: 0x00189C6A
	public override void read(PooledBinaryReader _reader)
	{
		this.index = _reader.ReadInt32();
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x0018BA78 File Offset: 0x00189C78
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.index);
	}

	// Token: 0x06003ECF RID: 16079 RVA: 0x0018BA8D File Offset: 0x00189C8D
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			_world.RemoveWallVolumeAt(this.index);
		}
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x040032BF RID: 12991
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;
}
