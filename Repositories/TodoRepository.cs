using Dapper;
using Models;
using Todos.Models;
using Todos.Repositories;
using Todos.Utilities;

namespace Todos.Repositories;

public interface ITodoRepository
{
    Task<Todo> Create(Todo Item);
    Task<bool> Update(Todo Item);
    Task<bool> Delete(long Id);
    Task<Todo> GetById(long Id);
    Task<List<Todo>> GetList();
    Task<List<Todo>> GetMyTodos(long UserId);
}
public class TodoRepository : BaseRepository, ITodoRepository
{
    public TodoRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Todo> Create(Todo Item)
    {

        var query = $@"INSERT INTO ""{TableNames.todo}""
        (user_id,is_completed,description)
        VALUES (@UserId,@IsCompleted,@Description)
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Todo>(query, Item);
            return res;
        }
    }


    public async Task<bool> Delete(long Id)
    {
         var query = $@"DELETE FROM ""{TableNames.todo}""
        WHERE id = @Id";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { Id });
            return res > 0;
        }
    }

    public async Task<Todo> GetById(long Id)
    {
          var query = $@"SELECT * FROM ""{TableNames.todo}""
        WHERE id = @Id";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Todo>(query, new { Id });

    }

    public async Task<List<Todo>> GetList()
    {
   var query = $@"SELECT * FROM ""{TableNames.todo}"" ORDER BY Id";

        List<Todo> res;
        using (var con = NewConnection) // Open connection
            res = (await con.QueryAsync<Todo>(query)).AsList(); // Execute the query
        // Close the connection

        // Return the result
        return res;
    }

    public async Task<List<Todo>> GetMyTodos(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.todo}"" WHERE user_id = @UserId";
        using (var con = NewConnection)
            return (await con.QueryAsync<Todo>(query, new { UserId })).ToList();
    }

    public async Task<bool> Update(Todo Item)
     {
         var query = $@"UPDATE ""{TableNames.todo}"" SET
         is_completed = @IsCompleted  WHERE id = @Id";


         using (var con = NewConnection)
         {
             var rowCount = await con.ExecuteAsync(query, Item);
             return rowCount == 1;
         }
     }



}