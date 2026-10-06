using System;
using System.Collections.Generic;

// Token: 0x02000AF0 RID: 2800
public class EntityGroupSpawnState
{
	// Token: 0x06005345 RID: 21317 RVA: 0x001FF760 File Offset: 0x001FD960
	public EntityGroupSpawnState(string _sEntityGroupName)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		for (int i = 0; i < list.Count; i++)
		{
			this.state.Add(new EntityGroupSpawnState.State(list[i]));
		}
	}

	// Token: 0x06005346 RID: 21318 RVA: 0x001FF7B4 File Offset: 0x001FD9B4
	public int GetRandomFromGroup()
	{
		float randomFloat = GameManager.Instance.World.GetGameRandom().RandomFloat;
		float num = 0f;
		for (int i = 0; i < this.state.Count; i++)
		{
			EntityGroupSpawnState.State state = this.state[i];
			num += state.prob;
			if (randomFloat <= num && state.prob > 0f)
			{
				return state.entityClassId;
			}
		}
		return -1;
	}

	// Token: 0x06005347 RID: 21319 RVA: 0x001FF824 File Offset: 0x001FDA24
	public void DidSpawn(int _classId)
	{
		for (int i = 0; i < this.state.Count; i++)
		{
			EntityGroupSpawnState.State state = this.state[i];
			if (state.entityClassId == _classId)
			{
				state.numSpawned++;
			}
			this.state[i] = state;
		}
	}

	// Token: 0x04004111 RID: 16657
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityGroupSpawnState.State> state = new List<EntityGroupSpawnState.State>();

	// Token: 0x02000AF1 RID: 2801
	[PublicizedFrom(EAccessModifier.Private)]
	public struct State
	{
		// Token: 0x06005348 RID: 21320 RVA: 0x001FF876 File Offset: 0x001FDA76
		public State(SEntityClassAndProb _src)
		{
			this.entityClassId = _src.entityClassId;
			this.prob = _src.prob;
			this.numSpawned = 0;
		}

		// Token: 0x04004112 RID: 16658
		public readonly int entityClassId;

		// Token: 0x04004113 RID: 16659
		public readonly float prob;

		// Token: 0x04004114 RID: 16660
		public int numSpawned;
	}
}
