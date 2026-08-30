using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007FD RID: 2045
[Preserve]
public class NetPackageHordeEvent : NetPackage
{
	// Token: 0x06003C4B RID: 15435 RVA: 0x00182087 File Offset: 0x00180287
	public NetPackageHordeEvent Setup(AIDirector.HordeEvent _event, Vector3 pos, float maxDist)
	{
		this.m_event = _event;
		this.m_pos = pos;
		this.m_maxDist = maxDist;
		return this;
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x001820A0 File Offset: 0x001802A0
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadByte();
		this.m_event = (AIDirector.HordeEvent)num;
		this.m_pos[0] = _reader.ReadSingle();
		this.m_pos[1] = _reader.ReadSingle();
		this.m_pos[2] = _reader.ReadSingle();
		this.m_maxDist = _reader.ReadSingle();
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x00182100 File Offset: 0x00180300
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.m_event);
		_writer.Write(this.m_pos[0]);
		_writer.Write(this.m_pos[1]);
		_writer.Write(this.m_pos[2]);
		_writer.Write(this.m_maxDist);
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x00182164 File Offset: 0x00180364
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

	// Token: 0x06003C4F RID: 15439 RVA: 0x0005F529 File Offset: 0x0005D729
	public override int GetLength()
	{
		return 21;
	}

	// Token: 0x0400310F RID: 12559
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirector.HordeEvent m_event;

	// Token: 0x04003110 RID: 12560
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x04003111 RID: 12561
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxDist;
}
