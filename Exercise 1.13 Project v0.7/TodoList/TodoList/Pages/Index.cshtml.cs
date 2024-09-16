using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoList.Models;

namespace TodoList.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IReadOnlyCollection<Todo> Todos;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Todos = new List<Todo>
            {
                new Todo
                {
                    Title = "I need to complete the first assignment",
                },
                new Todo
                {
                    Title = "I need to to go to helsinki airport",
                }
            };
        }

        [BindProperty]
        public Todo Todo { get; set; }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {

        }
    }
}
