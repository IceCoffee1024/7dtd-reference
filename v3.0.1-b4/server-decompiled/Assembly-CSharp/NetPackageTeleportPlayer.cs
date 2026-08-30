using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200084C RID: 2124
[Preserve]
public class NetPackageTeleportPlayer : NetPackage
{
	// Token: 0x06003E0F RID: 15887 RVA: 0x00188592 File Offset: 0x00186792
	public NetPackageTeleportPlayer Setup(Vector3 _newPos, Vector3? _viewDirection = null, bool _onlyIfNotFlying = false)
	{
		this.pos = _newPos;
		this.viewDirection = _viewDirection;
		this.onlyIfNotFlying = _onlyIfNotFlying;
		return this;
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x001885AC File Offset: 0x001867AC
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

	// Token: 0x06003E11 RID: 15889 RVA: 0x0018861C File Offset: 0x0018681C
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

	// Token: 0x06003E12 RID: 15890 RVA: 0x001886D8 File Offset: 0x001868D8
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

	// Token: 0x06003E13 RID: 15891 RVA: 0x000F8E8A File Offset: 0x000F708A
	public override int GetLength()
	{
		return 13;
	}

	// Token: 0x04003213 RID: 12819
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x04003214 RID: 12820
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3? viewDirection;

	// Token: 0x04003215 RID: 12821
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onlyIfNotFlying;
}
