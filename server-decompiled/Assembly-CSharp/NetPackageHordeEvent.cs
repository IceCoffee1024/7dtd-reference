using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000800 RID: 2048
[Preserve]
public class NetPackageHordeEvent : NetPackage
{
	// Token: 0x06003C46 RID: 15430 RVA: 0x0018258F File Offset: 0x0018078F
	public NetPackageHordeEvent Setup(AIDirector.HordeEvent _event, Vector3 pos, float maxDist)
	{
		this.m_event = _event;
		this.m_pos = pos;
		this.m_maxDist = maxDist;
		return this;
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x001825A8 File Offset: 0x001807A8
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadByte();
		this.m_event = (AIDirector.HordeEvent)num;
		this.m_pos[0] = _reader.ReadSingle();
		this.m_pos[1] = _reader.ReadSingle();
		this.m_pos[2] = _reader.ReadSingle();
		this.m_maxDist = _reader.ReadSingle();
	}

	// Token: 0x06003C48 RID: 15432 RVA: 0x00182608 File Offset: 0x00180808
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_event);
		_writer.Write(this.m_pos[0]);
		_writer.Write(this.m_pos[1]);
		_writer.Write(this.m_pos[2]);
		_writer.Write(this.m_maxDist);
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x0018266C File Offset: 0x0018086C
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

	// Token: 0x04003108 RID: 12552
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirector.HordeEvent m_event;

	// Token: 0x04003109 RID: 12553
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x0400310A RID: 12554
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxDist;
}
