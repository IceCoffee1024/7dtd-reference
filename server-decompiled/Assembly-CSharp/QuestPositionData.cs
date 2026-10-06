using System;

// Token: 0x020008F6 RID: 2294
public class QuestPositionData
{
	// Token: 0x060041E9 RID: 16873 RVA: 0x0000640C File Offset: 0x0000460C
	public QuestPositionData()
	{
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x0019DD62 File Offset: 0x0019BF62
	public QuestPositionData(int questCode, Quest.PositionDataTypes positionDataType, Vector3i blockPosition)
	{
		this.questCode = questCode;
		this.positionDataType = positionDataType;
		this.blockPosition = blockPosition;
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x0019DD7F File Offset: 0x0019BF7F
	public static QuestPositionData Read(PooledBinaryReader reader)
	{
		return new QuestPositionData
		{
			questCode = reader.ReadInt32(),
			positionDataType = (Quest.PositionDataTypes)reader.ReadInt32(),
			blockPosition = StreamUtils.ReadVector3i(reader)
		};
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x0019DDAA File Offset: 0x0019BFAA
	public void Write(PooledBinaryWriter writer)
	{
		writer.Write(this.questCode);
		writer.Write((int)this.positionDataType);
		StreamUtils.Write(writer, this.blockPosition);
	}

	// Token: 0x040035B0 RID: 13744
	public int questCode;

	// Token: 0x040035B1 RID: 13745
	public Quest.PositionDataTypes positionDataType;

	// Token: 0x040035B2 RID: 13746
	public Vector3i blockPosition;
}
