using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{
    public class HeroViewModelV2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Place { get; set; }
    }
}
