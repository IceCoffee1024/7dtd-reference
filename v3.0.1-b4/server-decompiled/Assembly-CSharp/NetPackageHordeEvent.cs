using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F2 RID: 2034
[Preserve]
public class NetPackageHordeEvent : NetPackage
{
	// Token: 0x06003BEC RID: 15340 RVA: 0x0017FCAF File Offset: 0x0017DEAF
	public NetPackageHordeEvent Setup(AIDirector.HordeEvent _event, Vector3 pos, float maxDist)
	{
		this.m_event = _event;
		this.m_pos = pos;
		this.m_maxDist = maxDist;
		return this;
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x0017FCC8 File Offset: 0x0017DEC8
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadByte();
		this.m_event = (AIDirector.HordeEvent)num;
		this.m_pos[0] = _reader.ReadSingle();
		this.m_pos[1] = _reader.ReadSingle();
		this.m_pos[2] = _reader.ReadSingle();
		this.m_maxDist = _reader.ReadSingle();
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x0017FD28 File Offset: 0x0017DF28
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_event);
		_writer.Write(this.m_pos[0]);
		_writer.Write(this.m_pos[1]);
		_writer.Write(this.m_pos[2]);
		_writer.Write(this.m_maxDist);
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x0017FD8C File Offset: 0x0017DF8C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer == null)
		{
			return;
		}
		if ((primaryPlayer.GetPosition() - this.m_pos).sqrMagnitude <= this.m_maxDist * this.m_maxDist)
		{
			primaryPlayer.HandleHordeEvent(this.m_event);
		}
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x0005EFE1 File Offset: 0x0005D1E1
	public override int GetLength()
	{
		return 21;
	}

	// Token: 0x04003090 RID: 12432
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirector.HordeEvent m_event;

	// Token: 0x04003091 RID: 12433
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x04003092 RID: 12434
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxDist;
}
