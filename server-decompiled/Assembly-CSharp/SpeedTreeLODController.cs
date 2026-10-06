using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001391 RID: 5009
public class SpeedTreeLODController : MonoBehaviour
{
	// Token: 0x06009DA4 RID: 40356 RVA: 0x003B9674 File Offset: 0x003B7874
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		base.GetComponentsInChildren<Tree>(SpeedTreeLODController.tempTrees);
		foreach (Tree tree in SpeedTreeLODController.tempTrees)
		{
			Renderer renderer;
			if (tree.hasSpeedTreeWind && tree.TryGetComponent<Renderer>(out renderer) && renderer.motionVectorGenerationMode == MotionVectorGenerationMode.Object)
			{
				tree.gameObject.AddMissingComponent<SpeedTreeMotionVectorHelper>().Init(renderer);
			}
		}
		SpeedTreeLODController.tempTrees.Clear();
	}

	// Token: 0x06009DA5 RID: 40357 RVA: 0x003B9700 File Offset: 0x003B7900
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		LODGroup component = base.GetComponent<LODGroup>();
		int lodCount = component.lodCount;
		if (lodCount > 0)
		{
			LOD[] lods = component.GetLODs();
			float num = Utils.FastLerp(0.02f, 0.03f, component.size / 5f);
			lods[lodCount - 1].screenRelativeTransitionHeight = num;
			if (lodCount > 1)
			{
				float num2 = 0.17f;
				float num3 = (0.58f - num2) / ((float)(lodCount - 2) + 0.001f);
				for (int i = lodCount - 2; i >= 0; i--)
				{
					lods[i].screenRelativeTransitionHeight = num2;
					num2 += num3;
				}
			}
			component.SetLODs(lods);
		}
	}

	// Token: 0x0400774B RID: 30539
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<Tree> tempTrees = new List<Tree>();
}
