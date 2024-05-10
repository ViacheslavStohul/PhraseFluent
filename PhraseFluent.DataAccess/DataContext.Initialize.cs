using PhraseFluent.DataAccess.Entities;

namespace PhraseFluent.DataAccess;

public partial class DataContext 
{
    public async Task Initialize()
    {
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            var transactions = CompleteInitializers.Select(x => x.CompleteInitializerId);
            await AddInitialLanguages(transactions);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task AddInitialLanguages(IEnumerable<Guid> completedTransactions)
    {
        var id = new Guid("825347D1-2B7B-4E4A-BD3D-C4F01EDCB486");
        
        if (completedTransactions.All(x => x != id))
        {
            var languagesToAdd = new List<Language>()
            {
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "English",
                    NormalizedTitle = "ENGLISH",
                    NativeName = "English",
                    NormalizedNativeName = "ENGLISH",
                    LanguageCode = "EN"
                },
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "Ukrainian",
                    NormalizedTitle = "UKRAINIAN",
                    NativeName = "Українська",
                    NormalizedNativeName = "УКРАЇНСЬКА",
                    LanguageCode = "UK"
                },
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "Italian",
                    NormalizedTitle = "ITALIAN",
                    NativeName = "Italiano",
                    NormalizedNativeName = "ITALIANO",
                    LanguageCode = "IT"
                },
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "German",
                    NormalizedTitle = "GERMAN",
                    NativeName = "Deutsch",
                    NormalizedNativeName = "DEUTSCH",
                    LanguageCode = "DE"
                },
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "French",
                    NormalizedTitle = "FRENCH",
                    NativeName = "Français",
                    NormalizedNativeName = "FRANÇAIS",
                    LanguageCode = "FR"
                },
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "Spanish",
                    NormalizedTitle = "SPANISH",
                    NativeName = "Español",
                    NormalizedNativeName = "ESPAÑOL",
                    LanguageCode = "ES"
                },
                new()
                {
                    Uuid = Guid.NewGuid(),
                    Title = "Russian",
                    NormalizedTitle = "RUSSIAN",
                    NativeName = "Русский",
                    NormalizedNativeName = "РУССКИЙ",
                    LanguageCode = "RU"
                }
            };
            
            Languages.AddRange(languagesToAdd);

            CompleteInitializers.Add(new CompleteInitializer
            {
                CompleteInitializerId = id,
                CompleteInitializerName = "Initial languages"
            });
            
            await SaveChangesAsync();
        }
    }
}