using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004B9 RID: 1209
[Preserve]
public class EntityNewStyleAvatar : Entity
{
	// Token: 0x06002665 RID: 9829 RVA: 0x000EB194 File Offset: 0x000E9394
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

	// Token: 0x06002666 RID: 9830 RVA: 0x000EB214 File Offset: 0x000E9414
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

	// Token: 0x06002667 RID: 9831 RVA: 0x000EB2D9 File Offset: 0x000E94D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x000EB2E1 File Offset: 0x000E94E1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x04001C74 RID: 7284
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, EntityNewStyleAvatar.BodySlot> m_entitySlots = new Dictionary<string, EntityNewStyleAvatar.BodySlot>();

	// Token: 0x020004BA RID: 1210
	[PublicizedFrom(EAccessModifier.Protected)]
	public class StringTags
	{
		// Token: 0x06002669 RID: 9833 RVA: 0x000EB2E9 File Offset: 0x000E94E9
		public void AddTag(string tag)
		{
			this.tags.Add(tag);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000EB2F8 File Offset: 0x000E94F8
		public bool HasTag(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x04001C75 RID: 7285
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> tags;
	}

	// Token: 0x020004BB RID: 1211
	[PublicizedFrom(EAccessModifier.Protected)]
	public class BodySlot
	{
		// Token: 0x04001C76 RID: 7286
		[PublicizedFrom(EAccessModifier.Private)]
		public string submeshName;

		// Token: 0x04001C77 RID: 7287
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityNewStyleAvatar.StringTags tags;
	}
}
