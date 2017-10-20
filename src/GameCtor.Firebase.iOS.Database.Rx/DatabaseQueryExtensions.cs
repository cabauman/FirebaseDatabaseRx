using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;
using Foundation;

namespace GameCtor.Firebase.Database.Rx
{
    public static class DatabaseQueryExtensions
    {
        /// <summary>
        /// Equivalent to ObserveSingleEvent, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <param name="dbQuery">The database query reference.</param>
        /// <param name="eventType">The type of event you want to observe.</param>
        /// <returns>Returns a snapshot of the data at dbQuery.</returns>
        public static IObservable<DataSnapshot> ObserveSingleEventRx(this DatabaseQuery dbQuery, DataEventType eventType)
        {
            return Observable.Create<DataSnapshot>(observer =>
            {
                dbQuery.ObserveSingleEvent(
                    eventType,
                    snapshot =>
                    {
                        observer.OnNext(snapshot);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        observer.OnError(new NSErrorException(error));
                    });

                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Equivalent to ObserveEvent, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <param name="dbQuery">The database query reference.</param>
        /// <param name="eventType">The type of event you want to observe.</param>
        /// <param name="handle">A handle to the listener so you can remove it, later.</param>
        /// <returns>Returns a stream of snapshots of the data at dbQuery.</returns>
        public static IObservable<DataSnapshot> ObserveEventRx(this DatabaseQuery dbQuery, DataEventType eventType, out nuint handle)
        {
            var eventSubject = new Subject<DataSnapshot>();

            handle = dbQuery.ObserveEvent(
                eventType,
                snapshot =>
                {
                    eventSubject.OnNext(snapshot);
                },
                error =>
                {
                    eventSubject.OnError(new NSErrorException(error));
                });

            return eventSubject.AsObservable();
        }
    }
}