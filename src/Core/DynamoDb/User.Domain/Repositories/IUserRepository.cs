using User.Domain.Entities;

namespace User.Domain.Repositories;

public interface IUserRepository
{
  Task<bool> CreateAsync(UserEntity user);

  Task<UserEntity?> GetAsync(string externalAccount, string externalAccountId, string email);

  /// <summary>
  ///   LSI/GSI sample
  /// </summary>
  /// <returns></returns>
  Task<UserEntity?> GetByUserNameAsync(string userName);

  /// <summary>
  ///   NOT RECOMMENDED Without pk or sk
  /// </summary>
  /// <returns></returns>
  Task<IEnumerable<UserEntity>> GetAllAsync();

  Task<bool> UpdateAsync(UserEntity user, DateTime requestStarted);

  //Task<bool> DeleteAsync(Guid id);
}