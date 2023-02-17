using Dapper;
using EfCoreDdd.Dtos;
using Npgsql;

namespace EfCoreDdd.Infrastructure.Dapper.Repositories;

public class QueryRepository
{
    private const string ConnectionString =
        "User ID=root;Password=changeme;Host=localhost;Port=5432;Database=myDataBase;";

    public async Task<IEnumerable<GroupDto>> GetGroups(long ownerId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        const string sql = $"SELECT g.\"Id\" {nameof(GroupDto.Id)}, g.\"Name\" {nameof(GroupDto.Name)}, count(c.*) {nameof(GroupDto.CardsCount)} FROM ddd.\"Groups\" g left join ddd.\"Cards\" c ON c.\"GroupId\" = g.\"Id\" where g.\"OwnerId\" = @OwnerId group by g.\"Id\"";


        var result = await connection.QueryAsync<GroupDto>(sql, new { OwnerId = ownerId });
        return result;
    }
}