using System;
using System.Collections.Generic;

// Token: 0x02000AFB RID: 2811
public class EntityGroupSpawnState
{
	// Token: 0x0600536A RID: 21354 RVA: 0x00200CE4 File Offset: 0x001FEEE4
	public EntityGroupSpawnState(string _sEntityGroupName)
	{
		List<SEntityClassAndProb> list = EntityGroups.list[_sEntityGroupName];
		for (int i = 0; i < list.Count; i++)
		{
			this.state.Add(new EntityGroupSpawnState.State(list[i]));
		}
	}

	// Token: 0x0600536B RID: 21355 RVA: 0x00200D38 File Offset: 0x001FEF38
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

	// Token: 0x0600536C RID: 21356 RVA: 0x00200DA8 File Offset: 0x001FEFA8
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

	// Token: 0x04004136 RID: 16694
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityGroupSpawnState.State> state = new List<EntityGroupSpawnState.State>();

	// Token: 0x02000AFC RID: 2812
	[PublicizedFrom(EAccessModifier.Private)]
	public struct State
	{
		// Token: 0x0600536D RID: 21357 RVA: 0x00200DFA File Offset: 0x001FEFFA
		public State(SEntityClassAndProb _src)
		{
			this.entityClassId = _src.entityClassId;
			this.prob = _src.prob;
			this.numSpawned = 0;
		}

		// Token: 0x04004137 RID: 16695
		public readonly int entityClassId;

		// Token: 0x04004138 RID: 16696
		public readonly float prob;

		// Token: 0x04004139 RID: 16697
		public int numSpawned;
	}
}
