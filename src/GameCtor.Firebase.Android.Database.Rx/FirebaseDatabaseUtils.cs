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
        public static IObservable<Dictionary<string, DataSnapshot>> AddListenerForSingleZippedEvent(params Query[] dbQueries)
        {
            var observables = new List<IObservable<DataSnapshot>>();

            foreach(var query in dbQueries)
            {
                var observable = query.AddListenerForSingleValueEventRx()
                    .SubscribeOn(Scheduler.Default);
                observables.Add(observable);
            }

            return observables
                .Zip()
                .Select(snapshots =>
                {
                    return dbQueries.Zip(snapshots, (query, snap) => new { Key = query.Ref.Key, Value = snap }) // ************** Need to check key value
                        .ToDictionary(x => x.Key, x => x.Value);
                });
        }
    }
}