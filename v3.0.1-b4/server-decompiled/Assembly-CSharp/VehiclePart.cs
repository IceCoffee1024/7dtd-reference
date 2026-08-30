using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C99 RID: 3225
[Preserve]
public class VehiclePart
{
	// Token: 0x06006329 RID: 25385 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitPrefabConnections()
	{
	}

	// Token: 0x0600632A RID: 25386 RVA: 0x0026F0A8 File Offset: 0x0026D2A8
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

	// Token: 0x0600632B RID: 25387 RVA: 0x0026F18B File Offset: 0x0026D38B
	public virtual void SetProperties(DynamicProperties _properties)
	{
		if (_properties == null)
		{
			Log.Warning("VehiclePart SetProperties null");
		}
		this.properties = _properties;
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x0026F1A1 File Offset: 0x0026D3A1
	public string GetProperty(string _key)
	{
		if (this.properties == null)
		{
			Log.Warning("VehiclePart GetProperty null");
			return string.Empty;
		}
		return this.properties.GetString(_key);
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x0026F1C8 File Offset: 0x0026D3C8
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

	// Token: 0x0600632E RID: 25390 RVA: 0x0026F294 File Offset: 0x0026D494
	public void SetVehicle(Vehicle _v)
	{
		this.vehicle = _v;
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x0026F29D File Offset: 0x0026D49D
	public void SetTag(string _tag)
	{
		this.tag = _tag;
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x0026F2A6 File Offset: 0x0026D4A6
	public Transform GetTransform()
	{
		return this.GetTransform("transform");
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x0026F2B4 File Offset: 0x0026D4B4
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

	// Token: 0x06006332 RID: 25394 RVA: 0x0026F2F0 File Offset: 0x0026D4F0
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

	// Token: 0x06006333 RID: 25395 RVA: 0x0026F354 File Offset: 0x0026D554
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

	// Token: 0x06006334 RID: 25396 RVA: 0x0026F3BC File Offset: 0x0026D5BC
	public virtual bool IsBroken()
	{
		return this.vehicle.GetHealth() <= 0;
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x0026F3CF File Offset: 0x0026D5CF
	public float GetHealthPercentage()
	{
		return this.vehicle.GetHealthPercent();
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsRequired()
	{
		return false;
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x0026F3DC File Offset: 0x0026D5DC
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

	// Token: 0x06006338 RID: 25400 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update(float _dt)
	{
	}

	// Token: 0x06006339 RID: 25401 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(Vehicle.Event _event, float _arg)
	{
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(VehiclePart.Event _event, VehiclePart _fromPart, float arg)
	{
	}

	// Token: 0x04004CEE RID: 19694
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicProperties properties;

	// Token: 0x04004CEF RID: 19695
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vehicle vehicle;

	// Token: 0x04004CF0 RID: 19696
	public string tag;

	// Token: 0x04004CF1 RID: 19697
	public bool modInstalled;

	// Token: 0x04004CF2 RID: 19698
	public List<IKController.Target> ikTargets;

	// Token: 0x04004CF3 RID: 19699
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Renderer> renderers = new List<Renderer>();

	// Token: 0x04004CF4 RID: 19700
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Material> materials = new List<Material>();

	// Token: 0x02000C9A RID: 3226
	public enum Event
	{
		// Token: 0x04004CF6 RID: 19702
		Broken,
		// Token: 0x04004CF7 RID: 19703
		LightsOn,
		// Token: 0x04004CF8 RID: 19704
		FuelEmpty,
		// Token: 0x04004CF9 RID: 19705
		FuelRemove
	}
}
