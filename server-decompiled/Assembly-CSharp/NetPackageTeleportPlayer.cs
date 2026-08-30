using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000858 RID: 2136
[Preserve]
public class NetPackageTeleportPlayer : NetPackage
{
	// Token: 0x06003E74 RID: 15988 RVA: 0x0018A5C6 File Offset: 0x001887C6
	public NetPackageTeleportPlayer Setup(Vector3 _newPos, Vector3? _viewDirection = null, bool _onlyIfNotFlying = false)
	{
		this.pos = _newPos;
		this.viewDirection = _viewDirection;
		this.onlyIfNotFlying = _onlyIfNotFlying;
		return this;
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0018A5E0 File Offset: 0x001887E0
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

	// Token: 0x06003E76 RID: 15990 RVA: 0x0018A650 File Offset: 0x00188850
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

	// Token: 0x06003E77 RID: 15991 RVA: 0x0018A70C File Offset: 0x0018890C
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

	// Token: 0x06003E78 RID: 15992 RVA: 0x000F9E96 File Offset: 0x000F8096
	public override int GetLength()
	{
		return 13;
	}

	// Token: 0x04003292 RID: 12946
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x04003293 RID: 12947
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3? viewDirection;

	// Token: 0x04003294 RID: 12948
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onlyIfNotFlying;
}
