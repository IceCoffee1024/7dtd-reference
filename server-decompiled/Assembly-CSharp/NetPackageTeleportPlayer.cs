using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200085B RID: 2139
[Preserve]
public class NetPackageTeleportPlayer : NetPackage
{
	// Token: 0x06003E1B RID: 15899 RVA: 0x0018A472 File Offset: 0x00188672
	public NetPackageTeleportPlayer Setup(Vector3 _newPos, Vector3? _viewDirection = null, bool _onlyIfNotFlying = false)
	{
		this.pos = _newPos;
		this.viewDirection = _viewDirection;
		this.onlyIfNotFlying = _onlyIfNotFlying;
		return this;
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x0018A48C File Offset: 0x0018868C
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

	// Token: 0x06003E1D RID: 15901 RVA: 0x0018A4FC File Offset: 0x001886FC
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

	// Token: 0x06003E1E RID: 15902 RVA: 0x0018A5B8 File Offset: 0x001887B8
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

	// Token: 0x0400327D RID: 12925
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x0400327E RID: 12926
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3? viewDirection;

	// Token: 0x0400327F RID: 12927
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onlyIfNotFlying;
}
