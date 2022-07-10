using Microsoft.EntityFrameworkCore;
using TodoASPApi.Models;

namespace TodoASPApi {
    public class TodoAppContext : DbContext {
        public DbSet<Todo> Todos { get; set; }
        public TodoAppContext(DbContextOptions<TodoAppContext> options) : base(options) { }
    }
}
