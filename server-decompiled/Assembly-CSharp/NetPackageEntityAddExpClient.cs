using System;
using UnityEngine.Scripting;

// Token: 0x020007D2 RID: 2002
[Preserve]
public class NetPackageEntityAddExpClient : NetPackage
{
	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06003B4A RID: 15178 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x0017E902 File Offset: 0x0017CB02
	public NetPackageEntityAddExpClient Setup(int _entityId, int _xp, Progression.XPTypes _xpType, ItemValue _usedItem)
	{
		this.entityId = _entityId;
		this.xp = _xp;
		this.xpType = (int)_xpType;
		this.includeItem = _usedItem != null;
		this.usedItem = _usedItem;
		return this;
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x0017E930 File Offset: 0x0017CB30
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

	// Token: 0x06003B4D RID: 15181 RVA: 0x0017E984 File Offset: 0x0017CB84
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

	// Token: 0x06003B4E RID: 15182 RVA: 0x0017E9E0 File Offset: 0x0017CBE0
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

	// Token: 0x06003B4F RID: 15183 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003067 RID: 12391
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04003068 RID: 12392
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xp;

	// Token: 0x04003069 RID: 12393
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xpType;

	// Token: 0x0400306A RID: 12394
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool includeItem;

	// Token: 0x0400306B RID: 12395
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue usedItem;
}
