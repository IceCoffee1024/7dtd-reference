using System;
using UnityEngine.Scripting;

// Token: 0x0200085A RID: 2138
[Preserve]
public class NetPackageWallVolumeRemove : NetPackage
{
	// Token: 0x06003E66 RID: 15974 RVA: 0x00189990 File Offset: 0x00187B90
	public NetPackageWallVolumeRemove Setup(int _index)
	{
		this.index = _index;
		return this;
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x0018999A File Offset: 0x00187B9A
	public override void read(PooledBinaryReader _reader)
	{
		this.index = _reader.ReadInt32();
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x001899A8 File Offset: 0x00187BA8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.index);
	}

	// Token: 0x06003E6A RID: 15978 RVA: 0x001899BD File Offset: 0x00187BBD
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

	// Token: 0x06003E6B RID: 15979 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400323E RID: 12862
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;
}
