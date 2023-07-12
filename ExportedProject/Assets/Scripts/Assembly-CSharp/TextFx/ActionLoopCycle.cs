using System;
using Boomlagoon.TextFx.JSON;

namespace TextFx
{
	[Serializable]
	public class ActionLoopCycle
	{
		public int m_start_action_idx;

		public int m_end_action_idx;

		public int m_number_of_loops;

		public LOOP_TYPE m_loop_type;

		public bool m_delay_first_only;

		public bool m_finish_at_end = true;

		public int m_active_loop_index = -1;

		private bool m_first_pass = true;

		public bool FirstPass
		{
			get
			{
				return m_first_pass;
			}
			set
			{
				m_first_pass = value;
			}
		}

		public int SpanWidth
		{
			get
			{
				return m_end_action_idx - m_start_action_idx;
			}
		}

		public ActionLoopCycle()
		{
		}

		public ActionLoopCycle(int start, int end)
		{
			m_start_action_idx = start;
			m_end_action_idx = end;
		}

		public ActionLoopCycle(int start, int end, int num_loops, LOOP_TYPE loop_type)
		{
			m_start_action_idx = start;
			m_end_action_idx = end;
			m_number_of_loops = num_loops;
			m_loop_type = loop_type;
		}

		public ActionLoopCycle Clone(int loop_index = -1)
		{
			ActionLoopCycle actionLoopCycle = new ActionLoopCycle(m_start_action_idx, m_end_action_idx);
			actionLoopCycle.m_active_loop_index = ((loop_index < 0) ? m_active_loop_index : loop_index);
			actionLoopCycle.m_number_of_loops = m_number_of_loops;
			actionLoopCycle.m_loop_type = m_loop_type;
			actionLoopCycle.m_delay_first_only = m_delay_first_only;
			actionLoopCycle.m_finish_at_end = m_finish_at_end;
			return actionLoopCycle;
		}

		public tfxJSONValue ExportData()
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["m_finish_at_end"] = m_finish_at_end;
			tfxJSONObject["m_delay_first_only"] = m_delay_first_only;
			tfxJSONObject["m_end_action_idx"] = m_end_action_idx;
			tfxJSONObject["m_loop_type"] = (double)m_loop_type;
			tfxJSONObject["m_number_of_loops"] = m_number_of_loops;
			tfxJSONObject["m_start_action_idx"] = m_start_action_idx;
			return new tfxJSONValue(tfxJSONObject);
		}

		public void ImportData(tfxJSONObject json_data)
		{
			if (json_data.ContainsKey("m_finish_at_end"))
			{
				m_finish_at_end = json_data["m_finish_at_end"].Boolean;
			}
			else
			{
				m_finish_at_end = false;
			}
			m_delay_first_only = json_data["m_delay_first_only"].Boolean;
			m_end_action_idx = (int)json_data["m_end_action_idx"].Number;
			m_loop_type = (LOOP_TYPE)json_data["m_loop_type"].Number;
			m_number_of_loops = (int)json_data["m_number_of_loops"].Number;
			m_start_action_idx = (int)json_data["m_start_action_idx"].Number;
		}
	}
}
