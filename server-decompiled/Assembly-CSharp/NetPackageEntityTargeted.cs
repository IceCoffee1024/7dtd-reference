using System;
using UnityEngine.Scripting;

// Token: 0x020007FA RID: 2042
[Preserve]
public abstract class NetPackageEntityTargeted : NetPackage
{
	// Token: 0x06003C1C RID: 15388 RVA: 0x00181FBE File Offset: 0x001801BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Setup(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00181FC7 File Offset: 0x001801C7
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x00181FD5 File Offset: 0x001801D5
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x00181FEC File Offset: 0x001801EC
	public override bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return _world == null || (_world.netEntityPackageQueue == null || _world.entityAsyncManager == null) || (!_world.netEntityPackageQueue.HasPackagesForEntity(this.entityId) && !_world.entityAsyncManager.IsEntityPending(this.entityId));
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x00182039 File Offset: 0x00180239
	public override void HandleSkipped(World _world, GameManager _callbacks)
	{
		_world.netEntityPackageQueue.EnqueueNetPackageForEntity(this.entityId, this);
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x0005F9C8 File Offset: 0x0005DBC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackageEntityTargeted()
	{
	}

	// Token: 0x040030DC RID: 12508
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;
}
