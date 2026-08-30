using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CAB RID: 3243
[Preserve]
public class VehiclePart
{
	// Token: 0x060063B5 RID: 25525 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void InitPrefabConnections()
	{
	}

	// Token: 0x060063B6 RID: 25526 RVA: 0x00271FA4 File Offset: 0x002701A4
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

	// Token: 0x060063B7 RID: 25527 RVA: 0x00272087 File Offset: 0x00270287
	public virtual void SetProperties(DynamicProperties _properties)
	{
		if (_properties == null)
		{
			Log.Warning("VehiclePart SetProperties null");
		}
		this.properties = _properties;
	}

	// Token: 0x060063B8 RID: 25528 RVA: 0x0027209D File Offset: 0x0027029D
	public string GetProperty(string _key)
	{
		if (this.properties == null)
		{
			Log.Warning("VehiclePart GetProperty null");
			return string.Empty;
		}
		return this.properties.GetString(_key);
	}

	// Token: 0x060063B9 RID: 25529 RVA: 0x002720C4 File Offset: 0x002702C4
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

	// Token: 0x060063BA RID: 25530 RVA: 0x00272190 File Offset: 0x00270390
	public void SetVehicle(Vehicle _v)
	{
		this.vehicle = _v;
	}

	// Token: 0x060063BB RID: 25531 RVA: 0x00272199 File Offset: 0x00270399
	public void SetTag(string _tag)
	{
		this.tag = _tag;
	}

	// Token: 0x060063BC RID: 25532 RVA: 0x002721A2 File Offset: 0x002703A2
	public Transform GetTransform()
	{
		return this.GetTransform("transform");
	}

	// Token: 0x060063BD RID: 25533 RVA: 0x002721B0 File Offset: 0x002703B0
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

	// Token: 0x060063BE RID: 25534 RVA: 0x002721EC File Offset: 0x002703EC
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

	// Token: 0x060063BF RID: 25535 RVA: 0x00272250 File Offset: 0x00270450
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

	// Token: 0x060063C0 RID: 25536 RVA: 0x002722B8 File Offset: 0x002704B8
	public virtual bool IsBroken()
	{
		return this.vehicle.GetHealth() <= 0;
	}

	// Token: 0x060063C1 RID: 25537 RVA: 0x002722CB File Offset: 0x002704CB
	public float GetHealthPercentage()
	{
		return this.vehicle.GetHealthPercent();
	}

	// Token: 0x060063C2 RID: 25538 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsRequired()
	{
		return false;
	}

	// Token: 0x060063C3 RID: 25539 RVA: 0x002722D8 File Offset: 0x002704D8
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

	// Token: 0x060063C4 RID: 25540 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update(float _dt)
	{
	}

	// Token: 0x060063C5 RID: 25541 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(Vehicle.Event _event, float _arg)
	{
	}

	// Token: 0x060063C6 RID: 25542 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleEvent(VehiclePart.Event _event, VehiclePart _fromPart, float arg)
	{
	}

	// Token: 0x04004D8C RID: 19852
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicProperties properties;

	// Token: 0x04004D8D RID: 19853
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vehicle vehicle;

	// Token: 0x04004D8E RID: 19854
	public string tag;

	// Token: 0x04004D8F RID: 19855
	public bool modInstalled;

	// Token: 0x04004D90 RID: 19856
	public List<IKController.Target> ikTargets;

	// Token: 0x04004D91 RID: 19857
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Renderer> renderers = new List<Renderer>();

	// Token: 0x04004D92 RID: 19858
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Material> materials = new List<Material>();

	// Token: 0x02000CAC RID: 3244
	public enum Event
	{
		// Token: 0x04004D94 RID: 19860
		Broken,
		// Token: 0x04004D95 RID: 19861
		LightsOn,
		// Token: 0x04004D96 RID: 19862
		FuelEmpty,
		// Token: 0x04004D97 RID: 19863
		FuelRemove
	}
}
