using System;
using System.Xml.Linq;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006DF RID: 1759
[Preserve]
public class MinEventActionAttachParticleEffectToEntity : MinEventActionTargetedBase
{
	// Token: 0x06003683 RID: 13955 RVA: 0x00167124 File Offset: 0x00165324
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		Transform transform = null;
		float num = 0f;
		bool flag = this.setShapeMesh;
		if (_params.Tags.Test_AnySet(this.usePassedInTransformTag))
		{
			transform = _params.Transform;
		}
		else if (this.parent_transform_path == null)
		{
			transform = _params.Self.emodel.meshTransform;
		}
		else if (this.parent_transform_path == "LOD0")
		{
			transform = _params.Self.emodel.meshTransform;
		}
		else if (this.parent_transform_path == ".item")
		{
			Transform rightHandTransform = _params.Self.emodel.GetRightHandTransform();
			if (rightHandTransform && rightHandTransform.childCount > 0)
			{
				transform = rightHandTransform.GetChild(0);
			}
		}
		else if (this.parent_transform_path == ".body")
		{
			EModelSDCS emodelSDCS = _params.Self.emodel as EModelSDCS;
			if (emodelSDCS != null)
			{
				Transform transform2 = _params.Self.transform.Find("Graphics/Model");
				if (this.setShapeMesh && !emodelSDCS.IsFPV)
				{
					transform = GameUtils.FindDeepChildActive(transform2, "body");
					if (transform == null)
					{
						transform = GameUtils.FindDeepChildActive(transform2, "torso");
					}
					if (transform == null)
					{
						transform = GameUtils.FindDeepChild(transform2, "body");
					}
				}
				else
				{
					transform = GameUtils.FindDeepChild(transform2, "Spine1");
					flag = false;
				}
			}
			else
			{
				transform = _params.Self.emodel.GetPelvisTransform();
				if ((_params.Self.entityFlags & EntityFlags.Animal) > EntityFlags.None)
				{
					this.local_rotation.x = this.local_rotation.x + 90f;
					num = 1f;
				}
			}
		}
		else if (this.parent_transform_path == ".head")
		{
			transform = _params.Self.emodel.GetHeadTransform();
		}
		else
		{
			Transform transform3 = GameUtils.FindDeepChild(_params.Self.transform, this.parent_transform_path);
			if (transform3)
			{
				Transform parent = transform3.parent;
				if (!parent || !this.setShapeMesh || !parent.gameObject.CompareTag("Item"))
				{
					transform = transform3;
				}
			}
		}
		if (!transform)
		{
			return;
		}
		string text = "Ptl_" + this.goToInstantiate.name;
		Transform transform4 = transform.Find(text);
		if (!transform4)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.goToInstantiate);
			if (!gameObject)
			{
				return;
			}
			transform4 = gameObject.transform;
			gameObject.name = text;
			Utils.SetLayerRecursively(gameObject, transform.gameObject.layer);
			transform4.SetParent(transform, false);
			transform4.SetLocalPositionAndRotation(this.local_offset, Quaternion.Euler(this.local_rotation.x, this.local_rotation.y, this.local_rotation.z));
			if (num > 0f)
			{
				transform4.localScale = Vector3.one * num;
			}
			if (!this.isOneshot)
			{
				_params.Self.AddParticle(text, transform4);
			}
			AudioPlayer component = transform4.GetComponent<AudioPlayer>();
			if (component)
			{
				component.duration = 100000f;
			}
			ParticleSystem[] componentsInChildren = transform4.GetComponentsInChildren<ParticleSystem>();
			if (componentsInChildren != null)
			{
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
					ParticleSystem.ShapeModule shape = particleSystem.shape;
					ParticleSystemShapeType shapeType = shape.shapeType;
					if (shapeType == ParticleSystemShapeType.SkinnedMeshRenderer || shapeType == ParticleSystemShapeType.Mesh)
					{
						SkinnedMeshRenderer componentInChildren = transform.GetComponentInChildren<SkinnedMeshRenderer>();
						if (componentInChildren && flag)
						{
							shape.skinnedMeshRenderer = componentInChildren;
						}
						else
						{
							MeshRenderer componentInChildren2 = transform.GetComponentInChildren<MeshRenderer>();
							if (componentInChildren2 && flag)
							{
								shape.meshRenderer = componentInChildren2;
								shape.shapeType = ParticleSystemShapeType.MeshRenderer;
							}
							else
							{
								shape.shapeType = ParticleSystemShapeType.Sphere;
								if (flag)
								{
									Log.Warning("AttachParticleEffectToEntity {0}, {1} no renderer!", new object[] { _params.Self, text });
								}
							}
						}
					}
					if (flag)
					{
						EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
						if (entityPlayerLocal && entityPlayerLocal.bFirstPersonView)
						{
							shape.position += new Vector3(0f, 0f, 0.3f);
						}
					}
					if (!this.isOneshot)
					{
						particleSystem.main.duration = 900000f;
					}
					particleSystem.Play();
				}
			}
		}
		if (this.soundName != null)
		{
			EntityPlayerLocal entityPlayerLocal2 = _params.Self as EntityPlayerLocal;
			if (entityPlayerLocal2)
			{
				Manager.PlayInsidePlayerHead(this.soundName, entityPlayerLocal2.entityId, 0f, false, false);
			}
		}
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x001675BB File Offset: 0x001657BB
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.goToInstantiate != null;
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x001675E4 File Offset: 0x001657E4
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
			if (num <= 1166642238U)
			{
				if (num != 235771284U)
				{
					if (num != 355251678U)
					{
						if (num == 1166642238U)
						{
							if (localName == "parent_transform")
							{
								this.parent_transform_path = _attribute.Value;
								return true;
							}
						}
					}
					else if (localName == "shape_mesh")
					{
						this.setShapeMesh = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
						return true;
					}
				}
				else if (localName == "sound")
				{
					this.soundName = _attribute.Value;
					return true;
				}
			}
			else if (num <= 1616204026U)
			{
				if (num != 1570167343U)
				{
					if (num == 1616204026U)
					{
						if (localName == "local_offset")
						{
							this.local_offset = StringParsers.ParseVector3(_attribute.Value, 0, -1);
							return true;
						}
					}
				}
				else if (localName == "local_rotation")
				{
					this.local_rotation = StringParsers.ParseVector3(_attribute.Value, 0, -1);
					return true;
				}
			}
			else if (num != 1859092719U)
			{
				if (num == 2324120511U)
				{
					if (localName == "particle")
					{
						this.goToInstantiate = LoadManager.LoadAssetFromAddressables<GameObject>("ParticleEffects/" + _attribute.Value + ".prefab", null, null, false, true, false).Asset;
						return true;
					}
				}
			}
			else if (localName == "oneshot")
			{
				this.isOneshot = true;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002BCD RID: 11213
	public const string cParticlePrefix = "Ptl_";

	// Token: 0x04002BCE RID: 11214
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goToInstantiate;

	// Token: 0x04002BCF RID: 11215
	[PublicizedFrom(EAccessModifier.Private)]
	public string parent_transform_path;

	// Token: 0x04002BD0 RID: 11216
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_offset;

	// Token: 0x04002BD1 RID: 11217
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 local_rotation;

	// Token: 0x04002BD2 RID: 11218
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> usePassedInTransformTag = FastTags<TagGroup.Global>.Parse("usePassedInTransform");

	// Token: 0x04002BD3 RID: 11219
	[PublicizedFrom(EAccessModifier.Private)]
	public bool setShapeMesh;

	// Token: 0x04002BD4 RID: 11220
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x04002BD5 RID: 11221
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOneshot;
}
