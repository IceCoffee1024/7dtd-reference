using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009B6 RID: 2486
public class ObjectiveRallyPointData : MonoBehaviour
{
	// Token: 0x060048A6 RID: 18598 RVA: 0x001C8B78 File Offset: 0x001C6D78
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		for (int i = 0; i < this.FlagParents.Length; i++)
		{
			GameObject gameObject = this.FlagParents[i];
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
			{
				if (gameObject.transform != transform)
				{
					GameObject gameObject2 = transform.gameObject;
					if (gameObject2.name.EndsWith("Highlight"))
					{
						list2.Add(gameObject2);
					}
					else
					{
						list.Add(gameObject2);
					}
				}
			}
			list.Sort((GameObject x, GameObject y) => x.name.CompareTo(y.name));
			this.flagNodes.Add(gameObject, list);
			list2.Sort((GameObject x, GameObject y) => x.name.CompareTo(y.name));
			this.highlightNodes.Add(gameObject, list2);
			this.flagCounts = new int[this.Flags.Length];
		}
		GameObject[] flags = this.Flags;
		for (int j = 0; j < flags.Length; j++)
		{
			flags[j].SetActive(false);
		}
		this.HelperFlag.SetActive(true);
		this.Highlight.SetActive(false);
		this.Root.SetActive(GameManager.Instance.World.IsEditor());
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x001C8CE4 File Offset: 0x001C6EE4
	public void ClearAllFlags()
	{
		for (int i = 0; i < this.Flags.Length; i++)
		{
			this.flagCounts[i] = 0;
		}
		this.highlightFlag = -1;
	}

	// Token: 0x060048A9 RID: 18601 RVA: 0x001C8D14 File Offset: 0x001C6F14
	public void AddFlag(string flagName, bool highLight)
	{
		for (int i = 0; i < this.Flags.Length; i++)
		{
			if (this.Flags[i].name.Equals(flagName))
			{
				if (highLight)
				{
					this.highlightFlag = i;
				}
				this.flagCounts[i]++;
			}
		}
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x001C8D64 File Offset: 0x001C6F64
	public void RemoveFlag(string flagName)
	{
		for (int i = 0; i < this.Flags.Length; i++)
		{
			if (this.Flags[i].name.Equals(flagName) && this.flagCounts[i] > 0)
			{
				this.flagCounts[i]--;
			}
		}
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x001C8DB8 File Offset: 0x001C6FB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRootActive(bool active)
	{
		this.Root.SetActive(active);
		if (active)
		{
			MeshRenderer[] componentsInChildren = this.Root.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
		}
	}

	// Token: 0x060048AC RID: 18604 RVA: 0x001C8DF8 File Offset: 0x001C6FF8
	public void UpdateAllFlags()
	{
		int i = 0;
		while (i < this.flagCounts.Length && this.flagCounts[i] <= 0)
		{
			i++;
		}
		bool flag = i < this.flagCounts.Length;
		this.SetRootActive(flag);
		this.Highlight.SetActive(this.highlightFlag >= 0);
		if (flag)
		{
			this.Highlight.SetActive(false);
			this.HelperFlag.SetActive(false);
			List<GameObject> list = new List<GameObject>();
			int num = -1;
			for (i = 0; i < this.flagCounts.Length; i++)
			{
				bool flag2 = this.flagCounts[i] > 0;
				GameObject gameObject = this.Flags[i];
				if (flag2)
				{
					if (i == this.highlightFlag)
					{
						num = list.Count;
					}
					list.Add(gameObject);
				}
				gameObject.SetActive(flag2);
			}
			int count = list.Count;
			for (i = 0; i < count; i++)
			{
				Transform transform = this.flagNodes[this.FlagParents[count - 1]][i].transform;
				list[i].transform.SetParent(transform, false);
				if (i == num)
				{
					this.Highlight.transform.SetParent(this.highlightNodes[this.FlagParents[count - 1]][i].transform, false);
					this.Highlight.SetActive(true);
				}
			}
		}
	}

	// Token: 0x060048AD RID: 18605 RVA: 0x001C8F54 File Offset: 0x001C7154
	public GameObject GetFlagParentNode(int flagCount, int flagIndex)
	{
		if (flagCount >= 0 && flagCount < this.FlagParents.Length)
		{
			List<GameObject> list = this.flagNodes[this.FlagParents[flagCount]];
			if (flagIndex >= 0 && flagIndex < list.Count)
			{
				return list[flagIndex];
			}
		}
		return null;
	}

	// Token: 0x04003A1A RID: 14874
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] Flags;

	// Token: 0x04003A1B RID: 14875
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] FlagParents;

	// Token: 0x04003A1C RID: 14876
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject Root;

	// Token: 0x04003A1D RID: 14877
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject HelperFlag;

	// Token: 0x04003A1E RID: 14878
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject Highlight;

	// Token: 0x04003A1F RID: 14879
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<GameObject, List<GameObject>> flagNodes = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04003A20 RID: 14880
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<GameObject, List<GameObject>> highlightNodes = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04003A21 RID: 14881
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] flagCounts;

	// Token: 0x04003A22 RID: 14882
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int highlightFlag = -1;
}
