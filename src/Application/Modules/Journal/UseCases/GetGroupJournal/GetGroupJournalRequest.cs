namespace Application.Modules.Journal.UseCases.GetGroupJournal;

// Запрос на получение журнала группы
public class GetGroupJournalRequest
{
    public Guid GroupId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
