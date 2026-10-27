using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004C5 RID: 1221
[Preserve]
public class EntityNewStyleAvatar : Entity
{
	// Token: 0x06002684 RID: 9860 RVA: 0x000EC2F0 File Offset: 0x000EA4F0
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

	// Token: 0x06002685 RID: 9861 RVA: 0x000EC370 File Offset: 0x000EA570
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

	// Token: 0x06002686 RID: 9862 RVA: 0x000EC435 File Offset: 0x000EA635
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x000EC43D File Offset: 0x000EA63D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x04001CA3 RID: 7331
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, EntityNewStyleAvatar.BodySlot> m_entitySlots = new Dictionary<string, EntityNewStyleAvatar.BodySlot>();

	// Token: 0x020004C6 RID: 1222
	[PublicizedFrom(EAccessModifier.Protected)]
	public class StringTags
	{
		// Token: 0x06002688 RID: 9864 RVA: 0x000EC445 File Offset: 0x000EA645
		public void AddTag(string tag)
		{
			this.tags.Add(tag);
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000EC454 File Offset: 0x000EA654
		public bool HasTag(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x04001CA4 RID: 7332
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> tags;
	}

	// Token: 0x020004C7 RID: 1223
	[PublicizedFrom(EAccessModifier.Protected)]
	public class BodySlot
	{
		// Token: 0x04001CA5 RID: 7333
		[PublicizedFrom(EAccessModifier.Private)]
		public string submeshName;

		// Token: 0x04001CA6 RID: 7334
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityNewStyleAvatar.StringTags tags;
	}
}
