using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CC4 RID: 3268
[Preserve]
public class VehiclePart
{
	// Token: 0x060063CD RID: 25549 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitPrefabConnections()
	{
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x002768D0 File Offset: 0x00274AD0
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

	// Token: 0x060063CF RID: 25551 RVA: 0x002769B3 File Offset: 0x00274BB3
	public virtual void SetProperties(DynamicProperties _properties)
	{
		if (_properties == null)
		{
			Log.Warning("VehiclePart SetProperties null");
		}
		this.properties = _properties;
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x002769C9 File Offset: 0x00274BC9
	public string GetProperty(string _key)
	{
		if (this.properties == null)
		{
			Log.Warning("VehiclePart GetProperty null");
			return string.Empty;
		}
		return this.properties.GetString(_key);
	}

	// Token: 0x060063D1 RID: 25553 RVA: 0x002769F0 File Offset: 0x00274BF0
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

	// Token: 0x060063D2 RID: 25554 RVA: 0x00276ABC File Offset: 0x00274CBC
	public void SetVehicle(Vehicle _v)
	{
		this.vehicle = _v;
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x00276AC5 File Offset: 0x00274CC5
	public void SetTag(string _tag)
	{
		this.tag = _tag;
	}

	// Token: 0x060063D4 RID: 25556 RVA: 0x00276ACE File Offset: 0x00274CCE
	public Transform GetTransform()
	{
		return this.GetTransform("transform");
	}

	// Token: 0x060063D5 RID: 25557 RVA: 0x00276ADC File Offset: 0x00274CDC
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

	// Token: 0x060063D6 RID: 25558 RVA: 0x00276B18 File Offset: 0x00274D18
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

	// Token: 0x060063D7 RID: 25559 RVA: 0x00276B7C File Offset: 0x00274D7C
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

	// Token: 0x060063D8 RID: 25560 RVA: 0x00276BE4 File Offset: 0x00274DE4
	public virtual bool IsBroken()
	{
		return this.vehicle.GetHealth() <= 0;
	}

	// Token: 0x060063D9 RID: 25561 RVA: 0x00276BF7 File Offset: 0x00274DF7
	public float GetHealthPercentage()
	{
		return this.vehicle.GetHealthPercent();
	}

	// Token: 0x060063DA RID: 25562 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsRequired()
	{
		return false;
	}

	// Token: 0x060063DB RID: 25563 RVA: 0x00276C04 File Offset: 0x00274E04
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

	// Token: 0x060063DC RID: 25564 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update(float _dt)
	{
	}

	// Token: 0x060063DD RID: 25565 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(Vehicle.Event _event, float _arg)
	{
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(VehiclePart.Event _event, VehiclePart _fromPart, float arg)
	{
	}

	// Token: 0x04004DE7 RID: 19943
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicProperties properties;

	// Token: 0x04004DE8 RID: 19944
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vehicle vehicle;

	// Token: 0x04004DE9 RID: 19945
	public string tag;

	// Token: 0x04004DEA RID: 19946
	public bool modInstalled;

	// Token: 0x04004DEB RID: 19947
	public List<IKController.Target> ikTargets;

	// Token: 0x04004DEC RID: 19948
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Renderer> renderers = new List<Renderer>();

	// Token: 0x04004DED RID: 19949
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Material> materials = new List<Material>();

	// Token: 0x02000CC5 RID: 3269
	public enum Event
	{
		// Token: 0x04004DEF RID: 19951
		Broken,
		// Token: 0x04004DF0 RID: 19952
		LightsOn,
		// Token: 0x04004DF1 RID: 19953
		FuelEmpty,
		// Token: 0x04004DF2 RID: 19954
		FuelRemove
	}
}
