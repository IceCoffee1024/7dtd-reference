using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001345 RID: 4933
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Rendering/Colorize")]
public class ImageEffectManager : MonoBehaviour
{
	// Token: 0x06009BD7 RID: 39895 RVA: 0x003AD1C4 File Offset: 0x003AB3C4
	public int GetNumEffects(string _effectGroupName)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (ImageEffectManager.staticEffectGroups == null)
		{
			return 0;
		}
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			return effectGroup.GetNumEffects();
		}
		return 0;
	}

	// Token: 0x06009BD8 RID: 39896 RVA: 0x003AD1F8 File Offset: 0x003AB3F8
	public string GetEffectName(string _effectGroupName, int _index)
	{
		if (_index <= 0)
		{
			return "Off";
		}
		ImageEffectManager.ValidateStaticClassEffects();
		if (ImageEffectManager.staticEffectGroups == null)
		{
			return "";
		}
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			return effectGroup.GetEffectName(_index);
		}
		return "";
	}

	// Token: 0x06009BD9 RID: 39897 RVA: 0x003AD240 File Offset: 0x003AB440
	public void DisableEffectGroup(string _effectGroupName)
	{
		if (this.enabledEffects == null)
		{
			return;
		}
		Dictionary<int, float> dictionary;
		if (this.enabledEffects.TryGetValue(_effectGroupName, out dictionary))
		{
			this.numEnabledEffects -= dictionary.Count;
			dictionary.Clear();
		}
	}

	// Token: 0x06009BDA RID: 39898 RVA: 0x003AD280 File Offset: 0x003AB480
	public bool SetEffect_Slow(string _effectGroupName, string _effectName, float _newIntensity = 1f)
	{
		if (!this.ValidateEffects())
		{
			return false;
		}
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			int num = 0;
			ImageEffectManager.Effect[] effects = effectGroup.effects;
			for (int i = 0; i < effects.Length; i++)
			{
				if (effects[i].name.Equals(_effectName))
				{
					return this.SetEffect(_effectGroupName, num, _newIntensity);
				}
				num++;
			}
		}
		return true;
	}

	// Token: 0x06009BDB RID: 39899 RVA: 0x003AD2DC File Offset: 0x003AB4DC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ValidateEffects()
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (ImageEffectManager.staticClass == null)
		{
			return false;
		}
		if (ImageEffectManager.staticEffectGroups == null)
		{
			return false;
		}
		if (this.enabledEffects == null)
		{
			this.enabledEffects = new Dictionary<string, Dictionary<int, float>>();
		}
		return true;
	}

	// Token: 0x06009BDC RID: 39900 RVA: 0x003AD310 File Offset: 0x003AB510
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetEffectIntenal(string _effectGroupName, int _index, float _newIntensity = 1f)
	{
		Dictionary<int, float> dictionary;
		if (this.enabledEffects.TryGetValue(_effectGroupName, out dictionary))
		{
			float num = 0f;
			if (dictionary.TryGetValue(_index, out num))
			{
				if (_newIntensity == 0f)
				{
					dictionary.Remove(_index);
					if (dictionary.Count == 0)
					{
						this.enabledEffects.Remove(_effectGroupName);
					}
					this.numEnabledEffects--;
					return;
				}
				dictionary[_index] = _newIntensity;
				return;
			}
			else if (_newIntensity > 0f)
			{
				dictionary.Add(_index, _newIntensity);
				this.numEnabledEffects++;
			}
		}
		else if (_newIntensity > 0f)
		{
			Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
			dictionary2.Add(_index, _newIntensity);
			this.enabledEffects.Add(_effectGroupName, dictionary2);
			this.numEnabledEffects++;
		}
		base.enabled = this.numEnabledEffects > 0;
	}

	// Token: 0x06009BDD RID: 39901 RVA: 0x003AD3DB File Offset: 0x003AB5DB
	public bool SetEffect(string _effectGroupName, float _newIntensity = 1f)
	{
		return this.SetEffect(_effectGroupName, 0, _newIntensity);
	}

	// Token: 0x06009BDE RID: 39902 RVA: 0x003AD3E8 File Offset: 0x003AB5E8
	public bool SetEffect(string _effectGroupName, string _effectName, float _newIntensity = 1f)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			int num = 0;
			ImageEffectManager.Effect[] effects = effectGroup.effects;
			for (int i = 0; i < effects.Length; i++)
			{
				if (effects[i].name.Equals(_effectName))
				{
					return this.SetEffect(_effectGroupName, num, _newIntensity);
				}
				num++;
			}
		}
		return false;
	}

	// Token: 0x06009BDF RID: 39903 RVA: 0x003AD43F File Offset: 0x003AB63F
	public bool SetEffect(string _effectGroupName, int _index, float _newIntensity = 1f)
	{
		if (_index < 0)
		{
			return false;
		}
		if (!this.ValidateEffects())
		{
			return false;
		}
		this.SetEffectIntenal(_effectGroupName, _index, _newIntensity);
		return true;
	}

	// Token: 0x06009BE0 RID: 39904 RVA: 0x003AD45B File Offset: 0x003AB65B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.staticClass == null;
	}

	// Token: 0x06009BE1 RID: 39905 RVA: 0x003AD470 File Offset: 0x003AB670
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateStaticClassEffects()
	{
		if (ImageEffectManager.validated)
		{
			return;
		}
		if (ImageEffectManager.staticGameObject == null)
		{
			ImageEffectManager.staticGameObject = Resources.Load<GameObject>("Prefabs/ImageEffectsPrefab");
			if (ImageEffectManager.staticGameObject != null)
			{
				ImageEffectManager.staticClass = ImageEffectManager.staticGameObject.GetComponent<ImageEffectManager>();
			}
		}
		if (ImageEffectManager.staticClass == null)
		{
			return;
		}
		if (ImageEffectManager.staticEffectGroups == null)
		{
			ImageEffectManager.staticEffectGroups = new Dictionary<string, ImageEffectManager.EffectGroup>();
		}
		if (ImageEffectManager.staticClass.effectGroups != null)
		{
			foreach (ImageEffectManager.EffectGroup effectGroup in ImageEffectManager.staticClass.effectGroups)
			{
				ImageEffectManager.staticEffectGroups.Add(effectGroup.name, effectGroup);
			}
		}
		ImageEffectManager.validated = true;
	}

	// Token: 0x06009BE2 RID: 39906 RVA: 0x003AD51C File Offset: 0x003AB71C
	public void SetFloat_Slow(string _effectGroup, string _effectName, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.EffectGroup effectGroup = null;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroup, out effectGroup))
		{
			for (int i = 0; i < effectGroup.effects.Length; i++)
			{
				if (effectGroup.effects[i].name.EqualsCaseInsensitive(_effectName))
				{
					this.SetFloat(effectGroup, i, _propertyName, _value);
					return;
				}
			}
		}
	}

	// Token: 0x06009BE3 RID: 39907 RVA: 0x003AD574 File Offset: 0x003AB774
	public void SetFloat(string _effectGroup, int _effectIndex, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (_effectIndex < 0)
		{
			return;
		}
		ImageEffectManager.EffectGroup effectGroup = null;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroup, out effectGroup))
		{
			this.SetFloat(effectGroup, _effectIndex, _propertyName, _value);
		}
	}

	// Token: 0x06009BE4 RID: 39908 RVA: 0x003AD5A7 File Offset: 0x003AB7A7
	public void SetFloat(ImageEffectManager.EffectGroup _effectGroup, int _effectIndex, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (_effectGroup.effects.Length > _effectIndex)
		{
			_effectGroup.effects[_effectIndex].SetFloat(_propertyName, _value);
		}
	}

	// Token: 0x06009BE5 RID: 39909 RVA: 0x003AD5CC File Offset: 0x003AB7CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (source == null)
		{
			return;
		}
		if (this.enabledEffects == null || this.enabledEffects.Count == 0)
		{
			base.enabled = false;
			return;
		}
		ImageEffectManager.ValidateStaticClassEffects();
		RenderTexture renderTexture = null;
		RenderTexture renderTexture2 = null;
		if (this.enabledEffects.Count > 1)
		{
			renderTexture = RenderTexture.GetTemporary(source.width, source.height);
		}
		if (this.enabledEffects.Count > 2)
		{
			renderTexture2 = RenderTexture.GetTemporary(source.width, source.height);
		}
		bool flag = true;
		int num = 0;
		RenderTexture renderTexture3 = source;
		foreach (KeyValuePair<string, Dictionary<int, float>> keyValuePair in this.enabledEffects)
		{
			foreach (KeyValuePair<int, float> keyValuePair2 in keyValuePair.Value)
			{
				if (ImageEffectManager.staticEffectGroups.ContainsKey(keyValuePair.Key))
				{
					ImageEffectManager.Effect effect = ImageEffectManager.staticEffectGroups[keyValuePair.Key].effects[keyValuePair2.Key];
					if (!(effect.material == null))
					{
						effect.GetMaterial().SetFloat("Intensity", Mathf.Clamp01(keyValuePair2.Value));
						Material material = effect.GetMaterial();
						BlendMode blendMode = BlendMode.OneMinusDstAlpha;
						bool flag2 = false;
						if (effect.hasProperty.TryGetValue("BlendSrc", out flag2) && flag2)
						{
							blendMode = (BlendMode)material.GetInt("BlendSrc");
						}
						effect.UpdateMaterial();
						if (renderTexture == null)
						{
							Graphics.Blit(renderTexture3, destination, effect.GetMaterial());
						}
						else if (flag)
						{
							if (blendMode == BlendMode.Zero)
							{
								Graphics.Blit(renderTexture3, renderTexture, effect.GetMaterial());
								renderTexture3 = renderTexture;
							}
							else
							{
								Graphics.Blit(renderTexture3, renderTexture3, effect.GetMaterial());
							}
							flag = false;
						}
						else if (num == this.numEnabledEffects - 1)
						{
							Graphics.Blit(renderTexture3, destination, effect.GetMaterial());
						}
						else if (blendMode == BlendMode.Zero)
						{
							RenderTexture renderTexture4 = ((renderTexture3 == renderTexture) ? renderTexture2 : renderTexture);
							Graphics.Blit(renderTexture3, renderTexture4, effect.GetMaterial());
							renderTexture3 = renderTexture4;
						}
						else
						{
							Graphics.Blit(renderTexture3, renderTexture3, effect.GetMaterial());
						}
						num++;
					}
				}
			}
		}
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		if (renderTexture2 != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture2);
		}
	}

	// Token: 0x04007592 RID: 30098
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject staticGameObject;

	// Token: 0x04007593 RID: 30099
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ImageEffectManager staticClass;

	// Token: 0x04007594 RID: 30100
	public static Dictionary<string, ImageEffectManager.EffectGroup> staticEffectGroups;

	// Token: 0x04007595 RID: 30101
	public ImageEffectManager.EffectGroup[] effectGroups;

	// Token: 0x04007596 RID: 30102
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, Dictionary<int, float>> enabledEffects;

	// Token: 0x04007597 RID: 30103
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numEnabledEffects;

	// Token: 0x04007598 RID: 30104
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool validated;

	// Token: 0x02001346 RID: 4934
	[Serializable]
	public class Effect
	{
		// Token: 0x06009BE7 RID: 39911 RVA: 0x003AD85C File Offset: 0x003ABA5C
		public Effect()
		{
			this.instantiatedMtrl = null;
			this.hasProperty = new Dictionary<string, bool>();
			this.floatPropertyUpdates = new Dictionary<string, float>();
		}

		// Token: 0x06009BE8 RID: 39912 RVA: 0x003AD888 File Offset: 0x003ABA88
		public void UpdateMaterial()
		{
			if (this.material == null)
			{
				return;
			}
			foreach (KeyValuePair<string, float> keyValuePair in this.floatPropertyUpdates)
			{
				if (this.needToCheckMaterial)
				{
					if (this.GetMaterial().HasProperty(keyValuePair.Key))
					{
						this.GetMaterial().SetFloat(keyValuePair.Key, keyValuePair.Value);
					}
				}
				else
				{
					this.GetMaterial().SetFloat(keyValuePair.Key, keyValuePair.Value);
				}
			}
			this.floatPropertyUpdates.Clear();
			this.needToCheckMaterial = false;
		}

		// Token: 0x06009BE9 RID: 39913 RVA: 0x003AD948 File Offset: 0x003ABB48
		public void SetFloat(string _propertyName, float _value)
		{
			bool flag = false;
			if (this.hasProperty.TryGetValue(_propertyName, out flag))
			{
				if (flag)
				{
					float num = 0f;
					if (!this.floatPropertyUpdates.TryGetValue(_propertyName, out num))
					{
						this.floatPropertyUpdates.Add(_propertyName, _value);
						return;
					}
					this.floatPropertyUpdates[_propertyName] = _value;
					return;
				}
			}
			else if (this.material != null)
			{
				bool flag2 = this.GetMaterial().HasProperty(_propertyName);
				this.hasProperty.Add(_propertyName, flag2);
				if (flag2)
				{
					this.floatPropertyUpdates.Add(_propertyName, _value);
					return;
				}
			}
			else
			{
				this.needToCheckMaterial = true;
				float num2 = 0f;
				if (!this.floatPropertyUpdates.TryGetValue(_propertyName, out num2))
				{
					this.floatPropertyUpdates.Add(_propertyName, _value);
					return;
				}
				this.floatPropertyUpdates[_propertyName] = _value;
			}
		}

		// Token: 0x06009BEA RID: 39914 RVA: 0x003ADA10 File Offset: 0x003ABC10
		public Material GetMaterial()
		{
			if (this.instantiatedMtrl == null)
			{
				this.instantiatedMtrl = global::UnityEngine.Object.Instantiate<Material>(this.material);
			}
			return this.instantiatedMtrl;
		}

		// Token: 0x04007599 RID: 30105
		public string name;

		// Token: 0x0400759A RID: 30106
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool needToCheckMaterial = true;

		// Token: 0x0400759B RID: 30107
		public Material material;

		// Token: 0x0400759C RID: 30108
		public Material instantiatedMtrl;

		// Token: 0x0400759D RID: 30109
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public GameObject materialHolder;

		// Token: 0x0400759E RID: 30110
		public Dictionary<string, bool> hasProperty;

		// Token: 0x0400759F RID: 30111
		public Dictionary<string, float> floatPropertyUpdates;
	}

	// Token: 0x02001347 RID: 4935
	[Serializable]
	public class EffectGroup
	{
		// Token: 0x06009BEB RID: 39915 RVA: 0x003ADA37 File Offset: 0x003ABC37
		public int GetNumEffects()
		{
			if (this.effects == null)
			{
				return 0;
			}
			return this.effects.Length;
		}

		// Token: 0x06009BEC RID: 39916 RVA: 0x003ADA4B File Offset: 0x003ABC4B
		public string GetEffectName(int _index)
		{
			if (this.effects == null)
			{
				return "";
			}
			if (_index < 0)
			{
				return "";
			}
			if (_index >= this.effects.Length)
			{
				return "";
			}
			return this.effects[_index].name;
		}

		// Token: 0x040075A0 RID: 30112
		public string name;

		// Token: 0x040075A1 RID: 30113
		public ImageEffectManager.Effect[] effects;
	}
}
