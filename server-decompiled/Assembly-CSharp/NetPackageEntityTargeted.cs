using System;
using UnityEngine.Scripting;

// Token: 0x020007F0 RID: 2032
[Preserve]
public abstract class NetPackageEntityTargeted : NetPackage
{
	// Token: 0x06003BF9 RID: 15353 RVA: 0x00180EBA File Offset: 0x0017F0BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Setup(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x00180EC3 File Offset: 0x0017F0C3
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x00180ED1 File Offset: 0x0017F0D1
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x00180EE8 File Offset: 0x0017F0E8
	public override bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return _world == null || (_world.netEntityPackageQueue == null || _world.entityAsyncManager == null) || (!_world.netEntityPackageQueue.HasPackagesForEntity(this.entityId) && !_world.entityAsyncManager.IsEntityPending(this.entityId));
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x00180F35 File Offset: 0x0017F135
	public override void HandleSkipped(World _world, GameManager _callbacks)
	{
		_world.netEntityPackageQueue.EnqueueNetPackageForEntity(this.entityId, this);
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x0005F704 File Offset: 0x0005D904
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackageEntityTargeted()
	{
	}

	// Token: 0x040030BC RID: 12476
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;
}
