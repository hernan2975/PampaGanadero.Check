using PampaGanadero.Core.ValueObjects;
using PampaGanadero.Core.Enums;

namespace PampaGanadero.Core.Entities;

public class EarTag
{
    public UHFRFID Uid { get; }
    public SenasaId SenasaNumber { get; }
    public DateTime IssueDate { get; }
    public string IssuingEntity { get; }
    public TagStatus Status { get; private set; }

    private EarTag(UHFRFID uid, SenasaId number, DateTime issueDate, string entity)
    {
        Uid = uid;
        SenasaNumber = number;
        IssueDate = issueDate;
        IssuingEntity = entity;
        Status = TagStatus.Unknown;
    }

    public static EarTag Create(string uidHex, string senasaNumber, DateTime issueDate, string entity)
    {
        var uid = UHFRFID.FromHex(uidHex);
        var num = SenasaId.Parse(senasaNumber);
        return new EarTag(uid, num, issueDate, entity);
    }

    public void UpdateStatus(TagStatus status) => Status = status;
}
