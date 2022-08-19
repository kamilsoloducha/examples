using Domain.Core;

namespace Domain;

public class Cart : IAggregateRoot
{
    
    public Cart()
    {
        Id = CardId.New();
    }
    
    public CardId Id { get; }
    public IEnumerable<IDomainEvent> DomainEvents { get; }
}

public readonly struct CardId
{
    public Guid Value { get; }

    private CardId(Guid value)
    {
        Value = value;
    }

    internal static CardId New() => new CardId(Guid.NewGuid());

    public static CardId Restore(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("Value cannot be empty", nameof(value));
        
        return new CardId(value);
    }
}

public class Product
{
    public long Id { get; }
    public Price Price { get; }
}

public readonly struct Price
{
    public decimal Value { get; }
    public Currency Currency { get; }
}

public enum Currency
{
    PLN,
    USD,
    EUR
}

public readonly struct Counter
{
    public int Amount { get; }
}

public class CartItem
{
    public Product Product { get; }
    public Counter Counter { get; }
}

public class User
{
    private List<Wallet> _wallets;
    
    public long Id { get; }
    public Cart Cart { get; }
    public IEnumerable<Wallet> Wallets { get; }
}

public class Wallet
{
    public Currency Currency { get; }
    public decimal Amount { get; }
}