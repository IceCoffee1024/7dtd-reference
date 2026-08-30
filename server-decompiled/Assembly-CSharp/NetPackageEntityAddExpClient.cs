using System;
using UnityEngine.Scripting;

// Token: 0x020007C7 RID: 1991
[Preserve]
public class NetPackageEntityAddExpClient : NetPackage
{
	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06003AEB RID: 15083 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x0017C52A File Offset: 0x0017A72A
	public NetPackageEntityAddExpClient Setup(int _entityId, int _xp, Progression.XPTypes _xpType, ItemValue _usedItem)
	{
		this.entityId = _entityId;
		this.xp = _xp;
		this.xpType = (int)_xpType;
		this.includeItem = _usedItem != null;
		this.usedItem = _usedItem;
		return this;
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x0017C558 File Offset: 0x0017A758
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

	// Token: 0x06003AEE RID: 15086 RVA: 0x0017C5AC File Offset: 0x0017A7AC
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

	// Token: 0x06003AEF RID: 15087 RVA: 0x0017C608 File Offset: 0x0017A808
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

	// Token: 0x06003AF0 RID: 15088 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04002FE8 RID: 12264
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04002FE9 RID: 12265
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xp;

	// Token: 0x04002FEA RID: 12266
	[PublicizedFrom(EAccessModifier.Protected)]
	public int xpType;

	// Token: 0x04002FEB RID: 12267
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool includeItem;

	// Token: 0x04002FEC RID: 12268
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemValue usedItem;
}
