namespace MessageHelper.Fcm.Model
{
    class FcmOptions
    {
        /// <summary>
        /// Number of connections that can connect to the remote server simultaneously
        /// </summary>
        public int MaxConnectionCount { get; set; }

        /// <summary>
        /// Enables a test environment for testing the FCM from the caller side
        /// </summary>
        public bool TestMode { get; set; }

        /// <summary>
        /// Each FCM project has a server key it uses as the Authorization key
        /// </summary>
        public string ServerKey { get; set; }

        /// <summary>
        /// Each FCM project has a sender id and caller should set it in the request header
        /// </summary>
        public string SenderId { get; set; }
    }
}
