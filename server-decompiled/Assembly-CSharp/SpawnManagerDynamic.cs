using System;
using System.IO;
using System.Xml;
using UnityEngine;

// Token: 0x02000B04 RID: 2820
public class SpawnManagerDynamic : SpawnManagerAbstract
{
	// Token: 0x0600539A RID: 21402 RVA: 0x0020174C File Offset: 0x001FF94C
	public SpawnManagerDynamic(World _world, XmlDocument _spawnXml)
		: base(_world)
	{
		this.lastDaySpawned = -1;
	}

	// Token: 0x0600539B RID: 21403 RVA: 0x0020175C File Offset: 0x001FF95C
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

	// Token: 0x0600539C RID: 21404 RVA: 0x00201794 File Offset: 0x001FF994
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

	// Token: 0x0600539D RID: 21405 RVA: 0x002017C8 File Offset: 0x001FF9C8
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

	// Token: 0x04004180 RID: 16768
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte CurrentFileVersion = 1;

	// Token: 0x04004181 RID: 16769
	public const int cMinRange = 64;

	// Token: 0x04004182 RID: 16770
	public const int cMaxRange = 96;

	// Token: 0x04004183 RID: 16771
	[PublicizedFrom(EAccessModifier.Private)]
	public EntitySpawner currentSpawner;

	// Token: 0x04004184 RID: 16772
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDaySpawned;
}
