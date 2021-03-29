using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invokes the Event Handler with a null check. Event args will be passed as null
        /// </summary>
        /// <param name="sender">The sender that triggered the event. Pass 'this' if not sure</param>
        public static void Trigger(this EventHandler handler, object sender)
        {
            handler?.Invoke(sender, null);
        }

        /// <summary>
        /// Invokes the Event Handler with a null check. Event args will be passed as null
        /// </summary>
        /// <param name="sender">The sender that triggered the event. Pass 'this' if not sure</param>
        /// <param name="args">Any arguments that need to be passed to subscribers. Please use Trigger if no arguments are needed</param>
        public static void TriggerWithData(this EventHandler handler, object sender, EventArgs args)
        {
            handler?.Invoke(sender, args);
        }

        public static void TriggerWithData<T>(this EventHandler<T> handler, object sender, T args)
        {
            handler?.Invoke(sender, args);
        }
    }
}