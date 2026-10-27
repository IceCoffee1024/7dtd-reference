using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000865 RID: 2149
[Preserve]
public class NetPackageTeleportPlayer : NetPackage
{
	// Token: 0x06003E3E RID: 15934 RVA: 0x0018B51A File Offset: 0x0018971A
	public NetPackageTeleportPlayer Setup(Vector3 _newPos, Vector3? _viewDirection = null, bool _onlyIfNotFlying = false)
	{
		this.pos = _newPos;
		this.viewDirection = _viewDirection;
		this.onlyIfNotFlying = _onlyIfNotFlying;
		return this;
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x0018B534 File Offset: 0x00189734
	public override void read(PooledBinaryReader _reader)
	{
		this.pos = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
		if (_reader.ReadBoolean())
		{
			this.viewDirection = new Vector3?(new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle()));
		}
		else
		{
			this.viewDirection = null;
		}
		this.onlyIfNotFlying = _reader.ReadBoolean();
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x0018B5A4 File Offset: 0x001897A4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.pos.x);
		_writer.Write(this.pos.y);
		_writer.Write(this.pos.z);
		_writer.Write(this.viewDirection != null);
		if (this.viewDirection != null)
		{
			_writer.Write(this.viewDirection.Value.x);
			_writer.Write(this.viewDirection.Value.y);
			_writer.Write(this.viewDirection.Value.z);
		}
		_writer.Write(this.onlyIfNotFlying);
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x0018B660 File Offset: 0x00189860
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			primaryPlayer.TeleportToPosition(this.pos, this.onlyIfNotFlying, this.viewDirection);
			return;
		}
		Log.Out("Discarding " + base.GetType().Name + " (no local player)");
	}

	// Token: 0x0400329D RID: 12957
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x0400329E RID: 12958
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3? viewDirection;

	// Token: 0x0400329F RID: 12959
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onlyIfNotFlying;
}
