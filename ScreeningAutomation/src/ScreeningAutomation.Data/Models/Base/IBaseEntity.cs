namespace ScreeningAutomation.Data.Models.Base
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        bool IsNew();
    }
}
