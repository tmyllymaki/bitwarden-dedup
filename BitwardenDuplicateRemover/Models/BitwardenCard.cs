namespace BitwardenDuplicateRemover.Models;

public record BitwardenCard(string CardholderName, string Brand, string Number, string ExpMonth, string ExpYear,
    string Code);