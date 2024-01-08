using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Common.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB;

public class DatabaseContext : DbContext
{
    protected readonly DatabaseConfig _configuration;
    public DatabaseContext(IOptions<DatabaseConfig> configuration) : base()
    {
        _configuration = configuration.Value;

        if(_configuration.Type == "TEST")
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();

            _configuration.Type = "MAIN";
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.ConnesctionString);
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    }

    public DbSet<RolesDictionary> RolesDictionaryEntity { get; set; }
    public DbSet<AccountStatusesDictionary> AccountStatusesDictionaryEntity { get; set; }
    public DbSet<PetTraitsDictionary> PetTraitsDictionaryEntity { get; set; }

    public DbSet<Token> TokensEntity { get; set; }
    public DbSet<User> UsersEntity { get; set; }
    public DbSet<UserDetails> UsersDetailsEntity { get; set; }
    public DbSet<LoginData> LoginsDataEntity { get; set; }
    public DbSet<Configuration> ConfigurationsEntity { get; set; }
    public DbSet<Appointment> AppointmentsEntity { get; set; }
    public DbSet<Pet> PetsEntity { get; set; }
    public DbSet<PetTrait> PetTraitsEntity { get; set; }
    public DbSet<Shelter> SheltersEntity { get; set; }
    public DbSet<Worker> WorkersEntity { get; set; }
    public DbSet<Common.Entities.File> FilesEntity { get; set; }
    public DbSet<PetImage> PetImagesEntity { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        SetDictionaries(builder);
        SetTokens(builder);
        SetUsers(builder);
        SetLoginsData(builder);
        SetAppointments(builder);
        SetPets(builder);
        SetShelters(builder);
        SetWorkers(builder);
        SetConfigurations(builder);
        SetPetTraits(builder);
        SetFiles(builder);
        SetPetImages(builder);
    }

    private static void SetDictionaries(ModelBuilder builder)
    {
        RolesDictionary.SetEntity(builder);
        AccountStatusesDictionary.SetEntity(builder);
        PetTraitsDictionary.SetEntity(builder);
    }

    private static void SetTokens(ModelBuilder builder)
    {
        Token.SetEntity(builder);
    }

    private static void SetUsers(ModelBuilder builder)
    {
        User.SetEntity(builder);
        UserDetails.SetEntity(builder);
    }

    private static void SetLoginsData(ModelBuilder builder)
    {
        LoginData.SetEntity(builder);
    }

    private static void SetConfigurations(ModelBuilder builder)
    {
        Configuration.SetEntity(builder);
    }

    private static void SetAppointments(ModelBuilder builder)
    {
        Appointment.SetEntity(builder);
    }

    private static void SetPets(ModelBuilder builder)
    {
        Pet.SetEntity(builder);
    }

    private static void SetShelters(ModelBuilder builder)
    {
        Shelter.SetEntity(builder);
    }

    private static void SetWorkers(ModelBuilder builder)
    {
        Worker.SetEntity(builder);
    }

    private static void SetPetTraits(ModelBuilder builder)
    {
        PetTrait.SetEntity(builder);
    }

    private static void SetFiles(ModelBuilder builder)
    {
        Common.Entities.File.SetEntity(builder);
    }

    private static void SetPetImages(ModelBuilder builder)
    {
        PetImage.SetEntity(builder);
    }
}