namespace ESD.Models.Dtos.Common
{
    public class ExpoPushNotificationData
    {
        //public IList<string> to { get; set; }
        //public string title { get; set; }
        //public string body { get; set; }

        //public ExpoPushNotificationData()
        //{
        //    to = new List<string>();
        //    title = string.Empty;
        //    body = string.Empty;
        //}


        public IList<string> registration_ids { get; set; }
        public FCMNotification notification { get; set; }

        public ExpoPushNotificationData()
        {
            registration_ids = new List<string>();
            notification = new FCMNotification();
        }
    }

    public class FCMNotification
    {
        public string title { get; set; }
        public string body { get; set; }
        public FCMNotification()
        {
            title = "Hanlim Inform";
            body = "New application version is coming";
        }
    }

    public class FCMNotificationData
    {
        public string experienceId { get; set; }
        public string scopeKey { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
    }

}
