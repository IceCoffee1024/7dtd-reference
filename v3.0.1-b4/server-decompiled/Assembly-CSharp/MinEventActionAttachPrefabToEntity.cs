using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006D9 RID: 1753
[Preserve]
public class MinEventActionAttachPrefabToEntity : MinEventActionTargetedBase
{
	// Token: 0x060035FE RID: 13822 RVA: 0x00165250 File Offset: 0x00163450
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

	// Token: 0x060035FF RID: 13823 RVA: 0x00165367 File Offset: 0x00163567
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.goToInstantiate != null;
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x00165390 File Offset: 0x00163590
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

	// Token: 0x04002B72 RID: 11122
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goToInstantiate;

	// Token: 0x04002B73 RID: 11123
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefab;

	// Token: 0x04002B74 RID: 11124
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;

	// Token: 0x04002B75 RID: 11125
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_offset = new Vector3(0f, 0f, 0f);

	// Token: 0x04002B76 RID: 11126
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_rotation = new Vector3(0f, 0f, 0f);

	// Token: 0x04002B77 RID: 11127
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_scale = new Vector3(1f, 1f, 1f);
}
