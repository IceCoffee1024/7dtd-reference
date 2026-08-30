using System;
using UnityEngine.Scripting;

// Token: 0x020007ED RID: 2029
[Preserve]
public abstract class NetPackageEntityTargeted : NetPackage
{
	// Token: 0x06003BF3 RID: 15347 RVA: 0x00180982 File Offset: 0x0017EB82
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Setup(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0018098B File Offset: 0x0017EB8B
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x00180999 File Offset: 0x0017EB99
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x001809B0 File Offset: 0x0017EBB0
	public override bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return _world == null || (_world.netEntityPackageQueue == null || _world.entityAsyncManager == null) || (!_world.netEntityPackageQueue.HasPackagesForEntity(this.entityId) && !_world.entityAsyncManager.IsEntityPending(this.entityId));
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x001809FD File Offset: 0x0017EBFD
	public override void HandleSkipped(World _world, GameManager _callbacks)
	{
		_world.netEntityPackageQueue.EnqueueNetPackageForEntity(this.entityId, this);
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x0005F714 File Offset: 0x0005D914
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackageEntityTargeted()
	{
	}

	// Token: 0x040030C3 RID: 12483
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;
}
