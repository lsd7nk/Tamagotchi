using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using Extensions;
using System;

namespace Save.State
{
    public sealed class GlobalStateHolder : StateHolder<GlobalState>
    {
        public override string Id => "Global";
    }


    public sealed class GlobalState : IState
    {
        public float PlayTimeSeconds;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public async Task<string> ToStringAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var task = this.SerializeJsonAsync(ct: ct);

            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
#if UNITY_EDITOR
                Debug.LogError($"{GetType()}: ToStringAsync cancelled. Message: {e.Message}");
#endif
                return string.Empty;
            }

            return task.Result;
        }
    }
}