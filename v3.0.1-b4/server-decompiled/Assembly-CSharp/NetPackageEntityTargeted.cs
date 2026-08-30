using System;
using UnityEngine.Scripting;

// Token: 0x020007E2 RID: 2018
[Preserve]
public abstract class NetPackageEntityTargeted : NetPackage
{
	// Token: 0x06003B94 RID: 15252 RVA: 0x0017E5AA File Offset: 0x0017C7AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Setup(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0017E5B3 File Offset: 0x0017C7B3
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0017E5C1 File Offset: 0x0017C7C1
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x0017E5D8 File Offset: 0x0017C7D8
	public override bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return _world == null || (_world.netEntityPackageQueue == null || _world.entityAsyncManager == null) || (!_world.netEntityPackageQueue.HasPackagesForEntity(this.entityId) && !_world.entityAsyncManager.IsEntityPending(this.entityId));
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0017E625 File Offset: 0x0017C825
	public override void HandleSkipped(World _world, GameManager _callbacks)
	{
		_world.netEntityPackageQueue.EnqueueNetPackageForEntity(this.entityId, this);
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0005F1CC File Offset: 0x0005D3CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackageEntityTargeted()
	{
	}

	// Token: 0x04003044 RID: 12356
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;
}
