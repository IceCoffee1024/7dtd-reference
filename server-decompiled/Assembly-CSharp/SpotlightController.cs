using System;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class SpotlightController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x000B5D7D File Offset: 0x000B3F7D
	// (set) Token: 0x06001DD3 RID: 7635 RVA: 0x000B5D85 File Offset: 0x000B3F85
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x06001DD4 RID: 7636 RVA: 0x000B5D90 File Offset: 0x000B3F90
	public void Init(DynamicProperties _properties)
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		if (this.Cone != null)
		{
			MeshRenderer component = this.Cone.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					this.ConeMaterial = component.material;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
				else if (component.sharedMaterial != null)
				{
					this.ConeMaterial = component.sharedMaterial;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
			}
			this.Cone.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000B5E48 File Offset: 0x000B4048
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.TileEntity == null)
		{
			return;
		}
		if (this.TileEntity.IsPowered && this.UserAccessingId == -1)
		{
			if (this.TileEntity.IsPowered)
			{
				if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
				{
					this.YawController.Yaw = Mathf.Lerp(this.YawController.Yaw, this.TileEntity.CenteredYaw, Time.deltaTime * this.degreesPerSecond);
					this.YawController.UpdateYaw();
				}
				if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
				{
					this.PitchController.Pitch = Mathf.Lerp(this.PitchController.Pitch, this.TileEntity.CenteredPitch, Time.deltaTime * this.degreesPerSecond);
					this.PitchController.UpdatePitch();
				}
			}
			this.IsOn &= this.TileEntity.IsPowered;
			if (this.LightScript.bSwitchedOn != this.IsOn)
			{
				this.UpdateEmissionColor(this.IsOn);
				this.LightScript.bSwitchedOn = this.IsOn;
			}
			return;
		}
		if (this.UserAccessingId != -1)
		{
			this.YawController.Yaw = this.TileEntity.CenteredYaw;
			this.YawController.UpdateYaw();
			this.PitchController.Pitch = this.TileEntity.CenteredPitch;
			this.PitchController.UpdatePitch();
			return;
		}
		if (!this.TileEntity.IsPowered)
		{
			if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
			{
				this.YawController.Yaw = this.TileEntity.CenteredYaw;
				this.YawController.SetYaw();
			}
			if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
			{
				this.PitchController.Pitch = this.TileEntity.CenteredPitch;
				this.PitchController.SetPitch();
			}
		}
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x000B6048 File Offset: 0x000B4248
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEmissionColor(bool isPowered)
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].material != componentsInChildren[i].sharedMaterial)
				{
					componentsInChildren[i].material = new Material(componentsInChildren[i].sharedMaterial);
				}
				if (isPowered)
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.white);
				}
				else
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.black);
				}
				componentsInChildren[i].sharedMaterial = componentsInChildren[i].material;
			}
		}
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000B60E1 File Offset: 0x000B42E1
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000B60EF File Offset: 0x000B42EF
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000B60FD File Offset: 0x000B42FD
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000B610A File Offset: 0x000B430A
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x000200DA File Offset: 0x0001E2DA
	public Transform GetCameraTransform()
	{
		return null;
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000B6117 File Offset: 0x000B4317
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x000B6120 File Offset: 0x000B4320
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000B6141 File Offset: 0x000B4341
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x000B6149 File Offset: 0x000B4349
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000B616A File Offset: 0x000B436A
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000B618C File Offset: 0x000B438C
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool HasLaser()
	{
		return false;
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000AF5A9 File Offset: 0x000AD7A9
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserActive(bool _active)
	{
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool GetLaserActive()
	{
		return false;
	}

	// Token: 0x040013EA RID: 5098
	public AutoTurretYawLerp YawController;

	// Token: 0x040013EB RID: 5099
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040013EC RID: 5100
	public LightLOD LightScript;

	// Token: 0x040013ED RID: 5101
	public Transform Cone;

	// Token: 0x040013EE RID: 5102
	public Material ConeMaterial;

	// Token: 0x040013EF RID: 5103
	public Color ConeColor;

	// Token: 0x040013F0 RID: 5104
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x040013F1 RID: 5105
	public bool IsOn;

	// Token: 0x040013F2 RID: 5106
	public TileEntityPowered TileEntity;

	// Token: 0x040013F4 RID: 5108
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialized;
}
