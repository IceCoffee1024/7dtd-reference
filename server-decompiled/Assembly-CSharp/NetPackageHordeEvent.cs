using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200080A RID: 2058
[Preserve]
public class NetPackageHordeEvent : NetPackage
{
	// Token: 0x06003C69 RID: 15465 RVA: 0x00183693 File Offset: 0x00181893
	public NetPackageHordeEvent Setup(AIDirector.HordeEvent _event, Vector3 pos, float maxDist)
	{
		this.m_event = _event;
		this.m_pos = pos;
		this.m_maxDist = maxDist;
		return this;
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x001836AC File Offset: 0x001818AC
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadByte();
		this.m_event = (AIDirector.HordeEvent)num;
		this.m_pos[0] = _reader.ReadSingle();
		this.m_pos[1] = _reader.ReadSingle();
		this.m_pos[2] = _reader.ReadSingle();
		this.m_maxDist = _reader.ReadSingle();
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x0018370C File Offset: 0x0018190C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_event);
		_writer.Write(this.m_pos[0]);
		_writer.Write(this.m_pos[1]);
		_writer.Write(this.m_pos[2]);
		_writer.Write(this.m_maxDist);
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x00183770 File Offset: 0x00181970
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

	// Token: 0x04003128 RID: 12584
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirector.HordeEvent m_event;

	// Token: 0x04003129 RID: 12585
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x0400312A RID: 12586
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxDist;
}
