using Dapper;
using Models;
using Todos.Repositories;
using Todos.Utilities;

namespace Todos.Repositories;

public interface IUserRepository
{


    Task<User> Create(User Item);
    Task<User> GetById(long Id);

    Task<User> GetUsername(string Username);
}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }


    public async Task<User> Create(User Item)
    {
        var query = $@"INSERT INTO ""{TableNames.users}""
        (username,name, email, mobile, password)
        VALUES (@UserName,@Name, @Email, @Mobile, @Password)
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, Item);
            return res;
        }

    }

    public async Task<User> GetUsername(string Username)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
       WHERE username = @UserName";

           //List<User> res;
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Username });



    }

    public async Task<User> GetById(long Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
        WHERE id = @Id";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Id });
    }

}
