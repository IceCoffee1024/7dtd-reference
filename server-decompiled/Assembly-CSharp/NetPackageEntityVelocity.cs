using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007FC RID: 2044
[Preserve]
public class NetPackageEntityVelocity : NetPackageEntityTargeted
{
	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06003C25 RID: 15397 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x00182118 File Offset: 0x00180318
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

	// Token: 0x06003C27 RID: 15399 RVA: 0x001821D1 File Offset: 0x001803D1
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.bAdd = _reader.ReadBoolean();
		this.motion = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x00182204 File Offset: 0x00180404
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.bAdd);
		_writer.Write(this.motion.x);
		_writer.Write(this.motion.y);
		_writer.Write(this.motion.z);
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x00182258 File Offset: 0x00180458
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

	// Token: 0x040030DD RID: 12509
	[PublicizedFrom(EAccessModifier.Private)]
	public const float max = 8f;

	// Token: 0x040030DE RID: 12510
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 motion;

	// Token: 0x040030DF RID: 12511
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAdd;
}
