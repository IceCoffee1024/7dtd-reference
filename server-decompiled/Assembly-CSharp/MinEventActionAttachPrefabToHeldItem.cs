using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000702 RID: 1794
[Preserve]
public class MinEventActionAttachPrefabToHeldItem : MinEventActionBase
{
	// Token: 0x060036CA RID: 14026 RVA: 0x001698D0 File Offset: 0x00167AD0
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

	// Token: 0x060036CB RID: 14027 RVA: 0x00169AEC File Offset: 0x00167CEC
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (_params.Self != null || _params.Transform != null) && (this.goToInstantiate != null || this.itemPropertyName != null);
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00169B3C File Offset: 0x00167D3C
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

	// Token: 0x04002C37 RID: 11319
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goToInstantiate;

	// Token: 0x04002C38 RID: 11320
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemPropertyName;

	// Token: 0x04002C39 RID: 11321
	[PublicizedFrom(EAccessModifier.Private)]
	public string prefab;

	// Token: 0x04002C3A RID: 11322
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform = "";

	// Token: 0x04002C3B RID: 11323
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_offset = new Vector3(0f, 0f, 0f);

	// Token: 0x04002C3C RID: 11324
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_rotation = new Vector3(0f, 0f, 0f);
}
