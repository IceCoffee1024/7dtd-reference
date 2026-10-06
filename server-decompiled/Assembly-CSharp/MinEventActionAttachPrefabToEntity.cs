using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006E5 RID: 1765
[Preserve]
public class MinEventActionAttachPrefabToEntity : MinEventActionTargetedBase
{
	// Token: 0x06003697 RID: 13975 RVA: 0x00167D8C File Offset: 0x00165F8C
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		Transform transform = _params.Self.RootTransform;
		if (this.parent_transform_path != null && this.parent_transform_path != "")
		{
			transform = GameUtils.FindDeepChildActive(_params.Self.RootTransform, this.parent_transform_path);
		}
		if (transform == null)
		{
			return;
		}
		string text = string.Format("tempPrefab_" + this.goToInstantiate.name, Array.Empty<object>());
		Transform transform2 = GameUtils.FindDeepChild(transform, text);
		if (transform2 == null)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.goToInstantiate);
			if (gameObject == null)
			{
				return;
			}
			transform2 = gameObject.transform;
			gameObject.name = text;
			Utils.SetLayerRecursively(gameObject, transform.gameObject.layer);
			transform2.parent = transform;
			transform2.localPosition = this.local_offset;
			transform2.localRotation = Quaternion.Euler(this.local_rotation.x, this.local_rotation.y, this.local_rotation.z);
			transform2.localScale = this.local_scale;
		}
	}

	// Token: 0x06003698 RID: 13976 RVA: 0x00167EA3 File Offset: 0x001660A3
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.goToInstantiate != null;
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x00167ECC File Offset: 0x001660CC
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "prefab")
			{
				this.goToInstantiate = DataLoader.LoadAsset<GameObject>(_attribute.Value, false);
				return true;
			}
			if (localName == "parent_transform")
			{
				this.parent_transform_path = _attribute.Value;
				return true;
			}
			if (localName == "local_offset")
			{
				this.local_offset = StringParsers.ParseVector3(_attribute.Value, 0, -1);
				return true;
			}
			if (localName == "local_rotation")
			{
				this.local_rotation = StringParsers.ParseVector3(_attribute.Value, 0, -1);
				return true;
			}
			if (localName == "local_scale")
			{
				this.local_scale = StringParsers.ParseVector3(_attribute.Value, 0, -1);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002BDF RID: 11231
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goToInstantiate;

	// Token: 0x04002BE0 RID: 11232
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefab;

	// Token: 0x04002BE1 RID: 11233
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;

	// Token: 0x04002BE2 RID: 11234
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_offset = new Vector3(0f, 0f, 0f);

	// Token: 0x04002BE3 RID: 11235
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_rotation = new Vector3(0f, 0f, 0f);

	// Token: 0x04002BE4 RID: 11236
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_scale = new Vector3(1f, 1f, 1f);
}
