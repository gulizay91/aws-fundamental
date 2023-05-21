using Contracts.Commands.V1.User;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using User.Application.ViewModels;
using User.Domain.Entities;

namespace User.Application.Registrations;

public static class MapperRegister
{
  public static void RegisterMapper(this IServiceCollection serviceCollection)
  {
    TypeAdapterConfig<CreateUser, UserEntity>.NewConfig()
      .Map(dest => dest.Id,
        src => Guid.NewGuid())
      .Map(dest => dest.UpdatedAt,
        src => DateTime.UtcNow);

    TypeAdapterConfig<UserEntity, UserViewModel>.NewConfig();
  }
}