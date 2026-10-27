using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200070C RID: 1804
[Preserve]
public class MinEventActionAttachPrefabToHeldItem : MinEventActionBase
{
	// Token: 0x06003724 RID: 14116 RVA: 0x0016B1CC File Offset: 0x001693CC
	public override void Execute(MinEventParams _params)
	{
		Transform transform = null;
		if (this.parent_transform != "")
		{
			transform = GameUtils.FindDeepChild(_params.Transform, this.parent_transform);
		}
		else if (_params.Transform != null)
		{
			transform = _params.Transform;
		}
		else if (_params.Self != null)
		{
			transform = GameUtils.FindDeepChildActive(_params.Self.RootTransform, "InactiveItems");
		}
		if (transform == null)
		{
			return;
		}
		string propertyOverride = _params.ItemValue.GetPropertyOverride(this.itemPropertyName, "");
		if (this.goToInstantiate == null && propertyOverride == "")
		{
			return;
		}
		if (propertyOverride != "")
		{
			this.goToInstantiate = DataLoader.LoadAsset<GameObject>(propertyOverride, false);
		}
		string text = string.Format("tempMod_" + this.goToInstantiate.name, Array.Empty<object>());
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
			Utils.SetLayerRecursively(gameObject, transform.gameObject.layer, Utils.ExcludeLayerZoom);
			transform2.parent = transform;
			transform2.localPosition = this.local_offset;
			transform2.localRotation = Quaternion.Euler(this.local_rotation.x, this.local_rotation.y, this.local_rotation.z);
		}
		if (transform2 != null)
		{
			UpdateLightOnAllMaterials updateLightOnAllMaterials = transform2.gameObject.AddMissingComponent<UpdateLightOnAllMaterials>();
			updateLightOnAllMaterials.SetTintColorForItem(Vector3.one);
			if (_params.ItemValue.ItemClass.Properties.Values.ContainsKey(Block.PropTintColor))
			{
				updateLightOnAllMaterials.SetTintColorForItem(Block.StringToVector3(_params.ItemValue.GetPropertyOverride(Block.PropTintColor, _params.ItemValue.ItemClass.Properties.Values[Block.PropTintColor])));
				return;
			}
			updateLightOnAllMaterials.SetTintColorForItem(Block.StringToVector3(_params.ItemValue.GetPropertyOverride(Block.PropTintColor, "255,255,255")));
		}
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x0016B3E8 File Offset: 0x001695E8
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (_params.Self != null || _params.Transform != null) && (this.goToInstantiate != null || this.itemPropertyName != null);
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x0016B438 File Offset: 0x00169638
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "prefab")
			{
				this.itemPropertyName = _attribute.Value;
				if (!this.itemPropertyName.StartsWith("property?"))
				{
					this.goToInstantiate = DataLoader.LoadAsset<GameObject>(_attribute.Value, false);
				}
				else
				{
					this.itemPropertyName = this.itemPropertyName.Replace("property?", "");
				}
				return true;
			}
			if (localName == "parent_transform")
			{
				this.parent_transform = _attribute.Value;
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
		}
		return flag;
	}

	// Token: 0x04002C4C RID: 11340
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goToInstantiate;

	// Token: 0x04002C4D RID: 11341
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemPropertyName;

	// Token: 0x04002C4E RID: 11342
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefab;

	// Token: 0x04002C4F RID: 11343
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C50 RID: 11344
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_offset = new Vector3(0f, 0f, 0f);

	// Token: 0x04002C51 RID: 11345
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_rotation = new Vector3(0f, 0f, 0f);
}
