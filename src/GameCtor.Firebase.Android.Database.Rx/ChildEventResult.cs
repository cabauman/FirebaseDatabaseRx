using Firebase.Database;

namespace GameCtor.Firebase.Database.Rx
{
    public struct ChildEventResult
    {
        public ChildEventType EventType { get; }

        public DataSnapshot Snapshot { get; }

        public string PreviousChildName { get; }

        public ChildEventResult(ChildEventType eventType, DataSnapshot snapshot, string previousChildName)
        {
            EventType = eventType;
            Snapshot = snapshot;
            PreviousChildName = previousChildName;
        }
    }
}