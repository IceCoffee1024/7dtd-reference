using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E4 RID: 2020
[Preserve]
public class NetPackageEntityVelocity : NetPackageEntityTargeted
{
	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06003B9E RID: 15262 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0017E704 File Offset: 0x0017C904
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

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0017E7BD File Offset: 0x0017C9BD
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.bAdd = _reader.ReadBoolean();
		this.motion = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0017E7F0 File Offset: 0x0017C9F0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.bAdd);
		_writer.Write(this.motion.x);
		_writer.Write(this.motion.y);
		_writer.Write(this.motion.z);
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0017E844 File Offset: 0x0017CA44
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

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04003045 RID: 12357
	[PublicizedFrom(EAccessModifier.Private)]
	public const float max = 8f;

	// Token: 0x04003046 RID: 12358
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 motion;

	// Token: 0x04003047 RID: 12359
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAdd;
}
