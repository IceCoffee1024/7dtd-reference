using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F2 RID: 2034
[Preserve]
public class NetPackageEntityVelocity : NetPackageEntityTargeted
{
	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06003C02 RID: 15362 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x00181014 File Offset: 0x0017F214
	public NetPackageEntityVelocity Setup(int _entityId, Vector3 _motion, bool _bAdd)
	{
		base.Setup(_entityId);
		this.bAdd = _bAdd;
		if (_motion.x < -8f)
		{
			_motion.x = -8f;
		}
		if (_motion.y < -8f)
		{
			_motion.y = -8f;
		}
		if (_motion.z < -8f)
		{
			_motion.z = -8f;
		}
		if (_motion.x > 8f)
		{
			_motion.x = 8f;
		}
		if (_motion.y > 8f)
		{
			_motion.y = 8f;
		}
		if (_motion.z > 8f)
		{
			_motion.z = 8f;
		}
		this.motion = _motion;
		return this;
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x001810CD File Offset: 0x0017F2CD
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.bAdd = _reader.ReadBoolean();
		this.motion = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x00181100 File Offset: 0x0017F300
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.bAdd);
		_writer.Write(this.motion.x);
		_writer.Write(this.motion.y);
		_writer.Write(this.motion.z);
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x00181154 File Offset: 0x0017F354
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (!(entity != null))
		{
			Log.Out("Discarding " + base.GetType().Name + " for entity Id=" + this.entityId.ToString());
			return;
		}
		if (!this.bAdd)
		{
			entity.SetVelocity(this.motion);
			return;
		}
		entity.AddVelocity(this.motion);
	}

	// Token: 0x040030BD RID: 12477
	[PublicizedFrom(EAccessModifier.Private)]
	public const float max = 8f;

	// Token: 0x040030BE RID: 12478
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 motion;

	// Token: 0x040030BF RID: 12479
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAdd;
}
