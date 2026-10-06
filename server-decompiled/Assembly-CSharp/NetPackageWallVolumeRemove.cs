using System;
using UnityEngine.Scripting;

// Token: 0x02000869 RID: 2153
[Preserve]
public class NetPackageWallVolumeRemove : NetPackage
{
	// Token: 0x06003E65 RID: 15973 RVA: 0x0018B8EC File Offset: 0x00189AEC
	public NetPackageWallVolumeRemove Setup(int _index)
	{
		this.index = _index;
		return this;
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06003E66 RID: 15974 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E67 RID: 15975 RVA: 0x0018B8F6 File Offset: 0x00189AF6
	public override void read(PooledBinaryReader _reader)
	{
		this.index = _reader.ReadInt32();
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x0018B904 File Offset: 0x00189B04
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.index);
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x0018B919 File Offset: 0x00189B19
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

	// Token: 0x040032AA RID: 12970
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;
}
