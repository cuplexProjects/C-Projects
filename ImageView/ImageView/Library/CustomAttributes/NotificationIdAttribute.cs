using System;

namespace ImageView.Library.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NotificationIdAttribute : Attribute
    {
        private readonly string _name;
        private readonly Guid _notificationId;

        public NotificationIdAttribute(string name) : this(name, Guid.NewGuid())
        {
            _name = name;
        }

        public NotificationIdAttribute(string name, Guid notificationId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name must be specified");
            }


            _name = name;
            _notificationId = notificationId;
        }



        public override string ToString()
        {
            return $"Name: {Name}, NotificationId: {NotificationId}";
        }

        public string Name => _name;
        public Guid NotificationId => _notificationId;
    }
}
