using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Firebase.Database;

namespace GameCtor.Firebase.Database.Rx
{
    public class FirebaseDatabaseUtils
    {
        /// <summary>
        /// Calls ObserveSingleEventRx on each reference and returns
        /// a dictionary of DataSnapshots that can be accessed via the reference url.
        /// </summary>
        /// <remarks>
        /// Only returns once, after all snapshots are received.
        /// SubscribeOn is used internally to allow for concurrency.
        /// </remarks>
        /// <param name="dbQueries"></param>
        /// <returns>Returns a dictionary of DataSnapshots that can be accessed via the reference url.</returns>
        public static IObservable<IDictionary<string, DataSnapshot>> AddListenerForSingleZippedEvent(params Query[] dbQueries)
        {
            IObservable<Query> dbQueryObservable = dbQueries
                .ToObservable();

            IObservable<DataSnapshot> snapshotObservable = dbQueryObservable
                .SelectMany(x => x.AddListenerForSingleValueEventRx());

            return Observable
                .Zip(
                    dbQueryObservable,
                    snapshotObservable,
                    (query, snap) => new { Key = query.GetUrl(), Value = snap })
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}