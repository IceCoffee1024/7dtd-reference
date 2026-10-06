using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004C3 RID: 1219
[Preserve]
public class EntityNewStyleAvatar : Entity
{
	// Token: 0x0600267D RID: 9853 RVA: 0x000EBEE0 File Offset: 0x000EA0E0
	public void EnableSubmesh(string submeshName, bool enable)
	{
		Transform transform = base.transform;
		Transform transform2 = transform.Find("Graphics/Model");
		if (transform2 == null)
		{
			transform2 = transform;
		}
		Transform transform3 = transform2.Find("base");
		if (transform3 != null)
		{
			int childCount = transform3.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject = transform3.GetChild(i).gameObject;
				if (gameObject.name == submeshName)
				{
					gameObject.SetActive(enable);
				}
			}
		}
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x000EBF60 File Offset: 0x000EA160
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		Transform transform = base.transform;
		Transform transform2 = transform.Find("Graphics/Model");
		if (transform2 == null)
		{
			transform2 = transform;
		}
		Transform transform3 = null;
		if (transform2 != null)
		{
			transform3 = DataLoader.LoadAsset<Transform>("@:Entities/Player/Male/maleTestPrefab.prefab", false);
			if (transform3 != null)
			{
				transform3 = global::UnityEngine.Object.Instantiate<Transform>(transform3, transform2);
				transform3.name = "base";
			}
		}
		if (transform3)
		{
			int childCount = transform3.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform3.GetChild(i);
				Renderer component = child.GetComponent<Renderer>();
				if (!(component == null) && component.sharedMaterials != null)
				{
					child.gameObject.SetActive(false);
				}
			}
		}
		base.gameObject.AddComponent<NewAvatarRootMotion>();
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x000EC025 File Offset: 0x000EA225
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002680 RID: 9856 RVA: 0x000EC02D File Offset: 0x000EA22D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x04001C93 RID: 7315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, EntityNewStyleAvatar.BodySlot> m_entitySlots = new Dictionary<string, EntityNewStyleAvatar.BodySlot>();

	// Token: 0x020004C4 RID: 1220
	[PublicizedFrom(EAccessModifier.Protected)]
	public class StringTags
	{
		// Token: 0x06002681 RID: 9857 RVA: 0x000EC035 File Offset: 0x000EA235
		public void AddTag(string tag)
		{
			this.tags.Add(tag);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000EC044 File Offset: 0x000EA244
		public bool HasTag(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x04001C94 RID: 7316
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> tags;
	}

	// Token: 0x020004C5 RID: 1221
	[PublicizedFrom(EAccessModifier.Protected)]
	public class BodySlot
	{
		// Token: 0x04001C95 RID: 7317
		[PublicizedFrom(EAccessModifier.Private)]
		public string submeshName;

		// Token: 0x04001C96 RID: 7318
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityNewStyleAvatar.StringTags tags;
	}
}
