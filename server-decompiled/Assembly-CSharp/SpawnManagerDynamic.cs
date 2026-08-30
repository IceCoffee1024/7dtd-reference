using System;
using System.IO;
using System.Xml;
using UnityEngine;

// Token: 0x02000AE0 RID: 2784
public class SpawnManagerDynamic : SpawnManagerAbstract
{
	// Token: 0x060052F9 RID: 21241 RVA: 0x001FB87C File Offset: 0x001F9A7C
	public SpawnManagerDynamic(World _world, XmlDocument _spawnXml)
		: base(_world)
	{
		this.lastDaySpawned = -1;
	}

	// Token: 0x060052FA RID: 21242 RVA: 0x001FB88C File Offset: 0x001F9A8C
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(1);
		_bw.Write(this.currentSpawner != null);
		if (this.currentSpawner != null)
		{
			_bw.Write(this.lastDaySpawned);
			this.currentSpawner.Write(_bw);
		}
	}

	// Token: 0x060052FB RID: 21243 RVA: 0x001FB8C4 File Offset: 0x001F9AC4
	public void Read(BinaryReader _br)
	{
		_br.ReadByte();
		if (_br.ReadBoolean())
		{
			this.currentSpawner = new EntitySpawner();
			this.lastDaySpawned = _br.ReadInt32();
			this.currentSpawner.Read(_br);
		}
	}

	// Token: 0x060052FC RID: 21244 RVA: 0x001FB8F8 File Offset: 0x001F9AF8
	public override void Update(string _spawnerName, bool _bSpawnEnemyEntities, object _userData)
	{
		if (this.world.IsDaytime())
		{
			return;
		}
		if (this.world.Players.list.Count == 0)
		{
			return;
		}
		int num = GameUtils.WorldTimeToDays(this.world.worldTime);
		if (num != this.lastDaySpawned || this.currentSpawner == null)
		{
			this.lastDaySpawned = num;
			EntitySpawner entitySpawner = this.currentSpawner;
			Log.Out("New ES '" + _spawnerName + "' for day: " + num.ToString());
			this.currentSpawner = new EntitySpawner(_spawnerName, Vector3i.zero, Vector3i.zero, 0, (entitySpawner != null) ? entitySpawner.GetEntityIdsSpaned() : null);
		}
		if (this.currentSpawner != null)
		{
			this.currentSpawner.SpawnManually(this.world, num, _bSpawnEnemyEntities, delegate(EntitySpawner _es, out EntityPlayer _outPlayerToAttack)
			{
				_outPlayerToAttack = null;
				return true;
			}, delegate(EntitySpawner _es, EntityPlayer _inPlayerToAttack, out EntityPlayer _outPlayerToAttack, out Vector3 _pos)
			{
				return this.world.GetRandomSpawnPositionMinMaxToRandomPlayer(64, 96, true, out _outPlayerToAttack, out _pos);
			}, null, null);
		}
	}

	// Token: 0x040040AD RID: 16557
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte CurrentFileVersion = 1;

	// Token: 0x040040AE RID: 16558
	public const int cMinRange = 64;

	// Token: 0x040040AF RID: 16559
	public const int cMaxRange = 96;

	// Token: 0x040040B0 RID: 16560
	[PublicizedFrom(EAccessModifier.Private)]
	public EntitySpawner currentSpawner;

	// Token: 0x040040B1 RID: 16561
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDaySpawned;
}
