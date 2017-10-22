using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;
using System.Text;

namespace GameCtor.Firebase.Database.Rx
{
    public static partial class DatabaseQueryExtensions
    {
        /// <summary>
        /// Get the relative url of the reference.
        /// </summary>
        /// <param name="dbQuery">The database query reference.</param>
        /// <returns>Returns the reference url minus the root.</returns>
        public static string GetUrl(this Query dbQuery)
        {
            StringBuilder sb = new StringBuilder(dbQuery.Ref.Key);
            var current = dbQuery.Ref.Parent;
            while(current != null && !string.IsNullOrEmpty(current.Key))
            {
                sb.Insert(0, string.Format("{0}/", current.Key));
                current = current.Parent;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Equivalent to AddListenerForSingleValueEvent, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <param name="dbQuery">The database query reference.</param>
        /// <returns>Returns a snapshot of the data at dbQuery.</returns>
        public static IObservable<DataSnapshot> AddListenerForSingleValueEventRx(this Query dbQuery)
        {
            return Observable.Create<DataSnapshot>(observer =>
            {
                var listener = new ValueEventListener(observer);
                dbQuery.AddListenerForSingleValueEvent(listener);

                return Disposable.Empty;
            })
                .Take(1);
        }

        /// <summary>
        /// Equivalent to AddValueEventListener, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <param name="dbQuery">The database query reference.</param>
        /// <param name="listener">A reference to the listener so you can remove it, later.</param>
        /// <returns>Returns a stream of snapshots of the data at dbQuery.</returns>
        public static IObservable<DataSnapshot> AddValueEventListenerRx(this Query dbQuery, out IValueEventListener listener)
        {
            var eventSubject = new Subject<DataSnapshot>();

            listener = dbQuery.AddValueEventListener(new ValueEventListener(eventSubject.AsObserver()));

            return eventSubject.AsObservable();
        }

        /// <summary>
        /// Equivalent to AddChildEventListener, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <param name="dbQuery">The database query reference.</param>
        /// <param name="listener">A reference to the listener so you can remove it, later.</param>
        /// <returns>Returns a stream of results containing a snapshot and event type.</returns>
        public static IObservable<ChildEventResult> AddChildEventListenerRx(this Query dbQuery, out IChildEventListener listener)
        {
            var eventSubject = new Subject<ChildEventResult>();

            listener = dbQuery.AddChildEventListener(new ChildEventListener(eventSubject));

            return eventSubject;
        }
    }
}