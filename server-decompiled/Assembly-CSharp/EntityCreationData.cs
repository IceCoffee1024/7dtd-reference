using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000492 RID: 1170
[Preserve]
public class EntityCreationData
{
	// Token: 0x06002423 RID: 9251 RVA: 0x000DC76C File Offset: 0x000DA96C
	public EntityCreationData()
	{
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000DC80C File Offset: 0x000DAA0C
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityCreationData(EntityCreationData _other)
	{
		this.entityClass = _other.entityClass;
		this.pos = _other.pos;
		this.rot = _other.rot;
		this.id = _other.id;
		this.onGround = _other.onGround;
		this.stats = ((_other.stats != null) ? _other.stats.SimpleClone() : null);
		this.deathTime = _other.deathTime;
		this.lifetime = _other.lifetime;
		this.itemStack = _other.itemStack;
		Bag bag = _other.bag;
		this.bag = ((bag != null) ? bag.Clone() : null);
		this.belongsPlayerId = _other.belongsPlayerId;
		this.clientEntityId = _other.clientEntityId;
		this.holdingItem = _other.holdingItem;
		this.teamNumber = _other.teamNumber;
		this.entityName = _other.entityName;
		this.skinTexture = _other.skinTexture;
		this.subType = _other.subType;
		this.traderData = ((_other.traderData != null) ? _other.traderData.Clone() : null);
		this.homePosition = _other.homePosition;
		this.homeRange = _other.homeRange;
		this.entityData = _other.entityData;
		this.readFileVersion = _other.readFileVersion;
		this.playerProfile = _other.playerProfile;
		this.bodyDamage = _other.bodyDamage;
		this.sleeperPose = _other.sleeperPose;
		this.isSleeper = _other.isSleeper;
		this.isSleeperPassive = _other.isSleeperPassive;
		this.spawnByName = _other.spawnByName;
		this.spawnById = _other.spawnById;
		this.spawnByAllowShare = _other.spawnByAllowShare;
		this.headState = _other.headState;
		this.overrideSize = _other.overrideSize;
		this.overrideHeadSize = _other.overrideHeadSize;
		this.isDancing = _other.isDancing;
		this.orderState = _other.orderState;
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000DCA7C File Offset: 0x000DAC7C
	public EntityCreationData(XmlElement _entityElement)
	{
		this.readXml(_entityElement);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000DCB24 File Offset: 0x000DAD24
	public void ApplyToEntity(Entity _e)
	{
		EntityAlive entityAlive = _e as EntityAlive;
		if (entityAlive)
		{
			if (this.stats != null)
			{
				entityAlive.SetStats(this.stats);
			}
			if (entityAlive.Health <= 0)
			{
				entityAlive.HasDeathAnim = false;
			}
			entityAlive.SetDeathTime(this.deathTime);
			entityAlive.setHomeArea(this.homePosition, this.homeRange);
			EntityPlayer entityPlayer = _e as EntityPlayer;
			if (entityPlayer)
			{
				entityPlayer.playerProfile = this.playerProfile;
			}
			entityAlive.bodyDamage = this.bodyDamage;
			entityAlive.IsSleeper = this.isSleeper;
			if (entityAlive.IsSleeper)
			{
				entityAlive.IsSleeperPassive = this.isSleeperPassive;
			}
			entityAlive.CurrentHeadState = this.headState;
			entityAlive.IsDancing = this.isDancing;
		}
		_e.spawnByAllowShare = this.spawnByAllowShare;
		_e.spawnById = this.spawnById;
		_e.spawnByName = this.spawnByName;
		EntityTrader entityTrader = _e as EntityTrader;
		if (entityTrader)
		{
			if (this.traderData == null)
			{
				this.traderData = new TraderData();
			}
			entityTrader.TraderData = this.traderData.Clone();
		}
		if (this.sleeperPose != 255 && entityAlive)
		{
			entityAlive.TriggerSleeperPose((int)this.sleeperPose, false);
		}
		EntityDrone entityDrone = _e as EntityDrone;
		if (entityDrone != null)
		{
			entityDrone.OnApplyToEntity(this.orderState);
		}
		_e.SetSpawnerSource(this.spawnerSource);
		if (this.bag != null)
		{
			_e.bag = this.bag.Clone();
		}
		if (this.entityData.Length > 0L)
		{
			this.entityData.Position = 0L;
			try
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader.SetBaseStream(this.entityData);
					_e.Read(this.readFileVersion, pooledBinaryReader);
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
				Log.Error("Error loading entity " + ((_e != null) ? _e.ToString() : null));
			}
		}
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000DCD2C File Offset: 0x000DAF2C
	public EntityCreationData(Entity _e, bool _bNetworkWrite = true)
	{
		this.entityClass = _e.entityClass;
		this.id = _e.entityId;
		this.pos = _e.position;
		this.rot = _e.rotation;
		this.onGround = _e.onGround;
		this.belongsPlayerId = _e.belongsPlayerId;
		this.clientEntityId = _e.clientEntityId;
		this.lifetime = _e.lifetime;
		this.spawnerSource = _e.GetSpawnerSource();
		this.spawnById = _e.spawnById;
		this.spawnByAllowShare = _e.spawnByAllowShare;
		this.spawnByName = _e.spawnByName;
		if (_e is EntityAlive)
		{
			EntityAlive entityAlive = _e as EntityAlive;
			if (entityAlive.inventory != null)
			{
				this.holdingItem = entityAlive.inventory.holdingItemItemValue;
			}
			this.stats = entityAlive.Stats;
			this.deathTime = entityAlive.GetDeathTime();
			this.teamNumber = entityAlive.TeamNumber;
			this.entityName = entityAlive.EntityName;
			this.skinTexture = string.Empty;
			this.homePosition = entityAlive.getHomePosition().position;
			this.homeRange = entityAlive.getMaximumHomeDistance();
			this.bodyDamage = entityAlive.bodyDamage;
			this.sleeperPose = (byte)(entityAlive.IsSleeping ? entityAlive.lastSleeperPose : 255);
			this.isSleeper = entityAlive.IsSleeper;
			this.isSleeperPassive = entityAlive.IsSleeperPassive;
			if (_e is EntityPlayer)
			{
				EntityPlayer entityPlayer = _e as EntityPlayer;
				this.playerProfile = entityPlayer.playerProfile;
			}
			else if (_e is EntityTrader)
			{
				TraderData traderData = ((EntityTrader)_e).TraderData;
				this.traderData = ((traderData != null) ? traderData.Clone() : null);
			}
			else
			{
				EntityDrone entityDrone = _e as EntityDrone;
				if (entityDrone != null)
				{
					this.orderState = (int)entityDrone.OrderState;
				}
			}
			this.headState = entityAlive.GetHeadState();
			this.overrideSize = entityAlive.OverrideSize;
			this.overrideHeadSize = entityAlive.OverrideHeadSize;
			this.isDancing = entityAlive.IsDancing;
		}
		else if (_e is EntityItem)
		{
			EntityItem entityItem = (EntityItem)_e;
			this.itemStack = entityItem.itemStack;
		}
		else if (_e is EntityFallingBlock)
		{
			EntityFallingBlock entityFallingBlock = _e as EntityFallingBlock;
			this.blockValues = new BlockValue[] { entityFallingBlock.GetBlockValue() };
			this.textureFullArrays = new TextureFullArray[] { entityFallingBlock.GetTextureFull() };
		}
		else if (_e is EntityFallingBlocks)
		{
			EntityFallingBlocks entityFallingBlocks = _e as EntityFallingBlocks;
			this.blockValues = entityFallingBlocks.GetBlockValues();
			this.blockPositions = entityFallingBlocks.GetBlockPositions();
			this.textureFullArrays = entityFallingBlocks.GetTextureFullArrays();
		}
		else if (_e is EntityFallingTree)
		{
			EntityFallingTree entityFallingTree = _e as EntityFallingTree;
			this.blockPos = entityFallingTree.GetBlockPos();
			this.fallTreeDir = entityFallingTree.GetFallTreeDir();
		}
		Bag bag = _e.bag;
		this.bag = ((bag != null) ? bag.Clone() : null);
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.entityData);
			_e.Write(pooledBinaryWriter, _bNetworkWrite);
		}
		this.readFileVersion = 35;
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000DD0D8 File Offset: 0x000DB2D8
	public EntityCreationData Clone()
	{
		return new EntityCreationData(this);
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x000DD0E0 File Offset: 0x000DB2E0
	public void read(PooledBinaryReader _br, bool _bNetworkRead)
	{
		this.readFileVersion = _br.ReadByte();
		byte b = this.readFileVersion;
		this.bag = null;
		this.entityClass = _br.ReadInt32();
		bool flag = this.entityClass == EntityClass.playerMaleClass || this.entityClass == EntityClass.playerFemaleClass;
		this.id = _br.ReadInt32();
		this.lifetime = _br.ReadSingle();
		this.pos.x = _br.ReadSingle();
		this.pos.y = _br.ReadSingle();
		this.pos.z = _br.ReadSingle();
		this.rot.x = _br.ReadSingle();
		this.rot.y = _br.ReadSingle();
		this.rot.z = _br.ReadSingle();
		this.onGround = _br.ReadBoolean();
		this.bodyDamage = BodyDamage.Read(_br, (int)b);
		if (b >= 8)
		{
			if (_br.ReadBoolean())
			{
				this.stats = (flag ? new PlayerEntityStats() : new EntityStats());
				this.stats.Read(_br);
			}
		}
		else
		{
			_br.ReadInt16();
			_br.ReadInt16();
			if (b >= 7)
			{
				_br.ReadInt16();
				_br.ReadInt16();
			}
		}
		this.deathTime = (int)_br.ReadInt16();
		if (b >= 35)
		{
			this.bag = (_br.ReadBoolean() ? Bag.Read(_br) : null);
		}
		else if (b >= 2 && _br.ReadBoolean())
		{
			_br.ReadInt32();
			this.bag = TileEntityLegacyUtils.ReadLegacyLootIntoBag(_br);
		}
		if (b >= 3)
		{
			this.homePosition = new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
			this.homeRange = (int)_br.ReadInt16();
		}
		if (b >= 5)
		{
			this.spawnerSource = (EnumSpawnerSource)_br.ReadByte();
		}
		if (this.entityClass == EntityClass.itemClass)
		{
			if (b <= 5)
			{
				this.belongsPlayerId = (int)_br.ReadInt16();
			}
			else
			{
				this.belongsPlayerId = _br.ReadInt32();
			}
			if (b >= 27)
			{
				this.clientEntityId = _br.ReadInt32();
			}
			this.itemStack = ItemStack.Empty;
			if (b < 14)
			{
				this.itemStack.ReadOld(_br);
			}
			else
			{
				this.itemStack.Read(_br);
			}
			if (b >= 3)
			{
				_br.ReadSByte();
			}
		}
		else if (this.entityClass == EntityClass.fallingBlockClass)
		{
			this.blockValues = new BlockValue[]
			{
				new BlockValue(_br.ReadUInt32())
			};
			this.textureFullArrays = new TextureFullArray[1];
			if (b < 29)
			{
				this.textureFullArrays[0].Fill(0L);
				this.textureFullArrays[0][0] = _br.ReadInt64();
			}
			else
			{
				this.textureFullArrays[0].Read(_br, 1);
			}
		}
		else if (this.entityClass == EntityClass.fallingBlocksClass)
		{
			int num = _br.ReadInt32();
			this.blockValues = new BlockValue[num];
			for (int i = 0; i < this.blockValues.Length; i++)
			{
				this.blockValues[i] = new BlockValue(_br.ReadUInt32());
			}
			this.blockPositions = new Vector3i[num];
			for (int j = 0; j < this.blockPositions.Length; j++)
			{
				this.blockPositions[j] = StreamUtils.ReadVector3i(_br);
			}
			this.textureFullArrays = new TextureFullArray[num];
			for (int k = 0; k < this.textureFullArrays.Length; k++)
			{
				if (b < 29)
				{
					this.textureFullArrays[k].Fill(0L);
					this.textureFullArrays[k][0] = _br.ReadInt64();
				}
				else
				{
					this.textureFullArrays[k].Read(_br, 1);
				}
			}
		}
		else if (this.entityClass == EntityClass.fallingTreeClass)
		{
			this.blockPos = StreamUtils.ReadVector3i(_br);
			this.fallTreeDir = StreamUtils.ReadVector3(_br);
		}
		else if (flag)
		{
			this.holdingItem.Read(_br);
			this.teamNumber = (int)_br.ReadByte();
			this.entityName = _br.ReadString();
			this.skinTexture = _br.ReadString();
			if (b > 12)
			{
				if (_br.ReadBoolean())
				{
					this.playerProfile = PlayerProfile.Read(_br);
				}
				else
				{
					this.playerProfile = null;
				}
			}
		}
		if (b > 9)
		{
			int num2 = (int)_br.ReadUInt16();
			if (num2 > 0)
			{
				byte[] array = _br.ReadBytes(num2);
				this.entityData = new MemoryStream(array);
			}
		}
		if (b > 23 && _br.ReadBoolean())
		{
			if (b >= 34)
			{
				if (this.traderData == null)
				{
					this.traderData = new TraderData();
				}
				this.traderData.Read(_br);
			}
			else
			{
				this.traderData = TileEntityLegacyUtils.ReadLegacyTileEntityTraderData(_br);
			}
		}
		if (_bNetworkRead)
		{
			this.sleeperPose = _br.ReadByte();
			this.isSleeper = _br.ReadBoolean();
			this.spawnById = _br.ReadInt32();
			this.spawnByName = _br.ReadString();
			this.spawnByAllowShare = _br.ReadBoolean();
			this.headState = (EModelBase.HeadStates)_br.ReadByte();
			this.overrideSize = _br.ReadSingle();
			this.overrideHeadSize = _br.ReadSingle();
			this.isDancing = _br.ReadBoolean();
			if (this.isSleeper)
			{
				this.isSleeperPassive = _br.ReadBoolean();
			}
		}
		if (this.entityClass == EntityClass.junkDroneClass)
		{
			if (b > 30)
			{
				this.belongsPlayerId = _br.ReadInt32();
			}
			if (b > 31)
			{
				this.orderState = _br.ReadInt32();
			}
		}
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000DD62C File Offset: 0x000DB82C
	public void write(PooledBinaryWriter _bw, bool _bNetworkWrite)
	{
		_bw.Write(35);
		_bw.Write(this.entityClass);
		_bw.Write(this.id);
		_bw.Write(this.lifetime);
		_bw.Write(this.pos.x);
		_bw.Write(this.pos.y);
		_bw.Write(this.pos.z);
		_bw.Write(this.rot.x);
		_bw.Write(this.rot.y);
		_bw.Write(this.rot.z);
		_bw.Write(this.onGround);
		this.bodyDamage.Write(_bw);
		_bw.Write(this.stats != null);
		if (this.stats != null)
		{
			this.stats.Write(_bw);
		}
		_bw.Write((short)this.deathTime);
		bool flag = this.bag != null;
		_bw.Write(flag);
		if (flag)
		{
			this.bag.Write(_bw);
		}
		_bw.Write(this.homePosition.x);
		_bw.Write(this.homePosition.y);
		_bw.Write(this.homePosition.z);
		_bw.Write((short)this.homeRange);
		_bw.Write((byte)this.spawnerSource);
		if (this.entityClass == EntityClass.itemClass)
		{
			_bw.Write(this.belongsPlayerId);
			_bw.Write(this.clientEntityId);
			this.itemStack.Write(_bw);
			_bw.Write(0);
		}
		else if (this.entityClass == EntityClass.fallingBlockClass)
		{
			_bw.Write(this.blockValues[0].rawData);
			this.textureFullArrays[0].Write(_bw);
		}
		else if (this.entityClass == EntityClass.fallingBlocksClass)
		{
			_bw.Write(this.blockValues.Length);
			for (int i = 0; i < this.blockValues.Length; i++)
			{
				_bw.Write(this.blockValues[i].rawData);
			}
			for (int j = 0; j < this.blockPositions.Length; j++)
			{
				StreamUtils.Write(_bw, this.blockPositions[j]);
			}
			for (int k = 0; k < this.textureFullArrays.Length; k++)
			{
				this.textureFullArrays[k].Write(_bw);
			}
		}
		else if (this.entityClass == EntityClass.fallingTreeClass)
		{
			StreamUtils.Write(_bw, this.blockPos);
			StreamUtils.Write(_bw, this.fallTreeDir);
		}
		else if (this.entityClass == EntityClass.playerMaleClass || this.entityClass == EntityClass.playerFemaleClass)
		{
			ItemValue.Write(this.holdingItem, _bw);
			_bw.Write((byte)this.teamNumber);
			_bw.Write(this.entityName);
			_bw.Write(this.skinTexture);
			_bw.Write(this.playerProfile != null);
			if (this.playerProfile != null)
			{
				this.playerProfile.Write(_bw);
			}
		}
		int num = (int)this.entityData.Length;
		_bw.Write((ushort)num);
		if (num > 0)
		{
			_bw.Write(this.entityData.ToArray());
		}
		_bw.Write(this.traderData != null);
		if (this.traderData != null)
		{
			this.traderData.Write(_bw);
		}
		if (_bNetworkWrite)
		{
			_bw.Write(this.sleeperPose);
			_bw.Write(this.isSleeper);
			_bw.Write(this.spawnById);
			_bw.Write(this.spawnByName);
			_bw.Write(this.spawnByAllowShare);
			_bw.Write((byte)this.headState);
			_bw.Write(this.overrideSize);
			_bw.Write(this.overrideHeadSize);
			_bw.Write(this.isDancing);
			if (this.isSleeper)
			{
				_bw.Write(this.isSleeperPassive);
			}
		}
		if (this.entityClass == EntityClass.junkDroneClass)
		{
			_bw.Write(this.belongsPlayerId);
			_bw.Write(this.orderState);
		}
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x000DDA24 File Offset: 0x000DBC24
	public void readXml(XmlElement _entityElement)
	{
		if (!_entityElement.HasAttribute("type"))
		{
			throw new Exception("No 'type' element found in entity tag!");
		}
		this.entityClass = EntityClass.FromString(_entityElement.GetAttribute("type"));
		if (!_entityElement.HasAttribute("position"))
		{
			throw new Exception("No 'position' element found in entity tag!");
		}
		this.pos = StringParsers.ParseVector3(_entityElement.GetAttribute("position"), 0, -1);
		if (!_entityElement.HasAttribute("rotation"))
		{
			throw new Exception("No 'rotation' element found in entity tag!");
		}
		this.rot = StringParsers.ParseVector3(_entityElement.GetAttribute("rotation"), 0, -1);
		this.id = -1;
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x000DDAC8 File Offset: 0x000DBCC8
	public void writeXml(StreamWriter _sw)
	{
		_sw.WriteLine(string.Concat(new string[]
		{
			"    <entity type=\"",
			EntityClass.list[this.entityClass].entityClassName,
			"\" position=\"",
			this.pos.x.ToCultureInvariantString(),
			",",
			this.pos.y.ToCultureInvariantString(),
			",",
			this.pos.z.ToCultureInvariantString(),
			"\" rotation=\"",
			this.rot.x.ToCultureInvariantString(),
			",",
			this.rot.y.ToCultureInvariantString(),
			",",
			this.rot.z.ToCultureInvariantString(),
			"\" />"
		}));
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000DDBB8 File Offset: 0x000DBDB8
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			EntityClass.list[this.entityClass].entityClassName,
			" ",
			this.entityName,
			" id=",
			this.id.ToString(),
			" pos=",
			this.pos.ToCultureInvariantString()
		});
	}

	// Token: 0x04001AC9 RID: 6857
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FileVersion = 35;

	// Token: 0x04001ACA RID: 6858
	public int entityClass;

	// Token: 0x04001ACB RID: 6859
	public Vector3 pos;

	// Token: 0x04001ACC RID: 6860
	public Vector3 rot;

	// Token: 0x04001ACD RID: 6861
	public int id;

	// Token: 0x04001ACE RID: 6862
	public bool onGround;

	// Token: 0x04001ACF RID: 6863
	public EntityStats stats;

	// Token: 0x04001AD0 RID: 6864
	public int deathTime;

	// Token: 0x04001AD1 RID: 6865
	public float lifetime = float.MaxValue;

	// Token: 0x04001AD2 RID: 6866
	public int belongsPlayerId = -1;

	// Token: 0x04001AD3 RID: 6867
	public int clientEntityId;

	// Token: 0x04001AD4 RID: 6868
	public ItemValue holdingItem = ItemValue.None;

	// Token: 0x04001AD5 RID: 6869
	public int teamNumber;

	// Token: 0x04001AD6 RID: 6870
	public string entityName = "";

	// Token: 0x04001AD7 RID: 6871
	public string skinTexture = "";

	// Token: 0x04001AD8 RID: 6872
	public Bag bag;

	// Token: 0x04001AD9 RID: 6873
	public TraderData traderData;

	// Token: 0x04001ADA RID: 6874
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i homePosition;

	// Token: 0x04001ADB RID: 6875
	[PublicizedFrom(EAccessModifier.Private)]
	public int homeRange = -1;

	// Token: 0x04001ADC RID: 6876
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumSpawnerSource spawnerSource;

	// Token: 0x04001ADD RID: 6877
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04001ADE RID: 6878
	public BlockValue[] blockValues;

	// Token: 0x04001ADF RID: 6879
	public Vector3i[] blockPositions;

	// Token: 0x04001AE0 RID: 6880
	public TextureFullArray[] textureFullArrays;

	// Token: 0x04001AE1 RID: 6881
	public Vector3i blockPos;

	// Token: 0x04001AE2 RID: 6882
	public Vector3 fallTreeDir;

	// Token: 0x04001AE3 RID: 6883
	public int subType;

	// Token: 0x04001AE4 RID: 6884
	public byte sleeperPose = byte.MaxValue;

	// Token: 0x04001AE5 RID: 6885
	public PlayerProfile playerProfile;

	// Token: 0x04001AE6 RID: 6886
	public BodyDamage bodyDamage;

	// Token: 0x04001AE7 RID: 6887
	public bool isSleeper;

	// Token: 0x04001AE8 RID: 6888
	public bool isSleeperPassive;

	// Token: 0x04001AE9 RID: 6889
	public string spawnByName = "";

	// Token: 0x04001AEA RID: 6890
	public int spawnById = -1;

	// Token: 0x04001AEB RID: 6891
	public bool spawnByAllowShare;

	// Token: 0x04001AEC RID: 6892
	public EModelBase.HeadStates headState;

	// Token: 0x04001AED RID: 6893
	public float overrideSize = 1f;

	// Token: 0x04001AEE RID: 6894
	public float overrideHeadSize = 1f;

	// Token: 0x04001AEF RID: 6895
	public bool isDancing;

	// Token: 0x04001AF0 RID: 6896
	public int orderState = -1;

	// Token: 0x04001AF1 RID: 6897
	public byte readFileVersion;

	// Token: 0x04001AF2 RID: 6898
	public MemoryStream entityData = new MemoryStream(0);
}
