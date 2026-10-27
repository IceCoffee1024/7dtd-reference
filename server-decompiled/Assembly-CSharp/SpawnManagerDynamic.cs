using System;
using System.IO;
using System.Xml;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class SpawnManagerDynamic : SpawnManagerAbstract
{
	// Token: 0x060053BF RID: 21439 RVA: 0x00202CD0 File Offset: 0x00200ED0
	public SpawnManagerDynamic(World _world, XmlDocument _spawnXml)
		: base(_world)
	{
		this.lastDaySpawned = -1;
	}

	// Token: 0x060053C0 RID: 21440 RVA: 0x00202CE0 File Offset: 0x00200EE0
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

	// Token: 0x060053C1 RID: 21441 RVA: 0x00202D18 File Offset: 0x00200F18
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

	// Token: 0x060053C2 RID: 21442 RVA: 0x00202D4C File Offset: 0x00200F4C
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

	// Token: 0x040041A5 RID: 16805
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte CurrentFileVersion = 1;

	// Token: 0x040041A6 RID: 16806
	public const int cMinRange = 64;

	// Token: 0x040041A7 RID: 16807
	public const int cMaxRange = 96;

	// Token: 0x040041A8 RID: 16808
	[PublicizedFrom(EAccessModifier.Private)]
	public EntitySpawner currentSpawner;

	// Token: 0x040041A9 RID: 16809
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDaySpawned;
}
