namespace IRON_PROGRAMMER_BOT_Common.User
{
    public sealed class UserData
    {
        public string? StepikId { get; set; }

        public MessageData? LastMessage { get; set; }


        public override string ToString()
        {
            return $"{StepikId} = stepikId";
        }
    }
}
