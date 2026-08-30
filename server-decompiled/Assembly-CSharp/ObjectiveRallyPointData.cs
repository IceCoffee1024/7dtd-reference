using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000995 RID: 2453
public class ObjectiveRallyPointData : MonoBehaviour
{
	// Token: 0x06004845 RID: 18501 RVA: 0x001C4968 File Offset: 0x001C2B68
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

	// Token: 0x06004846 RID: 18502 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x001C4AD4 File Offset: 0x001C2CD4
	public void ClearAllFlags()
	{
		for (int i = 0; i < this.Flags.Length; i++)
		{
			this.flagCounts[i] = 0;
		}
		this.highlightFlag = -1;
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x001C4B04 File Offset: 0x001C2D04
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

	// Token: 0x06004849 RID: 18505 RVA: 0x001C4B54 File Offset: 0x001C2D54
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

	// Token: 0x0600484A RID: 18506 RVA: 0x001C4BA8 File Offset: 0x001C2DA8
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

	// Token: 0x0600484B RID: 18507 RVA: 0x001C4BE8 File Offset: 0x001C2DE8
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

	// Token: 0x0600484C RID: 18508 RVA: 0x001C4D44 File Offset: 0x001C2F44
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

	// Token: 0x0400396C RID: 14700
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] Flags;

	// Token: 0x0400396D RID: 14701
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] FlagParents;

	// Token: 0x0400396E RID: 14702
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject Root;

	// Token: 0x0400396F RID: 14703
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject HelperFlag;

	// Token: 0x04003970 RID: 14704
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject Highlight;

	// Token: 0x04003971 RID: 14705
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<GameObject, List<GameObject>> flagNodes = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04003972 RID: 14706
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<GameObject, List<GameObject>> highlightNodes = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04003973 RID: 14707
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] flagCounts;

	// Token: 0x04003974 RID: 14708
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int highlightFlag = -1;
}
