using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CB9 RID: 3257
[Preserve]
public class VehiclePart
{
	// Token: 0x060063A8 RID: 25512 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitPrefabConnections()
	{
	}

	// Token: 0x060063A9 RID: 25513 RVA: 0x002753A8 File Offset: 0x002735A8
	public void InitIKTarget(AvatarIKGoal ikGoal, Transform parentT)
	{
		string text = IKController.IKNames[(int)ikGoal];
		string property = this.GetProperty(text + "Position");
		if (string.IsNullOrEmpty(property))
		{
			return;
		}
		Vector3 vector = StringParsers.ParseVector3(property, 0, -1);
		Vector3 vector2 = StringParsers.ParseVector3(this.GetProperty(text + "Rotation"), 0, -1);
		IKController.Target target;
		target.avatarGoal = ikGoal;
		target.transform = null;
		if (parentT)
		{
			Transform transform = new GameObject(text).transform;
			target.transform = transform;
			transform.SetParent(parentT, false);
			transform.localPosition = vector;
			transform.localEulerAngles = vector2;
			target.position = Vector3.zero;
			target.rotation = Vector3.zero;
		}
		else
		{
			target.position = vector;
			target.rotation = vector2;
		}
		if (this.ikTargets == null)
		{
			this.ikTargets = new List<IKController.Target>();
		}
		this.ikTargets.Add(target);
	}

	// Token: 0x060063AA RID: 25514 RVA: 0x0027548B File Offset: 0x0027368B
	public virtual void SetProperties(DynamicProperties _properties)
	{
		if (_properties == null)
		{
			Log.Warning("VehiclePart SetProperties null");
		}
		this.properties = _properties;
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x002754A1 File Offset: 0x002736A1
	public string GetProperty(string _key)
	{
		if (this.properties == null)
		{
			Log.Warning("VehiclePart GetProperty null");
			return string.Empty;
		}
		return this.properties.GetString(_key);
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x002754C8 File Offset: 0x002736C8
	public virtual void SetMods()
	{
		this.modInstalled = false;
		string property = this.GetProperty("mod");
		if (property.Length > 0)
		{
			int bit = FastTags<TagGroup.Global>.GetBit(property);
			this.modInstalled = this.vehicle.ModTags.Test_Bit(bit);
			Transform transform = this.GetTransform("modT");
			if (transform)
			{
				string property2 = this.GetProperty("modRot");
				if (property2.Length > 0)
				{
					Vector3 vector = Vector3.zero;
					if (this.modInstalled)
					{
						vector = StringParsers.ParseVector3(property2, 0, -1);
					}
					transform.localEulerAngles = vector;
				}
				else
				{
					transform.gameObject.SetActive(this.modInstalled);
				}
			}
			this.SetTransformActive("modHideT", !this.modInstalled);
			this.SetPhysicsTransformActive("modRBT", this.modInstalled);
		}
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x00275594 File Offset: 0x00273794
	public void SetVehicle(Vehicle _v)
	{
		this.vehicle = _v;
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x0027559D File Offset: 0x0027379D
	public void SetTag(string _tag)
	{
		this.tag = _tag;
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x002755A6 File Offset: 0x002737A6
	public Transform GetTransform()
	{
		return this.GetTransform("transform");
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x002755B4 File Offset: 0x002737B4
	public Transform GetTransform(string _property)
	{
		Transform meshTransform = this.vehicle.GetMeshTransform();
		if (meshTransform)
		{
			string property = this.GetProperty(_property);
			if (property.Length > 0)
			{
				return meshTransform.Find(property);
			}
		}
		return null;
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x002755F0 File Offset: 0x002737F0
	public void SetTransformActive(string _property, bool _active)
	{
		Transform transform = this.vehicle.GetMeshTransform();
		if (transform)
		{
			string property = this.GetProperty(_property);
			if (property.Length > 0)
			{
				transform = transform.Find(property);
				if (transform)
				{
					transform.gameObject.SetActive(_active);
					return;
				}
				Log.Warning("Vehicle SetTransformActive missing {0}", new object[] { property });
			}
		}
	}

	// Token: 0x060063B2 RID: 25522 RVA: 0x00275654 File Offset: 0x00273854
	public void SetPhysicsTransformActive(string _property, bool _active)
	{
		Transform transform = this.vehicle.entity.PhysicsTransform;
		if (transform)
		{
			string property = this.GetProperty(_property);
			if (property.Length > 0)
			{
				transform = transform.Find(property);
				if (transform)
				{
					transform.gameObject.SetActive(_active);
					return;
				}
				Log.Warning("Vehicle SetPhysicsTransformActive missing {0}", new object[] { property });
			}
		}
	}

	// Token: 0x060063B3 RID: 25523 RVA: 0x002756BC File Offset: 0x002738BC
	public virtual bool IsBroken()
	{
		return this.vehicle.GetHealth() <= 0;
	}

	// Token: 0x060063B4 RID: 25524 RVA: 0x002756CF File Offset: 0x002738CF
	public float GetHealthPercentage()
	{
		return this.vehicle.GetHealthPercent();
	}

	// Token: 0x060063B5 RID: 25525 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsRequired()
	{
		return false;
	}

	// Token: 0x060063B6 RID: 25526 RVA: 0x002756DC File Offset: 0x002738DC
	public void SetColors(Color _color)
	{
		Transform transform = this.GetTransform("paint");
		if (transform)
		{
			transform.GetComponentsInChildren<Renderer>(true, VehiclePart.renderers);
			if (VehiclePart.renderers.Count > 0)
			{
				Material material = VehiclePart.renderers[0].material;
				this.vehicle.mainEmissiveMat = material;
				material.color = _color;
				for (int i = 1; i < VehiclePart.renderers.Count; i++)
				{
					Renderer renderer = VehiclePart.renderers[i];
					if (renderer.CompareTag("LOD"))
					{
						renderer.GetSharedMaterials(VehiclePart.materials);
						if (VehiclePart.materials.Count == 1)
						{
							renderer.material = material;
						}
						VehiclePart.materials.Clear();
					}
				}
				VehiclePart.renderers.Clear();
			}
		}
	}

	// Token: 0x060063B7 RID: 25527 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update(float _dt)
	{
	}

	// Token: 0x060063B8 RID: 25528 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(Vehicle.Event _event, float _arg)
	{
	}

	// Token: 0x060063B9 RID: 25529 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(VehiclePart.Event _event, VehiclePart _fromPart, float arg)
	{
	}

	// Token: 0x04004DC2 RID: 19906
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicProperties properties;

	// Token: 0x04004DC3 RID: 19907
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vehicle vehicle;

	// Token: 0x04004DC4 RID: 19908
	public string tag;

	// Token: 0x04004DC5 RID: 19909
	public bool modInstalled;

	// Token: 0x04004DC6 RID: 19910
	public List<IKController.Target> ikTargets;

	// Token: 0x04004DC7 RID: 19911
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Renderer> renderers = new List<Renderer>();

	// Token: 0x04004DC8 RID: 19912
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Material> materials = new List<Material>();

	// Token: 0x02000CBA RID: 3258
	public enum Event
	{
		// Token: 0x04004DCA RID: 19914
		Broken,
		// Token: 0x04004DCB RID: 19915
		LightsOn,
		// Token: 0x04004DCC RID: 19916
		FuelEmpty,
		// Token: 0x04004DCD RID: 19917
		FuelRemove
	}
}
