using System;
using System.Collections.Generic;

namespace Spine
{
	internal class EventQueue
	{
		private struct EventQueueEntry
		{
			public EventType type;

			public TrackEntry entry;

			public Event e;

			public EventQueueEntry(EventType eventType, TrackEntry trackEntry, Event e = null)
			{
				type = eventType;
				entry = trackEntry;
				this.e = e;
			}
		}

		private enum EventType
		{
			Start = 0,
			Interrupt = 1,
			End = 2,
			Dispose = 3,
			Complete = 4,
			Event = 5
		}

		private readonly List<EventQueueEntry> eventQueueEntries = new List<EventQueueEntry>();

		public bool drainDisabled;

		private readonly AnimationState state;

		private readonly Pool<TrackEntry> trackEntryPool;

		public event Action AnimationsChanged;

		public EventQueue(AnimationState state, Action HandleAnimationsChanged, Pool<TrackEntry> trackEntryPool)
		{
			this.state = state;
			AnimationsChanged += HandleAnimationsChanged;
			this.trackEntryPool = trackEntryPool;
		}

		public void Start(TrackEntry entry)
		{
			eventQueueEntries.Add(new EventQueueEntry(EventType.Start, entry));
			if (this.AnimationsChanged != null)
			{
				this.AnimationsChanged();
			}
		}

		public void Interrupt(TrackEntry entry)
		{
			eventQueueEntries.Add(new EventQueueEntry(EventType.Interrupt, entry));
		}

		public void End(TrackEntry entry)
		{
			eventQueueEntries.Add(new EventQueueEntry(EventType.End, entry));
			if (this.AnimationsChanged != null)
			{
				this.AnimationsChanged();
			}
		}

		public void Dispose(TrackEntry entry)
		{
			eventQueueEntries.Add(new EventQueueEntry(EventType.Dispose, entry));
		}

		public void Complete(TrackEntry entry)
		{
			eventQueueEntries.Add(new EventQueueEntry(EventType.Complete, entry));
		}

		public void Event(TrackEntry entry, Event e)
		{
			eventQueueEntries.Add(new EventQueueEntry(EventType.Event, entry, e));
		}

		public void Drain()
		{
			if (drainDisabled)
			{
				return;
			}
			drainDisabled = true;
			List<EventQueueEntry> list = eventQueueEntries;
			AnimationState animationState = state;
			for (int i = 0; i < list.Count; i++)
			{
				EventQueueEntry eventQueueEntry = list[i];
				TrackEntry entry = eventQueueEntry.entry;
				switch (eventQueueEntry.type)
				{
				case EventType.Start:
					entry.OnStart();
					animationState.OnStart(entry);
					break;
				case EventType.Interrupt:
					entry.OnInterrupt();
					animationState.OnInterrupt(entry);
					break;
				case EventType.End:
					entry.OnEnd();
					animationState.OnEnd(entry);
					goto case EventType.Dispose;
				case EventType.Dispose:
					entry.OnDispose();
					animationState.OnDispose(entry);
					trackEntryPool.Free(entry);
					break;
				case EventType.Complete:
					entry.OnComplete();
					animationState.OnComplete(entry);
					break;
				case EventType.Event:
					entry.OnEvent(eventQueueEntry.e);
					animationState.OnEvent(entry, eventQueueEntry.e);
					break;
				}
			}
			eventQueueEntries.Clear();
			drainDisabled = false;
		}

		public void Clear()
		{
			eventQueueEntries.Clear();
		}
	}
}
