using System;
using System.IO;
using System.Xml;
using UnityEngine;

// Token: 0x02000AF2 RID: 2802
public class SpawnManagerDynamic : SpawnManagerAbstract
{
	// Token: 0x06005381 RID: 21377 RVA: 0x001FE4EC File Offset: 0x001FC6EC
	public SpawnManagerDynamic(World _world, XmlDocument _spawnXml)
		: base(_world)
	{
		this.lastDaySpawned = -1;
	}

	// Token: 0x06005382 RID: 21378 RVA: 0x001FE4FC File Offset: 0x001FC6FC
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

	// Token: 0x06005383 RID: 21379 RVA: 0x001FE534 File Offset: 0x001FC734
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

	// Token: 0x06005384 RID: 21380 RVA: 0x001FE568 File Offset: 0x001FC768
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

	// Token: 0x04004143 RID: 16707
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte CurrentFileVersion = 1;

	// Token: 0x04004144 RID: 16708
	public const int cMinRange = 64;

	// Token: 0x04004145 RID: 16709
	public const int cMaxRange = 96;

	// Token: 0x04004146 RID: 16710
	[PublicizedFrom(EAccessModifier.Private)]
	public EntitySpawner currentSpawner;

	// Token: 0x04004147 RID: 16711
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDaySpawned;
}
