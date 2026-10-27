using System;

// Token: 0x02000900 RID: 2304
public class QuestPositionData
{
	// Token: 0x0600420C RID: 16908 RVA: 0x0000640C File Offset: 0x0000460C
	public QuestPositionData()
	{
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x0019EE0A File Offset: 0x0019D00A
	public QuestPositionData(int questCode, Quest.PositionDataTypes positionDataType, Vector3i blockPosition)
	{
		this.questCode = questCode;
		this.positionDataType = positionDataType;
		this.blockPosition = blockPosition;
	}

	// Token: 0x0600420E RID: 16910 RVA: 0x0019EE27 File Offset: 0x0019D027
	public static QuestPositionData Read(PooledBinaryReader reader)
	{
		return new QuestPositionData
		{
			questCode = reader.ReadInt32(),
			positionDataType = (Quest.PositionDataTypes)reader.ReadInt32(),
			blockPosition = StreamUtils.ReadVector3i(reader)
		};
	}

	// Token: 0x0600420F RID: 16911 RVA: 0x0019EE52 File Offset: 0x0019D052
	public void Write(PooledBinaryWriter writer)
	{
		writer.Write(this.questCode);
		writer.Write((int)this.positionDataType);
		StreamUtils.Write(writer, this.blockPosition);
	}

	// Token: 0x040035D0 RID: 13776
	public int questCode;

	// Token: 0x040035D1 RID: 13777
	public Quest.PositionDataTypes positionDataType;

	// Token: 0x040035D2 RID: 13778
	public Vector3i blockPosition;
}
