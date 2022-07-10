using Microsoft.EntityFrameworkCore;
using TodoASPApi.Models;

namespace TodoASPApi.Services {
    public interface ITodosService {
        public Task<Todo> Create(CreateTodoDto dto);
        public Task<IEnumerable<Todo>> GetMany(int offset);
        public Task<Todo?> GetById(int id);
        public Task<Todo?> Update(int id, UpdateTodoDto dto);
        public Task<bool> Delete(int id);
    }

    public class TodosService : ITodosService {
        private readonly TodoAppContext _dbContext;
        public TodosService(TodoAppContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Todo> Create(CreateTodoDto dto) {
            var date = DateTime.UtcNow;
            Todo todo = new Todo {
                Text = dto.Text,
                CreatedDate = date,
                UpdatedDate = date,
            };

            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            return todo;
        }

        public async Task<IEnumerable<Todo>> GetMany(int offset) {
            return await _dbContext.Todos.OrderByDescending(t => t.CreatedDate).Take(15).Skip(offset).ToListAsync();
        }

        public async Task<Todo?> GetById(int id) {
            return await _dbContext.Todos.FindAsync(id);
        }

        public async Task<Todo?> Update(int id, UpdateTodoDto dto) {
            var todo = await GetById(id);
            if (todo == null) return null;

            todo.Text = dto.Text;
            todo.UpdatedDate = DateTime.UtcNow;
            todo.IsCompleted = dto.IsCompleted;

            _dbContext.Todos.Update(todo);
            await _dbContext.SaveChangesAsync();

            return todo;
        }

        public async Task<bool> Delete(int id) {
            var todo = await GetById(id);
            if (todo == null) return false;

            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
