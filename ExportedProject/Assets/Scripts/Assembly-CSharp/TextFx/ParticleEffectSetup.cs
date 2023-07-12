using System;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class ParticleEffectSetup : EffectItemSetup
	{
		public PARTICLE_EFFECT_TYPE m_effect_type;

		public ParticleSystem m_shuriken_particle_effect;

		public ActionFloatProgression m_duration = new ActionFloatProgression(0f);

		public bool m_follow_mesh;

		public ActionVector3Progression m_position_offset = new ActionVector3Progression(Vector3.zero);

		public ActionVector3Progression m_rotation_offset = new ActionVector3Progression(Vector3.zero);

		public bool m_rotate_relative_to_letter = true;

		public tfxJSONValue ExportData()
		{
			tfxJSONObject json_data = new tfxJSONObject();
			ExportBaseData(ref json_data);
			json_data["m_effect_type"] = (double)m_effect_type;
			json_data["m_shuriken_particle_effect"] = m_shuriken_particle_effect.ToPath();
			json_data["m_duration"] = m_duration.ExportData();
			json_data["m_follow_mesh"] = m_follow_mesh;
			json_data["m_position_offset"] = m_position_offset.ExportData();
			json_data["m_rotation_offset"] = m_rotation_offset.ExportData();
			json_data["m_rotate_relative_to_letter"] = m_rotate_relative_to_letter;
			return new tfxJSONValue(json_data);
		}

		public void ImportData(tfxJSONObject json_data, string assetNameSuffix = "")
		{
			m_effect_type = (PARTICLE_EFFECT_TYPE)json_data["m_effect_type"].Number;
			m_shuriken_particle_effect = json_data["m_shuriken_particle_effect"].Str.PathToParticleSystem(assetNameSuffix);
			m_duration.ImportData(json_data["m_duration"].Obj);
			m_follow_mesh = json_data["m_follow_mesh"].Boolean;
			m_position_offset.ImportData(json_data["m_position_offset"].Obj);
			m_rotation_offset.ImportData(json_data["m_rotation_offset"].Obj);
			m_rotate_relative_to_letter = json_data["m_rotate_relative_to_letter"].Boolean;
			ImportBaseData(json_data);
		}
	}
}
