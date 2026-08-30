using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007EF RID: 2031
[Preserve]
public class NetPackageEntityVelocity : NetPackageEntityTargeted
{
	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06003BFD RID: 15357 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x00180ADC File Offset: 0x0017ECDC
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

	// Token: 0x06003BFF RID: 15359 RVA: 0x00180B95 File Offset: 0x0017ED95
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.bAdd = _reader.ReadBoolean();
		this.motion = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x00180BC8 File Offset: 0x0017EDC8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.bAdd);
		_writer.Write(this.motion.x);
		_writer.Write(this.motion.y);
		_writer.Write(this.motion.z);
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x00180C1C File Offset: 0x0017EE1C
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

	// Token: 0x06003C02 RID: 15362 RVA: 0x0017E246 File Offset: 0x0017C446
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x040030C4 RID: 12484
	[PublicizedFrom(EAccessModifier.Private)]
	public const float max = 8f;

	// Token: 0x040030C5 RID: 12485
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 motion;

	// Token: 0x040030C6 RID: 12486
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAdd;
}
