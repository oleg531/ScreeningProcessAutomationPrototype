namespace ScreeningAutomation.Data.Models.Base
{
    using System;

    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
