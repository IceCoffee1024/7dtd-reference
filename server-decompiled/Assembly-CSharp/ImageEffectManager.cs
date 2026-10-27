using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001377 RID: 4983
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Rendering/Colorize")]
public class ImageEffectManager : MonoBehaviour
{
	// Token: 0x06009CDB RID: 40155 RVA: 0x003B4144 File Offset: 0x003B2344
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

	// Token: 0x06009CDC RID: 40156 RVA: 0x003B4178 File Offset: 0x003B2378
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

	// Token: 0x06009CDD RID: 40157 RVA: 0x003B41C0 File Offset: 0x003B23C0
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

	// Token: 0x06009CDE RID: 40158 RVA: 0x003B4200 File Offset: 0x003B2400
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

	// Token: 0x06009CDF RID: 40159 RVA: 0x003B425C File Offset: 0x003B245C
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

	// Token: 0x06009CE0 RID: 40160 RVA: 0x003B4290 File Offset: 0x003B2490
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

	// Token: 0x06009CE1 RID: 40161 RVA: 0x003B435B File Offset: 0x003B255B
	public bool SetEffect(string _effectGroupName, float _newIntensity = 1f)
	{
		return this.SetEffect(_effectGroupName, 0, _newIntensity);
	}

	// Token: 0x06009CE2 RID: 40162 RVA: 0x003B4368 File Offset: 0x003B2568
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

	// Token: 0x06009CE3 RID: 40163 RVA: 0x003B43BF File Offset: 0x003B25BF
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

	// Token: 0x06009CE4 RID: 40164 RVA: 0x003B43DB File Offset: 0x003B25DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.staticClass == null;
	}

	// Token: 0x06009CE5 RID: 40165 RVA: 0x003B43F0 File Offset: 0x003B25F0
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

	// Token: 0x06009CE6 RID: 40166 RVA: 0x003B449C File Offset: 0x003B269C
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

	// Token: 0x06009CE7 RID: 40167 RVA: 0x003B44F4 File Offset: 0x003B26F4
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

	// Token: 0x06009CE8 RID: 40168 RVA: 0x003B4527 File Offset: 0x003B2727
	public void SetFloat(ImageEffectManager.EffectGroup _effectGroup, int _effectIndex, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (_effectGroup.effects.Length > _effectIndex)
		{
			_effectGroup.effects[_effectIndex].SetFloat(_propertyName, _value);
		}
	}

	// Token: 0x06009CE9 RID: 40169 RVA: 0x003B454C File Offset: 0x003B274C
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

	// Token: 0x0400764E RID: 30286
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject staticGameObject;

	// Token: 0x0400764F RID: 30287
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ImageEffectManager staticClass;

	// Token: 0x04007650 RID: 30288
	public static Dictionary<string, ImageEffectManager.EffectGroup> staticEffectGroups;

	// Token: 0x04007651 RID: 30289
	public ImageEffectManager.EffectGroup[] effectGroups;

	// Token: 0x04007652 RID: 30290
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, Dictionary<int, float>> enabledEffects;

	// Token: 0x04007653 RID: 30291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numEnabledEffects;

	// Token: 0x04007654 RID: 30292
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool validated;

	// Token: 0x02001378 RID: 4984
	[Serializable]
	public class Effect
	{
		// Token: 0x06009CEB RID: 40171 RVA: 0x003B47DC File Offset: 0x003B29DC
		public Effect()
		{
			this.instantiatedMtrl = null;
			this.hasProperty = new Dictionary<string, bool>();
			this.floatPropertyUpdates = new Dictionary<string, float>();
		}

		// Token: 0x06009CEC RID: 40172 RVA: 0x003B4808 File Offset: 0x003B2A08
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

		// Token: 0x06009CED RID: 40173 RVA: 0x003B48C8 File Offset: 0x003B2AC8
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

		// Token: 0x06009CEE RID: 40174 RVA: 0x003B4990 File Offset: 0x003B2B90
		public Material GetMaterial()
		{
			if (this.instantiatedMtrl == null)
			{
				this.instantiatedMtrl = global::UnityEngine.Object.Instantiate<Material>(this.material);
			}
			return this.instantiatedMtrl;
		}

		// Token: 0x04007655 RID: 30293
		public string name;

		// Token: 0x04007656 RID: 30294
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool needToCheckMaterial = true;

		// Token: 0x04007657 RID: 30295
		public Material material;

		// Token: 0x04007658 RID: 30296
		public Material instantiatedMtrl;

		// Token: 0x04007659 RID: 30297
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public GameObject materialHolder;

		// Token: 0x0400765A RID: 30298
		public Dictionary<string, bool> hasProperty;

		// Token: 0x0400765B RID: 30299
		public Dictionary<string, float> floatPropertyUpdates;
	}

	// Token: 0x02001379 RID: 4985
	[Serializable]
	public class EffectGroup
	{
		// Token: 0x06009CEF RID: 40175 RVA: 0x003B49B7 File Offset: 0x003B2BB7
		public int GetNumEffects()
		{
			if (this.effects == null)
			{
				return 0;
			}
			return this.effects.Length;
		}

		// Token: 0x06009CF0 RID: 40176 RVA: 0x003B49CB File Offset: 0x003B2BCB
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

		// Token: 0x0400765C RID: 30300
		public string name;

		// Token: 0x0400765D RID: 30301
		public ImageEffectManager.Effect[] effects;
	}
}
