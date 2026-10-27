using System;
using UnityEngine.Scripting;

// Token: 0x02000873 RID: 2163
[Preserve]
public class NetPackageWallVolumeRemove : NetPackage
{
	// Token: 0x06003E88 RID: 16008 RVA: 0x0018C994 File Offset: 0x0018AB94
	public NetPackageWallVolumeRemove Setup(int _index)
	{
		this.index = _index;
		return this;
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0018C99E File Offset: 0x0018AB9E
	public override void read(PooledBinaryReader _reader)
	{
		this.index = _reader.ReadInt32();
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0018C9AC File Offset: 0x0018ABAC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.index);
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0018C9C1 File Offset: 0x0018ABC1
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

	// Token: 0x040032CA RID: 13002
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;
}
