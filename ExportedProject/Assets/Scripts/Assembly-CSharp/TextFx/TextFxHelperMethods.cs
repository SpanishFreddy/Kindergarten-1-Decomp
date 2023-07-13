using System.Collections.Generic;
using System.Text.RegularExpressions;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	public static class TextFxHelperMethods
	{
		public static string StripRichTextCode(string textString)
		{
			return Regex.Replace(textString, "<[^>]*>", string.Empty);
		}

		public static tfxJSONArray ExportData(this List<int> list)
		{
			tfxJSONArray tfxJSONArray = new tfxJSONArray();
			if (list != null)
			{
				foreach (int item in list)
				{
					tfxJSONArray.Add(item);
				}
				return tfxJSONArray;
			}
			return tfxJSONArray;
		}

		public static List<int> JSONtoListInt(this tfxJSONArray json_array)
		{
			List<int> list = new List<int>();
			foreach (tfxJSONValue item in json_array)
			{
				list.Add((int)item.Number);
			}
			return list;
		}

		public static List<object> StringToList(this string data_string, char delimiter = ',', char seperator = '=')
		{
			data_string = data_string.Substring(1, data_string.Length - 2);
			if (data_string.Equals(string.Empty))
			{
				return null;
			}
			List<object> list = new List<object>();
			if (data_string.Contains(string.Empty + seperator))
			{
				string[] array = data_string.Split(delimiter);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(seperator);
					list.Add(new KeyValuePair<string, string>(array3[0], array3[1]));
				}
			}
			else
			{
				list = new List<object>(data_string.Split(delimiter));
			}
			return list;
		}

		public static string Vector2ToString(this Vector2 vec, char delimiter = ';', char seperator = ':')
		{
			return "x" + seperator + vec.x + delimiter + "y" + seperator + vec.y;
		}

		public static tfxJSONValue Vector2ToJSON(this Vector2 vec)
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["x"] = vec.x;
			tfxJSONObject["y"] = vec.y;
			return new tfxJSONValue(tfxJSONObject);
		}

		public static Vector2 StringToVector2(this string data_string, char delimiter = ';', char seperator = ':')
		{
			string[] array = data_string.Split(delimiter);
			Vector2 result = default(Vector2);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(seperator);
				if (array3[0].Equals("x"))
				{
					result.x = float.Parse(array3[1]);
				}
				else if (array3[0].Equals("y"))
				{
					result.y = float.Parse(array3[1]);
				}
			}
			return result;
		}

		public static Vector2 JSONtoVector2(this tfxJSONObject json_data)
		{
			Vector2 result = default(Vector2);
			result.x = (float)json_data["x"].Number;
			result.y = (float)json_data["y"].Number;
			return result;
		}

		public static tfxJSONValue ExportData(this Vector3 vec)
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["x"] = vec.x;
			tfxJSONObject["y"] = vec.y;
			tfxJSONObject["z"] = vec.z;
			return new tfxJSONValue(tfxJSONObject);
		}

		public static Vector3 JSONtoVector3(this tfxJSONObject json_data)
		{
			Vector3 result = default(Vector3);
			result.x = (float)json_data["x"].Number;
			result.y = (float)json_data["y"].Number;
			result.z = (float)json_data["z"].Number;
			return result;
		}

		public static Vector3 StringToVector3(this string data_string, char delimiter = ';', char seperator = ':')
		{
			string[] array = data_string.Split(delimiter);
			Vector3 result = default(Vector3);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(seperator);
				if (array3[0].Equals("x"))
				{
					result.x = float.Parse(array3[1]);
				}
				else if (array3[0].Equals("y"))
				{
					result.y = float.Parse(array3[1]);
				}
				else if (array3[0].Equals("z"))
				{
					result.z = float.Parse(array3[1]);
				}
			}
			return result;
		}

		public static tfxJSONValue ExportData(this Color color)
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["r"] = color.r;
			tfxJSONObject["g"] = color.g;
			tfxJSONObject["b"] = color.b;
			tfxJSONObject["a"] = color.a;
			return new tfxJSONValue(tfxJSONObject);
		}

		public static Color JSONtoColor(this tfxJSONObject json_data)
		{
			Color result = default(Color);
			result.r = (float)json_data["r"].Number;
			result.g = (float)json_data["g"].Number;
			result.b = (float)json_data["b"].Number;
			result.a = (float)json_data["a"].Number;
			return result;
		}

		public static Color StringToColor(this string data_string, char delimiter = ';', char seperator = ':')
		{
			return StringDataToColor(data_string, delimiter, seperator);
		}

		public static tfxJSONValue ExportData(this VertexColour vert_color)
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["bottom_left"] = vert_color.bottom_left.ExportData();
			tfxJSONObject["bottom_right"] = vert_color.bottom_right.ExportData();
			tfxJSONObject["top_left"] = vert_color.top_left.ExportData();
			tfxJSONObject["top_right"] = vert_color.top_right.ExportData();
			return new tfxJSONValue(tfxJSONObject);
		}

		public static VertexColour JSONtoVertexColour(this tfxJSONObject json_data)
		{
			if (json_data.ContainsKey("r"))
			{
				return new VertexColour(json_data.JSONtoColor());
			}
			VertexColour vertexColour = new VertexColour();
			vertexColour.bottom_left = json_data["bottom_left"].Obj.JSONtoColor();
			vertexColour.bottom_right = json_data["bottom_right"].Obj.JSONtoColor();
			vertexColour.top_left = json_data["top_left"].Obj.JSONtoColor();
			vertexColour.top_right = json_data["top_right"].Obj.JSONtoColor();
			return vertexColour;
		}

		public static VertexColour StringToVertexColor(this string data_string, char delimiter = ';', char seperator = ':', char color_seperator = '|')
		{
			string[] array = data_string.Split(color_seperator);
			VertexColour vertexColour = new VertexColour();
			vertexColour.bottom_left = StringDataToColor(array[0], delimiter, seperator);
			vertexColour.bottom_right = StringDataToColor(array[1], delimiter, seperator);
			vertexColour.top_left = StringDataToColor(array[2], delimiter, seperator);
			vertexColour.top_right = StringDataToColor(array[3], delimiter, seperator);
			return vertexColour;
		}

		private static Color StringDataToColor(string data_string, char delimiter = ';', char seperator = ':')
		{
			string[] array = data_string.Split(delimiter);
			Color result = default(Color);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(seperator);
				if (array3[0].Equals("r"))
				{
					result.r = float.Parse(array3[1]);
				}
				else if (array3[0].Equals("g"))
				{
					result.g = float.Parse(array3[1]);
				}
				else if (array3[0].Equals("b"))
				{
					result.b = float.Parse(array3[1]);
				}
				else if (array3[0].Equals("a"))
				{
					result.a = float.Parse(array3[1]);
				}
			}
			return result;
		}

		public static tfxJSONValue ExportData(this Keyframe frame)
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["inTangent"] = frame.inTangent;
			tfxJSONObject["outTangent"] = frame.outTangent;
			tfxJSONObject["tangentMode"] = frame.tangentMode;
			tfxJSONObject["time"] = frame.time;
			tfxJSONObject["value"] = frame.value;
			return new tfxJSONValue(tfxJSONObject);
		}

		public static Keyframe JSONtoKeyframe(this tfxJSONObject json_data)
		{
			Keyframe result = default(Keyframe);
			result.inTangent = (float)json_data["inTangent"].Number;
			result.outTangent = (float)json_data["outTangent"].Number;
			result.tangentMode = (int)json_data["tangentMode"].Number;
			result.time = (float)json_data["time"].Number;
			result.value = (float)json_data["value"].Number;
			return result;
		}

		public static tfxJSONValue ExportData(this AnimationCurve curve)
		{
			tfxJSONArray tfxJSONArray = new tfxJSONArray();
			Keyframe[] keys = curve.keys;
			foreach (Keyframe frame in keys)
			{
				tfxJSONArray.Add(frame.ExportData());
			}
			return tfxJSONArray;
		}

		public static AnimationCurve JSONtoAnimationCurve(this tfxJSONArray json_data)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.keys = new Keyframe[0];
			foreach (tfxJSONValue json_datum in json_data)
			{
				animationCurve.AddKey(json_datum.Obj.JSONtoKeyframe());
			}
			return animationCurve;
		}

		public static AnimationCurve ToAnimationCurve(this string curve_data)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			string[] array = curve_data.Split('#');
			if (array.Length % 5 != 0)
			{
				return animationCurve;
			}
			int num = 0;
			Keyframe key = default(Keyframe);
			for (num = 0; num < array.Length; num++)
			{
				if (num % 5 == 0)
				{
					if (num > 0)
					{
						animationCurve.AddKey(key);
					}
					key = default(Keyframe);
					key.time = float.Parse(array[num]);
				}
				if (num % 5 == 1)
				{
					key.value = float.Parse(array[num]);
				}
				if (num % 5 == 2)
				{
					key.inTangent = float.Parse(array[num]);
				}
				if (num % 5 == 3)
				{
					key.outTangent = float.Parse(array[num]);
				}
				if (num % 5 == 4)
				{
					key.tangentMode = int.Parse(array[num]);
				}
			}
			if (num > 0)
			{
				animationCurve.AddKey(key);
			}
			return animationCurve;
		}

		public static string ToPath(this AudioClip clip)
		{
			return string.Empty;
		}

		public static AudioClip PathToAudioClip(this string path)
		{
			string empty = string.Empty;
			int num = path.LastIndexOf('/');
			if (num >= 0)
			{
				empty = path.Substring(num + 1);
				int num2 = empty.LastIndexOf('.');
				if (num2 >= 0)
				{
					empty = empty.Substring(0, num2);
					return Resources.Load<AudioClip>("TextFx/" + empty);
				}
			}
			return null;
		}

		public static string ToPath(this ParticleSystem p_system)
		{
			return string.Empty;
		}

		public static ParticleSystem PathToParticleSystem(this string path, string assetNameSuffix = "")
		{
			return null;
		}

		public static string[] GetArrayOfFirstEntries(this string[,] two_d_array)
		{
			string[] array = new string[two_d_array.GetLength(0)];
			for (int i = 0; i < two_d_array.GetLength(0); i++)
			{
				array[i] = two_d_array[i, 0];
			}
			return array;
		}

		public static void ImportLegacyData(this TextFxAnimationManager effect_manager, string data)
		{
			List<object> list = data.StringToList();
			int num = 0;
			effect_manager.m_master_animations = new List<LetterAnimation>();
			for (int i = 0; i < list.Count; i++)
			{
				KeyValuePair<string, string> keyValuePair = (KeyValuePair<string, string>)list[i];
				string key = keyValuePair.Key;
				string value = keyValuePair.Value;
				switch (key)
				{
				case "m_animate_per":
					effect_manager.m_animate_per = (AnimatePerOptions)int.Parse(value);
					break;
				case "m_begin_delay":
					effect_manager.m_begin_delay = float.Parse(value);
					break;
				case "m_begin_on_start":
					effect_manager.m_begin_on_start = bool.Parse(value);
					break;
				case "m_on_finish_action":
					effect_manager.m_on_finish_action = (ON_FINISH_ACTION)int.Parse(value);
					break;
				case "m_time_type":
					effect_manager.m_time_type = (AnimationTime)int.Parse(value);
					break;
				case "ANIM_DATA_START":
					if (num == effect_manager.NumAnimations)
					{
						effect_manager.AddAnimation();
					}
					i = effect_manager.GetAnimation(num).ImportLegacyData(list, i + 1);
					num++;
					break;
				}
			}
		}

		public static int ImportLegacyData(this LetterAnimation letter_anim, List<object> data_list, int index_offset = 0)
		{
			int num = 0;
			int num2 = 0;
			int i;
			for (i = index_offset; i < data_list.Count; i++)
			{
				KeyValuePair<string, string> keyValuePair = (KeyValuePair<string, string>)data_list[i];
				string key = keyValuePair.Key;
				string value = keyValuePair.Value;
				if (key.Equals("ANIM_DATA_END"))
				{
					break;
				}
				switch (key)
				{
				case "m_letters_to_animate":
				{
					List<object> list = value.StringToList(';');
					letter_anim.m_letters_to_animate = new List<int>();
					if (list == null)
					{
						break;
					}
					foreach (object item in list)
					{
						letter_anim.m_letters_to_animate.Add(int.Parse(item.ToString()));
					}
					break;
				}
				case "m_letters_to_animate_custom_idx":
					letter_anim.m_letters_to_animate_custom_idx = int.Parse(value);
					break;
				case "m_letters_to_animate_option":
					letter_anim.m_letters_to_animate_option = (LETTERS_TO_ANIMATE)int.Parse(value);
					break;
				case "LOOP_DATA_START":
					if (num == letter_anim.NumLoops)
					{
						letter_anim.AddLoop();
					}
					break;
				case "LOOP_DATA_END":
					num++;
					break;
				case "m_delay_first_only":
					letter_anim.GetLoop(num).m_delay_first_only = bool.Parse(value);
					break;
				case "m_end_action_idx":
					letter_anim.GetLoop(num).m_end_action_idx = int.Parse(value);
					break;
				case "m_loop_type":
					letter_anim.GetLoop(num).m_loop_type = (LOOP_TYPE)int.Parse(value);
					break;
				case "m_number_of_loops":
					letter_anim.GetLoop(num).m_number_of_loops = int.Parse(value);
					break;
				case "m_start_action_idx":
					letter_anim.GetLoop(num).m_start_action_idx = int.Parse(value);
					break;
				case "ACTION_DATA_START":
					if (num2 == letter_anim.NumActions)
					{
						letter_anim.AddAction();
					}
					i = letter_anim.GetAction(num2).ImportLegacyData(data_list, i + 1);
					num2++;
					break;
				}
			}
			if (letter_anim.NumLoops > num)
			{
				letter_anim.RemoveLoops(num, letter_anim.NumLoops - num);
			}
			if (letter_anim.NumActions > num2)
			{
				letter_anim.RemoveActions(num2, letter_anim.NumActions - num2);
			}
			return i;
		}

		public static int ImportLegacyData(this LetterAction letter_action, List<object> data_list, int index_offset = 0)
		{
			letter_action.ClearAudioEffectSetups();
			letter_action.ClearParticleEffectSetups();
			AudioEffectSetup audioEffectSetup = null;
			ParticleEffectSetup particleEffectSetup = null;
			int i;
			for (i = index_offset; i < data_list.Count; i++)
			{
				KeyValuePair<string, string> keyValuePair = (KeyValuePair<string, string>)data_list[i];
				string key = keyValuePair.Key;
				string value = keyValuePair.Value;
				if (key.Equals("ACTION_DATA_END"))
				{
					letter_action.m_colour_transition_active = true;
					letter_action.m_position_transition_active = true;
					letter_action.m_local_scale_transition_active = true;
					letter_action.m_local_rotation_transition_active = true;
					letter_action.m_global_scale_transition_active = true;
					letter_action.m_global_rotation_transition_active = true;
					break;
				}
				switch (key)
				{
				case "m_action_type":
					letter_action.m_action_type = (ACTION_TYPE)int.Parse(value);
					break;
				case "m_ease_type":
					letter_action.m_ease_type = (EasingEquation)int.Parse(value);
					break;
				case "m_force_same_start_time":
					letter_action.m_force_same_start_time = bool.Parse(value);
					break;
				case "m_letter_anchor":
					letter_action.m_letter_anchor_start = int.Parse(value);
					letter_action.m_letter_anchor_2_way = false;
					break;
				case "m_letter_anchor_start":
					letter_action.m_letter_anchor_start = int.Parse(value);
					break;
				case "m_letter_anchor_end":
					letter_action.m_letter_anchor_end = int.Parse(value);
					break;
				case "m_letter_anchor_2_way":
					letter_action.m_letter_anchor_2_way = bool.Parse(value);
					break;
				case "m_offset_from_last":
					letter_action.m_offset_from_last = bool.Parse(value);
					break;
				case "m_position_axis_ease_data":
					letter_action.m_position_axis_ease_data.ImportLegacyData(value);
					break;
				case "m_rotation_axis_ease_data":
					letter_action.m_rotation_axis_ease_data.ImportLegacyData(value);
					break;
				case "m_scale_axis_ease_data":
					letter_action.m_scale_axis_ease_data.ImportLegacyData(value);
					break;
				case "m_start_colour":
					letter_action.m_start_colour.ImportLegacyData(value);
					break;
				case "m_end_colour":
					letter_action.m_end_colour.ImportLegacyData(value);
					break;
				case "m_start_euler_rotation":
					letter_action.m_start_euler_rotation.ImportLegacyData(value);
					break;
				case "m_end_euler_rotation":
					letter_action.m_end_euler_rotation.ImportLegacyData(value);
					break;
				case "m_start_pos":
					letter_action.m_start_pos.ImportLegacyData(value);
					break;
				case "m_end_pos":
					letter_action.m_end_pos.ImportLegacyData(value);
					break;
				case "m_start_scale":
					letter_action.m_start_scale.ImportLegacyData(value);
					break;
				case "m_end_scale":
					letter_action.m_end_scale.ImportLegacyData(value);
					break;
				case "m_delay_progression":
					letter_action.m_delay_progression.ImportLegacyData(value);
					break;
				case "m_duration_progression":
					letter_action.m_duration_progression.ImportLegacyData(value);
					break;
				case "m_audio_on_start":
					if (value.PathToAudioClip() != null)
					{
						AudioEffectSetup audioEffectSetup2 = new AudioEffectSetup();
						audioEffectSetup2.m_audio_clip = value.PathToAudioClip();
						audioEffectSetup2.m_play_when = PLAY_ITEM_EVENTS.ON_START;
						audioEffectSetup2.m_effect_assignment = PLAY_ITEM_ASSIGNMENT.PER_LETTER;
						audioEffectSetup2.m_loop_play_once = false;
						audioEffectSetup = audioEffectSetup2;
					}
					break;
				case "m_audio_on_start_delay":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_delay.ImportLegacyData(value);
					}
					break;
				case "m_audio_on_start_offset":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_offset_time.ImportLegacyData(value);
					}
					break;
				case "m_audio_on_start_pitch":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_pitch.ImportLegacyData(value);
					}
					break;
				case "m_audio_on_start_volume":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_volume.ImportLegacyData(value);
						letter_action.AddAudioEffectSetup(audioEffectSetup);
						audioEffectSetup = null;
					}
					break;
				case "m_audio_on_finish":
					if (value.PathToAudioClip() != null)
					{
						AudioEffectSetup audioEffectSetup2 = new AudioEffectSetup();
						audioEffectSetup2.m_audio_clip = value.PathToAudioClip();
						audioEffectSetup2.m_play_when = PLAY_ITEM_EVENTS.ON_FINISH;
						audioEffectSetup2.m_effect_assignment = PLAY_ITEM_ASSIGNMENT.PER_LETTER;
						audioEffectSetup2.m_loop_play_once = false;
						audioEffectSetup = audioEffectSetup2;
					}
					break;
				case "m_audio_on_finish_delay":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_delay.ImportLegacyData(value);
					}
					break;
				case "m_audio_on_finish_offset":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_offset_time.ImportLegacyData(value);
					}
					break;
				case "m_audio_on_finish_pitch":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_pitch.ImportLegacyData(value);
					}
					break;
				case "m_audio_on_finish_volume":
					if (audioEffectSetup != null)
					{
						audioEffectSetup.m_volume.ImportLegacyData(value);
						letter_action.AddAudioEffectSetup(audioEffectSetup);
						audioEffectSetup = null;
					}
					break;
				case "m_emitter_on_start_delay":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_delay.ImportLegacyData(value);
					}
					break;
				case "m_emitter_on_start_duration":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_duration.ImportLegacyData(value);
					}
					break;
				case "m_emitter_on_start_follow_mesh":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_follow_mesh = bool.Parse(value);
					}
					break;
				case "m_emitter_on_start_offset":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_position_offset.ImportLegacyData(value);
					}
					break;
				case "m_emitter_on_start_per_letter":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_effect_assignment = ((!bool.Parse(value)) ? PLAY_ITEM_ASSIGNMENT.CUSTOM : PLAY_ITEM_ASSIGNMENT.PER_LETTER);
						if (particleEffectSetup.m_effect_assignment == PLAY_ITEM_ASSIGNMENT.CUSTOM)
						{
							particleEffectSetup.m_effect_assignment_custom_letters = new List<int> { 0 };
						}
						letter_action.AddParticleEffectSetup(particleEffectSetup);
						particleEffectSetup = null;
					}
					break;
				case "m_emitter_on_finish_delay":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_delay.ImportLegacyData(value);
					}
					break;
				case "m_emitter_on_finish_duration":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_duration.ImportLegacyData(value);
					}
					break;
				case "m_emitter_on_finish_follow_mesh":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_follow_mesh = bool.Parse(value);
					}
					break;
				case "m_emitter_on_finish_offset":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_position_offset.ImportLegacyData(value);
					}
					break;
				case "m_emitter_on_finish_per_letter":
					if (particleEffectSetup != null)
					{
						particleEffectSetup.m_effect_assignment = ((!bool.Parse(value)) ? PLAY_ITEM_ASSIGNMENT.CUSTOM : PLAY_ITEM_ASSIGNMENT.PER_LETTER);
						if (particleEffectSetup.m_effect_assignment == PLAY_ITEM_ASSIGNMENT.CUSTOM)
						{
							particleEffectSetup.m_effect_assignment_custom_letters = new List<int> { 0 };
						}
						letter_action.AddParticleEffectSetup(particleEffectSetup);
						particleEffectSetup = null;
					}
					break;
				}
			}
			return i;
		}

		public static void ImportLegacyData(this AxisEasingOverrideData axis_data, string data_string)
		{
			string[] array = data_string.Split('|');
			if (int.Parse(array[0]) == 1)
			{
				axis_data.m_override_default = true;
				axis_data.m_x_ease = (EasingEquation)int.Parse(array[1]);
				axis_data.m_y_ease = (EasingEquation)int.Parse(array[2]);
				axis_data.m_z_ease = (EasingEquation)int.Parse(array[3]);
			}
			else
			{
				axis_data.m_override_default = false;
			}
		}
	}
}
