using System;
using UnityEngine.Scripting;

// Token: 0x020007DF RID: 2015
[Preserve]
public class NetPackageEntityAddExpClient : NetPackage
{
	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06003B89 RID: 15241 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x0017FF86 File Offset: 0x0017E186
	public NetPackageEntityAddExpClient Setup(int _entityId, int _xp, Progression.XPTypes _xpType, ItemValue _usedItem)
	{
		this.entityId = _entityId;
		this.xp = _xp;
		this.xpType = (int)_xpType;
		this.includeItem = _usedItem != null;
		this.usedItem = _usedItem;
		return this;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x0017FFB4 File Offset: 0x0017E1B4
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.xp = _reader.ReadInt32();
		this.xpType = (int)_reader.ReadInt16();
		this.includeItem = _reader.ReadBoolean();
		if (this.includeItem)
		{
			this.usedItem = ItemValue.ReadOrNull(_reader);
		}
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x00180008 File Offset: 0x0017E208
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.xp);
		_writer.Write((short)this.xpType);
		_writer.Write(this.usedItem != null);
		if (this.usedItem != null)
		{
			this.usedItem.Write(_writer);
		}
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x00180064 File Offset: 0x0017E264
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = (EntityAlive)_world.GetEntity(this.entityId);
		if (entityAlive != null)
		{
			string text = "_xpOther";
			if (this.xpType == 0)
			{
				text = "_xpFromKill";
			}
			entityAlive.Progression.AddLevelExp(this.xp, text, (Progression.XPTypes)this.xpType, true, true, this.entityId, this.usedItem);
		}
	}

	// Token: 0x04003081 RID: 12417
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04003082 RID: 12418
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xp;

	// Token: 0x04003083 RID: 12419
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xpType;

	// Token: 0x04003084 RID: 12420
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool includeItem;

	// Token: 0x04003085 RID: 12421
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue usedItem;
}
