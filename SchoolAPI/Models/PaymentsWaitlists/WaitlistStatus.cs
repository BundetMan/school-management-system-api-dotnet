namespace SchoolAPI.Models.PaymentsWaitlists
{
    public enum WaitlistStatus
    {
        Waiting,    // Still in queue
        Promoted,   // Moved to registered
        Cancelled   // Dropped out of queue
    }
}
