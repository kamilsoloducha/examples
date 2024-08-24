namespace DbTransactions.Model;

public class Transfer
{
    public long ContractNumber { get; private set; } = 0;
    public long SourceAccount { get; init; }
    public long DestinationAccount { get; init; }
    public decimal Shares { get; init; }
    public TransferStatus Status { get; set; } = TransferStatus.Initiated;
}

public enum TransferStatus
{
    Initiated,
    Created,
    Failed
}
