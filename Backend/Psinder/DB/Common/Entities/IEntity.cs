using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Common.Entities;

public interface IEntity
{
    public static void ConfigureEntity(ModelBuilder builder) => throw new NotImplementedException();
    public static void SeedEntity(ModelBuilder builder) => throw new NotImplementedException();
}
