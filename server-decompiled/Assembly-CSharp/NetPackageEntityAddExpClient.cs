using System;
using UnityEngine.Scripting;

// Token: 0x020007D5 RID: 2005
[Preserve]
public class NetPackageEntityAddExpClient : NetPackage
{
	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06003B66 RID: 15206 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x0017EE82 File Offset: 0x0017D082
	public NetPackageEntityAddExpClient Setup(int _entityId, int _xp, Progression.XPTypes _xpType, ItemValue _usedItem)
	{
		this.entityId = _entityId;
		this.xp = _xp;
		this.xpType = (int)_xpType;
		this.includeItem = _usedItem != null;
		this.usedItem = _usedItem;
		return this;
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0017EEB0 File Offset: 0x0017D0B0
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

	// Token: 0x06003B69 RID: 15209 RVA: 0x0017EF04 File Offset: 0x0017D104
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

	// Token: 0x06003B6A RID: 15210 RVA: 0x0017EF60 File Offset: 0x0017D160
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

	// Token: 0x04003061 RID: 12385
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04003062 RID: 12386
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xp;

	// Token: 0x04003063 RID: 12387
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xpType;

	// Token: 0x04003064 RID: 12388
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool includeItem;

	// Token: 0x04003065 RID: 12389
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue usedItem;
}
