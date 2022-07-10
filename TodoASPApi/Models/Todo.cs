using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoASPApi.Models {
    public class Todo {

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsCompleted { get; set; }
    }

    public class CreateTodoDto {
        [Required]
        public string Text { get; set; }
    }

    public class UpdateTodoDto {
        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}
