using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F5 RID: 2037
[Preserve]
public class NetPackageExplosionInitiate : NetPackage
{
	// Token: 0x06003C18 RID: 15384 RVA: 0x001810B5 File Offset: 0x0017F2B5
	public NetPackageExplosionInitiate Setup(Vector3 _worldPos, Vector3i _blockPos, Quaternion _rotation, ExplosionData _explosionData, int _entityId, float _delay, bool _bRemoveBlockAtExplPosition, ItemValue _itemValueExplosive)
	{
		this.worldPos = _worldPos;
		this.blockPos = _blockPos;
		this.rotation = _rotation;
		this.explosionData = _explosionData;
		this.entityId = _entityId;
		this.delay = _delay;
		this.bRemoveBlockAtExplPosition = _bRemoveBlockAtExplPosition;
		this.itemValueExplosive = _itemValueExplosive;
		return this;
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x001810F8 File Offset: 0x0017F2F8
	public override void read(PooledBinaryReader _br)
	{
		this.worldPos = StreamUtils.ReadVector3(_br);
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.rotation = StreamUtils.ReadQuaterion(_br);
		int num = (int)_br.ReadUInt16();
		this.explosionData = new ExplosionData(_br.ReadBytes(num));
		this.entityId = _br.ReadInt32();
		this.delay = _br.ReadSingle();
		this.bRemoveBlockAtExplPosition = _br.ReadBoolean();
		if (_br.ReadBoolean())
		{
			this.itemValueExplosive = new ItemValue();
			this.itemValueExplosive.Read(_br);
		}
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x00181188 File Offset: 0x0017F388
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.worldPos);
		StreamUtils.Write(_bw, this.blockPos);
		StreamUtils.Write(_bw, this.rotation);
		byte[] array = this.explosionData.ToByteArray();
		_bw.Write((ushort)array.Length);
		_bw.Write(array);
		_bw.Write(this.entityId);
		_bw.Write(this.delay);
		_bw.Write(this.bRemoveBlockAtExplPosition);
		_bw.Write(this.itemValueExplosive != null);
		if (this.itemValueExplosive != null)
		{
			this.itemValueExplosive.Write(_bw);
		}
	}

	// Token: 0x06003C1B RID: 15387 RVA: 0x00181228 File Offset: 0x0017F428
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ExplosionServer(this.worldPos, this.blockPos, this.rotation, this.explosionData, this.entityId, this.delay, this.bRemoveBlockAtExplPosition, this.itemValueExplosive);
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06003C1C RID: 15388 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00177B5A File Offset: 0x00175D5A
	public override int GetLength()
	{
		return 70;
	}

	// Token: 0x040030D9 RID: 12505
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x040030DA RID: 12506
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040030DB RID: 12507
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion rotation;

	// Token: 0x040030DC RID: 12508
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosionData;

	// Token: 0x040030DD RID: 12509
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030DE RID: 12510
	[PublicizedFrom(EAccessModifier.Private)]
	public float delay;

	// Token: 0x040030DF RID: 12511
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bRemoveBlockAtExplPosition;

	// Token: 0x040030E0 RID: 12512
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValueExplosive;
}
