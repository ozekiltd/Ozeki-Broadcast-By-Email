namespace OzDemoEmailSender.Model
{
    public enum WorkState
    {
        Init,
        InProgress,
        Routed,
        Success,
        RoutingFailed,
        DeliveringFailed
    }
}
