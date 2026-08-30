using System;
using System.Collections.Generic;

// Token: 0x02000ACC RID: 2764
public class EntityGroupSpawnState
{
	// Token: 0x060052A4 RID: 21156 RVA: 0x001F9890 File Offset: 0x001F7A90
	public EntityGroupSpawnState(string _sEntityGroupName)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		for (int i = 0; i < list.Count; i++)
		{
			this.state.Add(new EntityGroupSpawnState.State(list[i]));
		}
	}

	// Token: 0x060052A5 RID: 21157 RVA: 0x001F98E4 File Offset: 0x001F7AE4
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

	// Token: 0x060052A6 RID: 21158 RVA: 0x001F9954 File Offset: 0x001F7B54
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

	// Token: 0x0400403E RID: 16446
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityGroupSpawnState.State> state = new List<EntityGroupSpawnState.State>();

	// Token: 0x02000ACD RID: 2765
	[PublicizedFrom(EAccessModifier.Private)]
	public struct State
	{
		// Token: 0x060052A7 RID: 21159 RVA: 0x001F99A6 File Offset: 0x001F7BA6
		public State(SEntityClassAndProb _src)
		{
			this.entityClassId = _src.entityClassId;
			this.prob = _src.prob;
			this.numSpawned = 0;
		}

		// Token: 0x0400403F RID: 16447
		public readonly int entityClassId;

		// Token: 0x04004040 RID: 16448
		public readonly float prob;

		// Token: 0x04004041 RID: 16449
		public int numSpawned;
	}
}
