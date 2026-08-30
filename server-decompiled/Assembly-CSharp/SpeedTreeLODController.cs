using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001357 RID: 4951
public class SpeedTreeLODController : MonoBehaviour
{
	// Token: 0x06009BFD RID: 39933 RVA: 0x003AF850 File Offset: 0x003ADA50
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

	// Token: 0x06009BFE RID: 39934 RVA: 0x003AF8DC File Offset: 0x003ADADC
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

	// Token: 0x040075F8 RID: 30200
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<Tree> tempTrees = new List<Tree>();
}
