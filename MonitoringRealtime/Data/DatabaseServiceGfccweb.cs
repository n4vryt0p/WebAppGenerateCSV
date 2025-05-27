namespace MonitoringRealtime.Data
{
    public class DatabaseServiceGfccweb
    {
        private readonly DatabaseService _databaseService;

        public DatabaseServiceGfccweb(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            string sql = "SELECT Id, Name, Email FROM Users";
            return await _databaseService.QueryAsync<User>(sql);
        }

        public async Task<User?> GetUserById(int id)
        {
            string sql = "SELECT Id, Name, Email FROM Users WHERE Id = @Id";
            return await _databaseService.QuerySingleAsync<User>(sql, new { Id = id });
        }

        public async Task<int> CreateUser(User user)
        {
            string sql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
            return await _databaseService.ExecuteAsync(sql, user);
        }
    }
}
