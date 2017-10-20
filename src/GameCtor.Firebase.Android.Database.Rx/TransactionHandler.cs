using Firebase.Database;

namespace GameCtor.Firebase.Database.Rx
{
    /// <summary>
    /// A convenience class for use with DatabaseReference's RunTransaction method.
    /// Allows you to use lambdas instead of having to create a new class that implements Transaction.IHandler.
    /// </summary>
    public sealed class TransactionHandler : Java.Lang.Object, Transaction.IHandler
    {
        public delegate Transaction.Result DoTransactionHandler(MutableData currentData);
        public delegate void OnCompleteHandler(DatabaseError error, bool committed, DataSnapshot snapshot);

        private DoTransactionHandler _doTransaction;
        private OnCompleteHandler _onComplete;

        public TransactionHandler(DoTransactionHandler doTransaction, OnCompleteHandler onComplete)
        {
            _doTransaction = doTransaction;
            _onComplete = onComplete;
        }

        public Transaction.Result DoTransaction(MutableData currentData)
        {
            return _doTransaction(currentData);
        }

        public void OnComplete(DatabaseError error, bool committed, DataSnapshot currentData)
        {
            _onComplete(error, committed, currentData);
        }
    }
}