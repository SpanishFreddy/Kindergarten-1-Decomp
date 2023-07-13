using System;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class AudioEffectSetup : EffectItemSetup
	{
		public AudioClip m_audio_clip;

		public ActionFloatProgression m_offset_time = new ActionFloatProgression(0f);

		public ActionFloatProgression m_volume = new ActionFloatProgression(1f);

		public ActionFloatProgression m_pitch = new ActionFloatProgression(1f);

		public tfxJSONValue ExportData()
		{
			tfxJSONObject json_data = new tfxJSONObject();
			ExportBaseData(ref json_data);
			json_data["m_audio_clip"] = m_audio_clip.ToPath();
			json_data["m_offset_time"] = m_offset_time.ExportData();
			json_data["m_volume"] = m_volume.ExportData();
			json_data["m_pitch"] = m_pitch.ExportData();
			return new tfxJSONValue(json_data);
		}

		public void ImportData(tfxJSONObject json_data)
		{
			m_audio_clip = json_data["m_audio_clip"].Str.PathToAudioClip();
			m_offset_time.ImportData(json_data["m_offset_time"].Obj);
			m_volume.ImportData(json_data["m_volume"].Obj);
			m_pitch.ImportData(json_data["m_pitch"].Obj);
			ImportBaseData(json_data);
		}
	}
}
