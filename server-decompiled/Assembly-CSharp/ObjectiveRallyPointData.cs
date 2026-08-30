using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009A6 RID: 2470
public class ObjectiveRallyPointData : MonoBehaviour
{
	// Token: 0x060048CB RID: 18635 RVA: 0x001C715C File Offset: 0x001C535C
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

	// Token: 0x060048CC RID: 18636 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x001C72C8 File Offset: 0x001C54C8
	public void ClearAllFlags()
	{
		for (int i = 0; i < this.Flags.Length; i++)
		{
			this.flagCounts[i] = 0;
		}
		this.highlightFlag = -1;
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x001C72F8 File Offset: 0x001C54F8
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

	// Token: 0x060048CF RID: 18639 RVA: 0x001C7348 File Offset: 0x001C5548
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

	// Token: 0x060048D0 RID: 18640 RVA: 0x001C739C File Offset: 0x001C559C
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

	// Token: 0x060048D1 RID: 18641 RVA: 0x001C73DC File Offset: 0x001C55DC
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

	// Token: 0x060048D2 RID: 18642 RVA: 0x001C7538 File Offset: 0x001C5738
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

	// Token: 0x040039FD RID: 14845
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] Flags;

	// Token: 0x040039FE RID: 14846
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] FlagParents;

	// Token: 0x040039FF RID: 14847
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject Root;

	// Token: 0x04003A00 RID: 14848
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject HelperFlag;

	// Token: 0x04003A01 RID: 14849
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject Highlight;

	// Token: 0x04003A02 RID: 14850
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<GameObject, List<GameObject>> flagNodes = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04003A03 RID: 14851
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<GameObject, List<GameObject>> highlightNodes = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x04003A04 RID: 14852
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] flagCounts;

	// Token: 0x04003A05 RID: 14853
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int highlightFlag = -1;
}
