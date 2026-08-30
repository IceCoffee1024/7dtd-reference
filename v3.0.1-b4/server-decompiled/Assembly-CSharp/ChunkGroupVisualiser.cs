using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020013A0 RID: 5024
public class ChunkGroupVisualiser : SingletonMonoBehaviour<ChunkGroupVisualiser>
{
	// Token: 0x06009DF9 RID: 40441 RVA: 0x003B9AB4 File Offset: 0x003B7CB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrawGizmos()
	{
		Vector3 vector = Vector3.one * 16f;
		vector.y = 128f;
		Vector3 vector2 = -Origin.Instance.OriginPos;
		vector2.y = 0f;
		Gizmos.matrix = Matrix4x4.Translate(vector2);
		foreach (ValueTuple<Color, List<Vector3>> valueTuple in this.groupInfos)
		{
			Gizmos.color = valueTuple.Item1;
			foreach (Vector3 vector3 in valueTuple.Item2)
			{
				Gizmos.DrawCube(vector3, vector);
			}
		}
	}

	// Token: 0x06009DFA RID: 40442 RVA: 0x003B9B8C File Offset: 0x003B7D8C
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetGroups(IEnumerable<LongSetGroups.Group> chunkGroups)
	{
		this.groupInfos.Clear();
		global::UnityEngine.Random.InitState(42);
		foreach (LongSetGroups.Group group in chunkGroups)
		{
			ValueTuple<Color, List<Vector3>> valueTuple = new ValueTuple<Color, List<Vector3>>(global::UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f, 0.25f, 0.25f), new List<Vector3>(group.Count));
			foreach (long num in group.Keys)
			{
				int num2 = WorldChunkCache.extractX(num) * 16 + 8;
				int num3 = WorldChunkCache.extractZ(num) * 16 + 8;
				valueTuple.Item2.Add(new Vector3((float)num2, 8f, (float)num3));
			}
			this.groupInfos.Add(valueTuple);
		}
	}

	// Token: 0x0400779C RID: 30620
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float alpha = 0.25f;

	// Token: 0x0400779D RID: 30621
	[TupleElementNames(new string[] { "color", "positions" })]
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<ValueTuple<Color, List<Vector3>>> groupInfos = new List<ValueTuple<Color, List<Vector3>>>();
}
