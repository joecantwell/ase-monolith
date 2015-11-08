
using Akka.Actor;


namespace ActorUI.Actors
{
    /// <summary>
    /// Static class to reference High level Actors
    /// https://petabridge.com/blog/akkadotnet-aspnet/
    /// </summary>
    public static class SystemActors
    {
        public static IActorRef VehicleActor = ActorRefs.Nobody;

        public static IActorRef QuoteActor = ActorRefs.Nobody;
    }

    public class ActorSystemRefs
    {
        public static ActorSystem ActorSystem;
    }
}
