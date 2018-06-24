using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes {
    public partial class Cutscene {
        private static readonly RandomNumberGenerator FutureIDGenerator = RandomNumberGenerator.Create();

        private static int CreateFutureId() {
            var data = new byte[4];
            FutureIDGenerator.GetBytes(data);
            return BitConverter.ToInt32(data, 0);
        }

        private void CheckAndRegisterFutureProvider(Token token) {
            var provider = token as IFutureProvider;
            if (provider != null) {
                provider.RegisterFutures(this);
            }
        }

        public const string DefaultFutureName = "unnamed_future";

        [SerializeField]
        private List<ExpectedFuture> futures = new List<ExpectedFuture>();

        public int TotalFutures {
            get {
                return futures.Count;
            }
        }

        public IList<ExpectedFuture> Futures {
            get {
                return new List<ExpectedFuture>(futures);
            }
        }

        public ExpectedFuture GetFuture(int futureId) {
            return futures.FirstOrDefault(future => future.Id == futureId);
        }

        public ExpectedFuture AddFuture<T>(Token responsible, string tokenName) where T : Object {
            return AddFuture(responsible, tokenName, typeof(T));
        }

        private ExpectedFuture AddFuture(Token responsible, string tokenName, Type expectedType) {
            var future = new ExpectedFuture(tokenName, CreateFutureId(), responsible, expectedType);
            futures.Add(future);
            return future;
        }
    }
}